import { ComponentModel } from './componentModel';

export class IntItem extends ComponentModel {
    value: number;

    isValid(): boolean {
        return !(!!this.value);
    }
}
