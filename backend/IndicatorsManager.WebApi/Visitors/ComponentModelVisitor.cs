using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using IndicatorsManager.Domain.Visitors;
using IndicatorsManager.WebApi.Models;

namespace IndicatorsManager.WebApi.Visitors
{
    public class ComponentModelVisitor : IVisitorComponent<ComponentModel>
    {
        public ComponentModel VisitItemNumeric(ItemNumeric numeric)
        {
            return new IntItemModel(numeric);
        }

        public ComponentModel VisitItemQuery(ItemQuery query)
        {
            return new StringItemModel(query);
        }

        public ComponentModel VisitItemText(ItemText text)
        {
            return new StringItemModel(text);
        }

        public ComponentModel VisitAndCondition(AndCondition andCondition)
        {
            ConditionModel model = new ConditionModel("And", andCondition.Position);
            model.Components = ConditionToModel(andCondition);
            return model;
        }

        public ComponentModel VisitOrCondition(OrCondition orCondition)
        {
            ConditionModel model = new ConditionModel("Or", orCondition.Position);
            model.Components = ConditionToModel(orCondition);
            return model;
        }

        public ComponentModel VisitEqualsCondition(EqualsCondition equalsCondition)
        {
            ConditionModel model = new ConditionModel("Equals", equalsCondition.Position);
            model.Components = ConditionToModel(equalsCondition);
            return model;
        }

        public ComponentModel VisitMayorCondition(MayorCondition mayorCondition)
        {
            ConditionModel model = new ConditionModel("Mayor", mayorCondition.Position);
            model.Components = ConditionToModel(mayorCondition);
            return model;
        }

        public ComponentModel VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition)
        {
            ConditionModel model = new ConditionModel("MayorEquals", mayorEqualsCondition.Position);
            model.Components = ConditionToModel(mayorEqualsCondition);
            return model;
        }

        public ComponentModel VisitMinorCondition(MinorCondition minorCondition)
        {
            ConditionModel model = new ConditionModel("Minor", minorCondition.Position);
            model.Components = ConditionToModel(minorCondition);
            return model;
        }

        public ComponentModel VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition)
        {
            ConditionModel model = new ConditionModel("MinorEquals", minorEqualsCondition.Position);
            model.Components = ConditionToModel(minorEqualsCondition);
            return model;
        }

        public IEnumerable<ComponentModel> ConditionToModel(Condition model)
        {
            return model.Components.Select(c => c.Accept(this));
        }

        public ComponentModel VisitItemBoolean(ItemBoolean boolean)
        {
            throw new NotImplementedException();
        }

        public ComponentModel VisitItemDate(ItemDate date)
        {
            throw new NotImplementedException();
        }
    }
}