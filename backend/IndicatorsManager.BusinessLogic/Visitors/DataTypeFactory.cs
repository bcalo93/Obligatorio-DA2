using System;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    internal static class DataTypeFactory
    {
        public static DataType ObjectToDataType(object obj)
        {
            if(IsNumber(obj))
            {
                return AsDecimal(obj);
            }
            else if(obj.GetType() == typeof(bool))
            {
                return new BooleanDataType { BooleanValue = (bool)obj };
            }
            else if(obj.GetType() == typeof(DateTime))
            {
                return new DateDataType { DateValue = (DateTime)obj };
            }
            else
            {
                return new StringDataType { StringValue = obj.ToString() };
            }
        }

        private static bool IsNumber(object obj)
        {
            return obj.GetType() == typeof(int) || obj.GetType() == typeof(double) || obj.GetType() == typeof(decimal);
        }
        
        private static DecimalDataType AsDecimal(object obj)
        {
            if(obj.GetType() == typeof(int))
            {
                return new DecimalDataType{ DecimalValue = (int)obj };
            }
            else if(obj.GetType() == typeof(double))
            {
                return new DecimalDataType{ DecimalValue = (decimal)(double)obj };
            }
            else if(obj.GetType() == typeof(decimal)) 
            {
                return new DecimalDataType{ DecimalValue = (decimal)obj };
            }
            else
            {
                throw new EvaluationException("Cannot convert to decimal.");
            }
        }
    }
    
}