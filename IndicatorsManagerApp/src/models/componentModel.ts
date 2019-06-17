export abstract class ComponentModel {
    id?: string;
    position: number;

    abstract isValid(): boolean;
}
