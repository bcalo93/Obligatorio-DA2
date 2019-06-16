import { Result } from '.';
import { ComponentModel } from './componentModel';

export class IndicatorItem {
    id?: string;
    name: string;
    condition: ComponentModel;
    result?: Result;
}
