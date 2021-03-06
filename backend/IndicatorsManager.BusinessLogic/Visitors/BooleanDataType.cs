using System;
using IndicatorsManager.BusinessLogic.Interface.Exceptions;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class BooleanDataType : DataType
    {
        public bool BooleanValue { get; set; }
        protected override bool IsEqualsImplementation(DataType dataType)
        {
            return this.BooleanValue == ((BooleanDataType)dataType).BooleanValue;
        }

        protected override bool IsMayorImplementation(DataType dataType)
        {
            throw new EvaluationException("Booleans cannot be compared using > o >=");
        }

        protected override bool IsMinorImplementation(DataType dataType)
        {
            throw new EvaluationException("Booleans cannot be compared using < o <=");
        }

        public override object GetDataValue()
        {
            return this.BooleanValue;
        }

        public override string ToString()
        {
            return this.BooleanValue ? "True" : "False";
        }
    }

}