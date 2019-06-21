import { Component, OnInit, Input, Output } from '@angular/core';
import { toArray, Operators } from 'src/enums';
import { EventEmitter } from '@angular/core';
import { TodoItemFlatNode } from '../condition-edit/condition-edit.component';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-condition-dropdown',
  templateUrl: './condition-dropdown.component.html',
  styleUrls: ['./condition-dropdown.component.css']
})
export class ConditionDropdownComponent implements OnInit {

  operators: any[];
  inputType: string;
  selectedConditionType: string;
  selectedOperator: string;
  selectedBoolean: string;

  inputInt = new FormControl('', [Validators.required]);
  inputDate = new FormControl('', [Validators.required]);
  inputQuery = new FormControl('', [Validators.required]);
  inputString = new FormControl('', [Validators.required]);

  @Output() addChildren: EventEmitter<any> = new EventEmitter();
  @Output() deleteChildren: EventEmitter<any> = new EventEmitter();
  @Output() operatorItemChange: EventEmitter<any> = new EventEmitter();
  @Output() updateNode: EventEmitter<any> = new EventEmitter();

  @Output() inputValue: EventEmitter<any> = new EventEmitter();



  @Input() state: boolean;
  @Input() node: TodoItemFlatNode;
  @Input() nodeType: string;


  constructor() {
    this.operators = toArray(Operators);
  }

  ngOnInit() {
    this.selectedConditionType = this.nodeType;
    this.selectedConditionType = this.nodeType;

  }

  isRoot = () => this.node.item === 'Root';

  isCompound = () => {
    const isRootNode = this.node.item === 'Root';

    if (isRootNode) {
      this.selectedConditionType = 'Compound';
      return true;
    } else if (!isRootNode && this.selectedConditionType === 'Compound') {
      this.updateNode.emit({node: this.node, nodeType: this.selectedConditionType});
      return true;
    } else {
      if (!!this.selectedConditionType && this.selectedConditionType !== '') {
        this.updateNode.emit({node: this.node, nodeType: this.selectedConditionType});
      }
      return false;
    }
  }

  operatorChange(event: any) {
    const operatorItem = this.operators.find(x => x.value === event);
    this.operatorItemChange.emit(operatorItem);
  }

  onChangeInt(event: any) {
    const value = parseInt(event.target.value);
    if (isNaN(value)) {
      this.inputInt.setErrors({incorrect: true});
    } else { this.inputValue.emit( { node: this.node, value }); }
  }

  onChangeDate(event: any) {
    let value = event.target.value;
    if (value instanceof Date) {
      value = value.toISOString().split('T')[0];
      if (value !== '') {
        this.inputValue.emit( { node: this.node, value });
      }
    } else { this.inputDate.setErrors({incorrect: true}); }
  }

  onChangeString(event: any) {
    const value = event.target.value;
    if (value !== '') {
      this.inputValue.emit( { node: this.node, value });
    }
  }

  onChangeBoolean(event: any) {
    const isTrue = event === 'true';
    this.inputValue.emit( { node: this.node, value: isTrue });
  }
}
