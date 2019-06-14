import { Component, OnInit, Input, Output } from '@angular/core';
import { toArray, Operators } from 'src/enums';
import { EventEmitter } from '@angular/core';
import { TodoItemFlatNode } from '../condition-edit/condition-edit.component';

@Component({
  selector: 'app-condition-dropdown',
  templateUrl: './condition-dropdown.component.html',
  styleUrls: ['./condition-dropdown.component.css']
})
export class ConditionDropdownComponent implements OnInit {

  operators: any[];
  selectedOperator: string;
  selectedConditionType: string;
  inputType: string;

  @Output() addChildren: EventEmitter<any> = new EventEmitter();
  @Output() deleteChildren: EventEmitter<any> = new EventEmitter();

  @Input() state: boolean;
  @Input() node: TodoItemFlatNode;
  @Input() nodeItemName: string;

  constructor() {
    this.operators = toArray(Operators);
   }

  ngOnInit() {
  }

  isRoot = () => this.nodeItemName === 'Root';

  isCompound = () => {
    const isRootNode = this.nodeItemName === 'Root';

    if (isRootNode) {
      this.selectedConditionType = 'Compound';
      return true;
    } else if (!isRootNode && this.selectedConditionType === 'Compound') {
      this.addChildren.emit(this.node);
      return true;
    } else {
      if (!!this.selectedConditionType && this.selectedConditionType !== '') {
        this.deleteChildren.emit(this.node);
      }
      return false;
    }
  }

  alertMe(event:any) { 
    console.log(event)
  }
}
