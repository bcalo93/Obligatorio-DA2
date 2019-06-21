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
    public class VisitorComponentEvaluate : IVisitorComponent<DataType>
    {
        private IQueryRunner queryRunner;
        
        public VisitorComponentEvaluate(IQueryRunner queryRunner)
        {
            this.queryRunner = queryRunner;
        }

        public DataType VisitItemNumeric(ItemNumeric numeric)
        {
            return new DecimalDataType { DecimalValue = numeric.NumberValue };
        }

        public DataType VisitItemQuery(ItemQuery query)
        {
            try 
            {
                var result = queryRunner.RunQuery(query.QueryTextValue);
                return DataTypeFactory.ObjectToDataType(result);
            }
            catch(DataAccessException de)
            {
                throw new EvaluationException(string.Format("Query {0} is incorrect.", query.QueryTextValue), de);
            }
        }

        public DataType VisitItemText(ItemText text)
        {
            return new StringDataType { StringValue = text.TextValue };
        }

        public DataType VisitAndCondition(AndCondition andCondition)
        {
            IEnumerable<DataType> results = andCondition.Components.Select(c => c.Accept(this));
            try
            {
                return new BooleanDataType { BooleanValue = results.All(r => ((BooleanDataType)r).BooleanValue)};
            }
            catch(InvalidCastException ic)
            {
                throw new EvaluationException("One or many And element/s don't return a boolean value.", ic);
            }
        }

        public DataType VisitOrCondition(OrCondition orCondition)
        {
            IEnumerable<DataType> results = orCondition.Components.Select(c => c.Accept(this));
            try
            {
                return new BooleanDataType { BooleanValue = results.Any(r => ((BooleanDataType)r).BooleanValue) };
            }
            catch(InvalidCastException ic)
            {
                throw new EvaluationException("One or many Or element/s don't return a boolean value.", ic);
            }
        }

        public DataType VisitEqualsCondition(EqualsCondition equalsCondition)
        {
            BinaryCondition condition = new BinaryCondition(equalsCondition);
            DataType leftResult = condition.LeftCondition.Accept(this);
            DataType rightResult = condition.RightCondition.Accept(this);
            return new BooleanDataType{ BooleanValue = leftResult.IsEquals(rightResult) };
        }

        public DataType VisitMayorCondition(MayorCondition mayorCondition)
        {
            BinaryCondition condition = new BinaryCondition(mayorCondition);
            DataType leftResult = condition.LeftCondition.Accept(this);
            DataType rightResult = condition.RightCondition.Accept(this);
            return new BooleanDataType { BooleanValue = leftResult.IsMayor(rightResult) };
        }

        public DataType VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition)
        {
            BinaryCondition condition = new BinaryCondition(mayorEqualsCondition);
            DataType leftResult = condition.LeftCondition.Accept(this);
            DataType rightResult = condition.RightCondition.Accept(this);
            return new BooleanDataType{ BooleanValue = leftResult.IsMayorOrEquals(rightResult) };
        }

        public DataType VisitMinorCondition(MinorCondition minorCondition)
        {
            BinaryCondition condition = new BinaryCondition(minorCondition);
            DataType leftResult = condition.LeftCondition.Accept(this);
            DataType rightResult = condition.RightCondition.Accept(this);
            return new BooleanDataType { BooleanValue = leftResult.IsMinor(rightResult) };
        }

        public DataType VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition)
        {
            BinaryCondition condition = new BinaryCondition(minorEqualsCondition);
            DataType leftResult = condition.LeftCondition.Accept(this);
            DataType rightResult = condition.RightCondition.Accept(this);
            return new BooleanDataType { BooleanValue = leftResult.IsMinorOrEquals(rightResult) };
        }
        
        public DataType VisitItemBoolean(ItemBoolean boolean)
        {
            return new BooleanDataType { BooleanValue = boolean.Boolean };
        }

        public DataType VisitItemDate(ItemDate date)
        {
            return new DateDataType { DateValue = date.Date };
        }
        
    }
}