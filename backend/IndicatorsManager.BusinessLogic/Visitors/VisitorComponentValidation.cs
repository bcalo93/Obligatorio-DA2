using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class VisitorComponentValidation : IVisitorComponent<bool>
    {

        public bool VisitItemNumeric(ItemNumeric numeric)
        {
            return true;
        }

        public bool VisitItemQuery(ItemQuery query)
        {
            return !String.IsNullOrEmpty(query.QueryTextValue) && query.QueryTextValue.Trim() != "";
        }

        public bool VisitItemText(ItemText text)
        {
            return text.TextValue != null;
        }

        public bool VisitAndCondition(AndCondition andCondition)
        {
            return ValidateCondition(andCondition); 
        }
        
        public bool VisitOrCondition(OrCondition orCondition)
        {
            return ValidateCondition(orCondition);
        }

        public bool VisitEqualsCondition(EqualsCondition equalsCondition)
        {
            return equalsCondition.Components != null && equalsCondition.Components.Count == 2 && ValidateCondition(equalsCondition);
        }

        public bool VisitMayorCondition(MayorCondition mayorCondition)
        {
            return ValidateNumericCondition(mayorCondition);
        }

        public bool VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition)
        {
            return ValidateNumericCondition(mayorEqualsCondition);
        }

        public bool VisitMinorCondition(MinorCondition minorCondition)
        {
            return ValidateNumericCondition(minorCondition);
        }

        public bool VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition)
        {
            return ValidateNumericCondition(minorEqualsCondition);
        }

        public bool VisitItemBoolean(ItemBoolean boolean)
        {
            return true;
        }

        public bool VisitItemDate(ItemDate date)
        {
            return true;
        }

        private bool ValidateCondition(Condition condition)
        {
            return condition.Components != null && condition.Components.Count > 1 && 
            condition.Components.GroupBy(c => c.Position).All(c => c.Count() == 1) && condition.Components.All(c => c.Accept(this));
        }
        private bool ValidateNumericCondition(Condition condition)
        { 
            return condition.Components != null && condition.Components.Count == 2 && 
                !condition.Components.Any(c => c.GetType() == typeof(ItemBoolean)) && ValidateCondition(condition);
        }
    }
}