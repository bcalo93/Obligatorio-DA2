export enum Operators {
    AND_OPERATOR = 'And',
    OR_OPERATOR = 'Or',
    EQUALS_OPERATOR = 'Equals',
    MINOR_OPERATOR = 'Minor',
    MINOR_EQUALS_OPERATOR = 'MinorEquals',
    MAYOR_OPERATOR =  'Mayor',
    MAYOR_EQUALS_OPERATOR = 'MayorEquals'
}

export function toArray(enumme) {
    return Object.keys(enumme)
        .map(key => {
            const value = enumme[key];
            let label: string = enumme[key];
            if (value === Operators.MINOR_EQUALS_OPERATOR) {
                label = 'Less Equals (<=)';
            }
            if (value === Operators.MINOR_OPERATOR) {
                label = 'Less (<)';
            }
            if (value === Operators.MAYOR_EQUALS_OPERATOR) {
                label = 'Greater Equals (>=)';
            }
            if (value === Operators.MAYOR_OPERATOR) {
                label = 'Greater (>)';
            }
            if (value === Operators.EQUALS_OPERATOR) {
                label = 'Equals (=)';
            }
            label = label.toUpperCase();
            return { key, value, label };
        });
}
