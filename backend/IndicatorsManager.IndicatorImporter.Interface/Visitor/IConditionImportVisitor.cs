namespace IndicatorsManager.IndicatorImporter.Interface.Visitor
{
    public interface IConditionImportVisitor<T>
    {
        T VisitConditionImport(ConditionImport condition);
        T VisitItemBooleanImport(ItemBooleanImport boolean);
        T VisitItemDateImport(ItemDateImport date);
        T VisitItemNumberImport(ItemNumberImport number);
        T VisitItemQueryImport(ItemQueryImport query);
        T VisitItemTextImport(ItemTextImport text);
    }
}