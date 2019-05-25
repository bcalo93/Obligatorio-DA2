using System;

namespace IndicatorsManager.Domain.Visitors
{
    public interface IVisitorComponent<T>
    {
        
        T VisitAndCondition(AndCondition andCondition);
        T VisitOrCondition(OrCondition orCondition);
        T VisitEqualsCondition(EqualsCondition equalsCondition);
        T VisitMayorCondition(MayorCondition mayorCondition);
        T VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition);
        T VisitMinorCondition(MinorCondition minorCondition);
        T VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition);
        T VisitItemQuery(ItemQuery query);
        T VisitItemText(ItemText text);
        T VisitItemNumeric(ItemNumeric numeric);
        
    }
    
}