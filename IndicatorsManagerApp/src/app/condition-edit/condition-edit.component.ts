import {FlatTreeControl} from '@angular/cdk/tree';
import {Component, Injectable, AfterViewInit} from '@angular/core';
import {MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import {BehaviorSubject} from 'rxjs';
import { Operators } from 'src/enums';
import { ConditionModel, ComponentModel, StringItem, IntItem, DateItem, BooleanItem } from 'src/models';

/**
 * Node for to-do item
 */
export class TodoItemNode {
  item: string;
  children: TodoItemNode[];
  position: number; //index in parent array [Root => position 0]
  type: string; //selectedConditionType child property
  operator?: Operators;
  operatorLabel: string;
  value: any;
}

/** Flat to-do item node with expandable and level information */
export class TodoItemFlatNode {
  item: string;
  level: number;
  expandable: boolean;
}

/**
 * The Json object for to-do list data.
 */
const TREE_DATA = {
  Root: {
    Root_LeftCondition: {},
    Root_RightCondition: {}
  }
};

/**
 * Checklist database, it can build a tree structured Json object.
 * Each node in Json object represents a to-do item or a category.
 * If a node is a category, it has children items and new items can be added under the category.
 */
@Injectable()
export class ChecklistDatabase {
  dataChange = new BehaviorSubject<TodoItemNode[]>([]);

  get data(): TodoItemNode[] { return this.dataChange.value; }

  constructor() {
    this.initialize();
  }

  initialize() {
    // Build the tree nodes from Json object. The result is a list of `TodoItemNode` with nested
    //     file node as children.
    const data = this.buildFileTree(TREE_DATA, 0);

    // Notify the change.
    this.dataChange.next(data);
  }

  /**
   * Build the file structure tree. The `value` is the Json object, or a sub-tree of a Json object.
   * The return value is the list of `TodoItemNode`.
   */
  buildFileTree(obj: {[key: string]: any}, level: number): TodoItemNode[] {
    return Object.keys(obj).reduce<TodoItemNode[]>((accumulator, key) => {
      const value = obj[key];
      const node = new TodoItemNode();
      node.item = key;
      node.type = level === 0 ? 'Compound' : '';
      if (value != null) {
        if (typeof value === 'object') {
          node.children = this.buildFileTree(value, level + 1);
        } else {
          node.children = [];
        }
      }

      return accumulator.concat(node);
    }, []);
  }

  /** Add an item to to-do list */
  insertItem(parent: TodoItemNode, obj: any) {
    if (parent.children) {
      parent.children.push(obj);
    }
  }

  updateItem() {
    this.dataChange.next(this.data);
  }

  deleteChildren(parent: TodoItemNode) {
    if (parent.children) {
      parent.children = [];
    }
  }

  buildCurrentState(parentNode: TodoItemNode): string {
    let ret = '';
    let leftItem = '';
    let rightItem = '';
    const actualOperatorLabel = parentNode.operatorLabel;
    if (actualOperatorLabel && parentNode.children && parentNode.children.length > 0) {
      leftItem = ret.concat(this.buildCurrentState(parentNode.children[0]));
      rightItem = ret.concat(this.buildCurrentState(parentNode.children[1]));
      ret = ret.concat('[( ' + leftItem).concat(' ) ' + actualOperatorLabel + ' ( ').concat(rightItem + ' )]');
    } else {
      if ( parentNode.item !== 'Root') {
        ret = ret.concat(parentNode.item);
      }
    }
    return ret;
  }

  buildModel(node: TodoItemNode, index: number): ComponentModel {
    if (node.children.length === 0) {
      if (node.type === 'Sql' ||  node.type === 'String') {
        const component = new StringItem();
        component.value = node.value;
        component.type = node.type;
        component.position = index;
        return component;
      } else if (node.type === 'Int') {
        const component = new IntItem();
        component.position = index;
        component.value = node.value;
        component.position = index;
        return component;
      } else if (node.type === 'Date') {
        const component = new DateItem();
        component.dateValue = node.value;
        component.position = index;
        return component;
      } else {
        const component = new BooleanItem();
        component.booleanValue = node.value;
        component.position = index;
        return component;
      }
    } else {
      const parentComponent = new ConditionModel();
      parentComponent.conditionType = node.operator;
      parentComponent.components = node.children.map((item, pos) => this.buildModel(item, pos));
      return parentComponent;
    }
  }
}


@Component({
  selector: 'app-condition-edit',
  templateUrl: 'condition-edit.component.html',
  styleUrls: ['condition-edit.component.css'],
  providers: [ChecklistDatabase],
})
export class ConditionEditComponent implements AfterViewInit{
  /** Map from flat node to nested node. This helps us finding the nested node to be modified */
  flatNodeMap = new Map<TodoItemFlatNode, TodoItemNode>();

  /** Map from nested node to flattened node. This helps us to keep the same object for selection */
  nestedNodeMap = new Map<TodoItemNode, TodoItemFlatNode>();

  /** A selected parent node to be inserted */
  selectedParent: TodoItemFlatNode | null = null;

  /** The new item's name */
  newItemName = '';

  treeControl: FlatTreeControl<TodoItemFlatNode>;

  treeFlattener: MatTreeFlattener<TodoItemNode, TodoItemFlatNode>;

  dataSource: MatTreeFlatDataSource<TodoItemNode, TodoItemFlatNode>;

  /** CUSTOM PROPS */
  currentExpression = '';

  constructor(private _database: ChecklistDatabase) {
    this.treeFlattener = new MatTreeFlattener(this.transformer, this.getLevel, this.isExpandable, this.getChildren);
    this.treeControl = new FlatTreeControl<TodoItemFlatNode>(this.getLevel, this.isExpandable);
    this.dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

    _database.dataChange.subscribe(data => {
      this.dataSource.data = data;
      this.currentExpression = this._database.buildCurrentState(this.findRootNode());
    });

  }

  ngAfterViewInit() {
    document.getElementById('node-Root').click();
  }

  /* TREE COMPONENT */
  getLevel = (node: TodoItemFlatNode) => node.level;

  isExpandable = (node: TodoItemFlatNode) => node.expandable;

  getChildren = (node: TodoItemNode): TodoItemNode[] => node.children;

  hasChild = (node: TodoItemFlatNode) => this.flatNodeMap.get(node).children.length > 0;

  /**
   * Transformer to convert nested node to flat node. Record the nodes in maps for later use.
   */
  transformer = (node: TodoItemNode, level: number) => {
    const existingNode = this.nestedNodeMap.get(node);
    const flatNode = existingNode && existingNode.item === node.item
        ? existingNode
        : new TodoItemFlatNode();
    flatNode.item = node.item;
    flatNode.level = level;
    flatNode.expandable = !!node.children;
    this.flatNodeMap.set(flatNode, node);
    this.nestedNodeMap.set(node, flatNode);
    return flatNode;
  }

  /* Get the parent node of a node */
  getParentNode(node: TodoItemFlatNode): TodoItemFlatNode | null {
    const currentLevel = this.getLevel(node);

    if (currentLevel < 1) {
      return null;
    }

    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;

    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];

      if (this.getLevel(currentNode) < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }

  /* CUSTOM */
  findRootNode = () => this._database.data.find(x => x.item === 'Root');

  showNodeItem(node: TodoItemNode) {
    console.log(node);
  }

  showData() {
    console.log(this._database.data);
    console.log('PUTO EL QUE LEE', this._database.buildModel(this._database.data[0], 0));
  }

  getNodeSelectedConditionType(node: TodoItemFlatNode) {
    if (!!node) {
      const currentNode = this.flatNodeMap.get(node);
      return currentNode.type;
    } else { return ''; }
  }

  updateNode(event: any) {
    console.log('updateNode() event', event);
    if (event.nodeType === 'Compound') {
      this.addComponents(event.node);
    } else {
      this.deleteComponents(event.node);
    }
    const updatedNode = this.updateNodeProps(event);
    const parentNodeFlat = this.getParentNode(event.node);
    const parentNode = this.flatNodeMap.get(parentNodeFlat);
    const childrenIndex = parentNode.children.indexOf(updatedNode);

    parentNode.children.splice(childrenIndex, 1, event.node);
    parentNode.children.splice(childrenIndex, 1, updatedNode);
    this._database.updateItem();
  }

  addComponents(node: TodoItemFlatNode) {
    const parentNode = this.flatNodeMap.get(node);
    const childrens = this.getChildren(parentNode);
    if (childrens && childrens.length === 0) {
      const keyLeft = node.level + '_LeftCondition';
      const keyRight = node.level + '_RightCondition';
      this._database.insertItem(parentNode!, { item: keyLeft, children: [] });
      this._database.insertItem(parentNode!, { item: keyRight, children: [] });
      this.treeControl.expand(node);
    }
  }

  deleteComponents(node: TodoItemFlatNode) {
    const parentNode = this.flatNodeMap.get(node);
    const childrens = this.getChildren(parentNode);
    if (childrens) {
      this._database.deleteChildren(parentNode!);
      this.treeControl.collapse(node);
    }
  }

  updateNodeProps(event: any): TodoItemNode {
    const newNode = this.flatNodeMap.get(event.node);
    if (!!event.nodeType) {
      newNode.type = event.nodeType;
    }
    if (!!event.value) {
      newNode.value = event.value;
    }
    return newNode;
  }

  updateOperatorSelection(operatorItem: any, node: TodoItemFlatNode) {
    const newNode = this.flatNodeMap.get(node);
    newNode.operatorLabel = operatorItem.label;
    newNode.operator = operatorItem.value;
    this._database.updateItem();
    this.currentExpression = this._database.buildCurrentState(this.findRootNode());
  }

  updateInputNode(event: any) {
    console.log('updateNode() event', event);
    this.updateNodeProps(event);
    this._database.updateItem();
  }
}
