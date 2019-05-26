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
    public class VisitorComponentEvaluate : IVisitorComponent<EvaluateConditionResult>
    {
        private IQueryRunner queryRunner;
        
        public VisitorComponentEvaluate(IQueryRunner queryRunner)
        {
            this.queryRunner = queryRunner;
        }

        public EvaluateConditionResult VisitItemNumeric(ItemNumeric numeric)
        {
            return new EvaluateConditionResult { ConditionToString = numeric.NumberValue.ToString(), ConditionResult = numeric.NumberValue };
        }

        public EvaluateConditionResult VisitItemQuery(ItemQuery query)
        {
            try {
                var result = queryRunner.RunQuery(query.QueryTextValue);
                return new EvaluateConditionResult { ConditionToString = result.ToString(), ConditionResult = result };
            }
            catch(DataAccessException de)
            {
                throw new EvaluationException(string.Format("La consulta {0} es incorrecta.", query.QueryTextValue), de);
            }
        }

        public EvaluateConditionResult VisitItemText(ItemText text)
        {
            return new EvaluateConditionResult { ConditionToString = text.TextValue, 
                ConditionResult = text.TextValue };
        }

        public EvaluateConditionResult VisitAndCondition(AndCondition andCondition)
        {
            IEnumerable<EvaluateConditionResult> results = andCondition.Components.OrderBy(c => c.Position)
                .Select(c => c.Accept(this));
            try
            {
                return new EvaluateConditionResult { ConditionResult = results.All(r => (bool)r.ConditionResult),
                    ConditionToString = string.Join(" And ", results.Select(r => r.ConditionToString)) };
            }
            catch(InvalidCastException ic)
            {
                throw new EvaluationException("Uno de los elementos del And no devuelve booleano", ic);
            }
        }

        public EvaluateConditionResult VisitOrCondition(OrCondition orCondition)
        {
            IEnumerable<EvaluateConditionResult> results = orCondition.Components.OrderBy(c => c.Position)
                .Select(c => c.Accept(this));
            try
            {
                return new EvaluateConditionResult { ConditionResult = results.Any(r => (bool)r.ConditionResult),
                    ConditionToString = string.Join(" Or ", results.Select(r => r.ConditionToString)) };
            }
            catch(InvalidCastException ic)
            {
                throw new EvaluationException("Uno de los elementos del Or no devuelve booleano", ic);
            }
        }

        public EvaluateConditionResult VisitEqualsCondition(EqualsCondition equalsCondition)
        {
            bool result = false;
            IEnumerable<Component> ordered = equalsCondition.Components.OrderBy(c => c.Position);
            EvaluateConditionResult leftResult = ordered.ElementAt(0).Accept(this);
            EvaluateConditionResult rightResult = ordered.ElementAt(1).Accept(this);
            
            if(IsNumber(leftResult.ConditionResult) && IsNumber(rightResult.ConditionResult))
            {
                result = AsDecimal(leftResult.ConditionResult) == AsDecimal(rightResult.ConditionResult);
            } 
            else 
            {
                result = leftResult.ConditionResult.ToString().Equals(rightResult.ConditionResult.ToString());
            }
            return new EvaluateConditionResult{ ConditionResult = result, ConditionToString = string.Format("({0} = {1})", 
                leftResult.ConditionToString, rightResult.ConditionToString ) };
        }

        public EvaluateConditionResult VisitMayorCondition(MayorCondition mayorCondition)
        {
            bool result = false;
            IEnumerable<Component> ordered = mayorCondition.Components.OrderBy(c => c.Position);
            EvaluateConditionResult leftResult = ordered.ElementAt(0).Accept(this);
            EvaluateConditionResult rightResult = ordered.ElementAt(1).Accept(this);
            
            if(IsNumber(leftResult.ConditionResult) && IsNumber(rightResult.ConditionResult))
            {
                result = AsDecimal(leftResult.ConditionResult) > AsDecimal(rightResult.ConditionResult);
            } 
            else 
            {
                result = leftResult.ConditionResult.ToString().CompareTo(rightResult.ConditionResult.ToString()) > 0;
            }
            return new EvaluateConditionResult{ ConditionResult = result, ConditionToString = string.Format("({0} > {1})", 
                leftResult.ConditionToString, rightResult.ConditionToString ) };
        }

        public EvaluateConditionResult VisitMayorEqualsCondition(MayorEqualsCondition mayorEqualsCondition)
        {
            bool result = false;
            IEnumerable<Component> ordered = mayorEqualsCondition.Components.OrderBy(c => c.Position);
            EvaluateConditionResult leftResult = ordered.ElementAt(0).Accept(this);
            EvaluateConditionResult rightResult = ordered.ElementAt(1).Accept(this);
            
            if(IsNumber(leftResult.ConditionResult) && IsNumber(rightResult.ConditionResult))
            {
                result = AsDecimal(leftResult.ConditionResult) >= AsDecimal(rightResult.ConditionResult);
            } 
            else 
            {
                result = leftResult.ConditionResult.ToString().CompareTo(rightResult.ConditionResult.ToString()) >= 0;
            }
            return new EvaluateConditionResult{ ConditionResult = result, ConditionToString = string.Format("({0} >= {1})", 
                leftResult.ConditionToString, rightResult.ConditionToString ) };
        }

        public EvaluateConditionResult VisitMinorCondition(MinorCondition minorCondition)
        {
            bool result = false;
            IEnumerable<Component> ordered = minorCondition.Components.OrderBy(c => c.Position);
            EvaluateConditionResult leftResult = ordered.ElementAt(0).Accept(this);
            EvaluateConditionResult rightResult = ordered.ElementAt(1).Accept(this);
            
            if(IsNumber(leftResult.ConditionResult) && IsNumber(rightResult.ConditionResult))
            {
                result = AsDecimal(leftResult.ConditionResult) < AsDecimal(rightResult.ConditionResult);
            } 
            else 
            {
                result = leftResult.ConditionResult.ToString().CompareTo(rightResult.ConditionResult.ToString()) < 0;
            }
            return new EvaluateConditionResult{ ConditionResult = result, ConditionToString = string.Format("({0} < {1})", 
                leftResult.ConditionToString, rightResult.ConditionToString ) };
        }

        public EvaluateConditionResult VisitMinorEqualsCondition(MinorEqualsCondition minorEqualsCondition)
        {
            bool result = false;
            BinaryCondition condition = ConvertToBinaryCondition(minorEqualsCondition);
            EvaluateConditionResult leftResult = condition.LeftCondition.Accept(this);
            EvaluateConditionResult rightResult = condition.RightCondition.Accept(this);
            
            if(IsNumber(leftResult.ConditionResult) && IsNumber(rightResult.ConditionResult))
            {
                result = AsDecimal(leftResult.ConditionResult) <= AsDecimal(rightResult.ConditionResult);
            } 
            else if(leftResult.ConditionResult.GetType() == typeof(DateTime)) 
            {
                result = (DateTime)leftResult.ConditionResult <= (DateTime)rightResult.ConditionResult;
            }
            else 
            {
                result = leftResult.ConditionResult.ToString().CompareTo(rightResult.ConditionResult.ToString()) <= 0;
            }
            return new EvaluateConditionResult{ ConditionResult = result, ConditionToString = string.Format("({0} <= {1})", 
                leftResult.ConditionToString, rightResult.ConditionToString ) };
        }
        
        public EvaluateConditionResult VisitItemBoolean(ItemBoolean boolean)
        {
            return new EvaluateConditionResult { ConditionToString = boolean.Boolean ? "True" : "False" , ConditionResult = boolean.Boolean };
        }

        public EvaluateConditionResult VisitItemDate(ItemDate date)
        {
            return new EvaluateConditionResult { ConditionToString = date.Date.ToString(), ConditionResult = date.Date };
        }

        private bool IsNumber(object obj)
        {
            return obj.GetType() == typeof(int) || obj.GetType() == typeof(double) || obj.GetType() == typeof(decimal) || obj.GetType() == typeof(bool);
        }

        private decimal AsDecimal(object obj)
        {
            if(obj.GetType() == typeof(int))
            {
                return (int)obj;
            }
            else if(obj.GetType() == typeof(double))
            {
                return (decimal)(double)obj;
            }
            else if(obj.GetType() == typeof(decimal)) 
            {
                return (decimal)obj;
            }
            else if(obj.GetType() == typeof(bool))
            {
                return (bool)obj ? 1 : 0;
            }
            else
            {
                throw new EvaluationException("No se puede convertir a decimal");
            }
        }

        
        private BinaryCondition ConvertToBinaryCondition(Condition condition)
        {
            IEnumerable<Component> ordered = condition.Components.OrderBy(c => c.Position);
            return new BinaryCondition { LeftCondition = ordered.ElementAt(0), RightCondition = ordered.ElementAt(1) };
        }

        class BinaryCondition 
        {
            public Component LeftCondition { get; set; }
            public Component RightCondition { get; set; }

            public BinaryCondition() { }
        }
    }
}