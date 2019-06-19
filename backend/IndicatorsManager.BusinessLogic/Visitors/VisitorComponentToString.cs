using System;
using System.Linq;
using System.Collections.Generic;
using IndicatorsManager.Domain;
using IndicatorsManager.Domain.Visitors;
using IndicatorsManager.BusinessLogic.Interface;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class VisitorComponentToString : IVisitorComponent<string>
    {
        private  IQueryRunner queryRunner;
        public VisitorComponentToString(IQueryRunner queryRunner)
        {
            this.queryRunner = queryRunner;
        }

        public string VisitItemBoolean(ItemBoolean boolean)
        {
            return boolean.Boolean ? "True" : "False";
        }

        public string VisitItemDate(ItemDate date)
        {
            return date.Date.ToString("dd/MM/yyyy");
        }

        public string VisitItemNumeric(ItemNumeric numeric)
        {
            return numeric.NumberValue.ToString();
        }

        public string VisitItemQuery(ItemQuery query)
        {
            try 
            {
                var result = queryRunner.RunQuery(query.QueryTextValue);
                return result.ToString();
            }
            catch(DataAccessException)
            {
                return string.Format("Incorrect Query - {0}", query.QueryTextValue);
            }
        }

        public string VisitItemText(ItemText text)
        {
            return text.TextValue;
        }

        public string VisitAndCondition(AndCondition andCondition)
        {
            return string.Join(" And ", andCondition.Components.OrderBy(c => c.Position).Select(c => c.Accept(this)));
        }

        public string VisitEqualsCondition(EqualsCondition equalsCondition)
        {
            return BinaryConditionToString(equalsCondition, " = ");
        }

        public string VisitMayorCondition(MayorCondition mayorCondition)
        {
            return BinaryConditionToString(mayorCondition, " > ");
        }

        public string VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition)
        {
            return BinaryConditionToString(mayorEqualsCondition, " >= ");
        }

        public string VisitMinorCondition(MinorCondition minorCondition)
        {
            return BinaryConditionToString(minorCondition, " < ");
        }

        public string VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition)
        {
            return BinaryConditionToString(minorEqualsCondition, " <= ");
        }

        public string VisitOrCondition(OrCondition orCondition)
        {
            return string.Join(" Or ", orCondition.Components.OrderBy(c => c.Position).Select(c => c.Accept(this)));
        }
        
        private string BinaryConditionToString(Condition condition, string connector)
        {
            BinaryCondition toBinary = new BinaryCondition(condition);
            return string.Format("({0}" + connector + "{1})", toBinary.LeftCondition.Accept(this), toBinary.RightCondition.Accept(this));
        }
    }
}