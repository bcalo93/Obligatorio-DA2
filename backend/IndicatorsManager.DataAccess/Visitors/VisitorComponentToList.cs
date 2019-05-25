using System;
using System.Collections.Generic;
using IndicatorsManager.Domain;
using IndicatorsManager.Domain.Visitors;

namespace IndicatorsManager.DataAccess.Visitors
{
    public class VisitorComponentToList : IVisitorComponent<List<Component>>
    {
        public List<Component> VisitItemNumeric(ItemNumeric numeric)
        {
            return new List<Component> { numeric };
        }

        public List<Component> VisitItemQuery(ItemQuery query)
        {
            return new List<Component> { query };
        }

        public List<Component> VisitItemText(ItemText text)
        {
            return new List<Component> { text };
        }

        public List<Component> VisitAndCondition(AndCondition andCondition)
        {
            return VisitCondition(andCondition);
        }

        public List<Component> VisitEqualsCondition(EqualsCondition equalsCondition)
        {
            return VisitCondition(equalsCondition);
        }

        public List<Component> VisitMayorCondition(MayorCondition mayorCondition)
        {
            return VisitCondition(mayorCondition);
        }

        public List<Component> VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition)
        {
            return VisitCondition(mayorEqualsCondition);
        }

        public List<Component> VisitMinorCondition(MinorCondition minorCondition)
        {
            return VisitCondition(minorCondition);
        }

        public List<Component> VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition)
        {
            return VisitCondition(minorEqualsCondition);
        }

        public List<Component> VisitOrCondition(OrCondition orCondition)
        {
            return VisitCondition(orCondition);
        }

        private List<Component> VisitCondition(Condition condition)
        {
            List<Component> result = new List<Component>{ condition };
            foreach (Component component in condition.Components)
            {
                result.AddRange(component.Accept(this));
            }
            return result;
        }
    }

}