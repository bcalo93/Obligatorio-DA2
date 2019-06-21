import { ComponentModel } from './componentModel';
import { Operators } from 'src/enums';

export class ConditionModel extends ComponentModel {
    components: ComponentModel[];
    conditionType: string;

    constructor()  {
        super();
    }

    isValid() {
        return !this.components.some( e => e === null);
    }
}
