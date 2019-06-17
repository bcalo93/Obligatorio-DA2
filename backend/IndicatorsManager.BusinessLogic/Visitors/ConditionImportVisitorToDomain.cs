using IndicatorsManager.Domain;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;
using IndicatorsManager.IndicatorImporter.Interface;
using IndicatorsManager.IndicatorImporter.Interface.Visitor;
using System.Linq;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class ConditionImportVisitorToDomain : IConditionImportVisitor<Component>
    {
        public Component VisitConditionImport(ConditionImport condition)
        {
            Condition result;
            switch (condition.ConditionType)
            {
                case ConditionType.And:
                    result = new AndCondition();
                    break;
                case ConditionType.Or:
                    result = new OrCondition();
                    break;
                case ConditionType.Equals:
                    result = new EqualsCondition();
                    break;
                case ConditionType.Greater:
                    result = new MayorCondition();
                    break;
                case ConditionType.GreaterEquals:
                   result = new MayorEqualsCondition();
                   break;
                case ConditionType.Minor:
                    result = new MinorCondition();
                    break;
                case ConditionType.MinorEquals:
                    result = new MinorEqualsCondition();
                    break;
                default:
                    throw new ImportException("Could not resolve ConditionType value");
            }
            result.Position = condition.Position;
            result.Components = condition.Components.Select(c => c.Accept(this)).ToList();
            return result;
        }

        public Component VisitItemBooleanImport(ItemBooleanImport boolean)
        {
            return new ItemBoolean { Position = boolean.Position, Boolean = boolean.Boolean };
        }

        public Component VisitItemDateImport(ItemDateImport date)
        {
            return new ItemDate { Position = date.Position, Date = date.Date };
        }

        public Component VisitItemNumberImport(ItemNumberImport number)
        {
            return new ItemNumeric { Position = number.Position, NumberValue = number.Number };
        }

        public Component VisitItemQueryImport(ItemQueryImport query)
        {
            return new ItemQuery { Position = query.Position, QueryTextValue = query.Query };
        }

        public Component VisitItemTextImport(ItemTextImport text)
        {
            return new ItemText { Position = text.Position, TextValue = text.Text };
        }
    }
}