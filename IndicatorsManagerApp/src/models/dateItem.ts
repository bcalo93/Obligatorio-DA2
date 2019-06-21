import { ComponentModel } from './componentModel';

export class DateItem extends ComponentModel {

    dateValue: Date;

    isValid(): boolean {
        return !(!!this.dateValue);
    }
}