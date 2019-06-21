import { ComponentModel } from './componentModel';

export class BooleanItem extends ComponentModel {
    booleanValue: string;    
    isValid(): boolean {
        return !(!!this.booleanValue);
    }
}

