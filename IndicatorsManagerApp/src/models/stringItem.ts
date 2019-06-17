import { ComponentModel } from './componentModel';

export class StringItem extends ComponentModel {
    value: string;
    type: string;

    isValid(): boolean {
        return !(!!this.value);
    }
}

