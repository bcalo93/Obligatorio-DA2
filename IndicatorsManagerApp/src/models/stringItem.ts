import { ComponentModel } from './componentModel';
import { StringType } from 'src/enums';

export class StringItem extends ComponentModel {
    value: string;
    type: StringType;
}

