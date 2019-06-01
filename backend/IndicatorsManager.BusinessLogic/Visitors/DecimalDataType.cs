using System;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class DecimalDataType : DataType
    {
        public decimal DecimalValue { get; set; }
        protected override bool IsEqualsImplementation(DataType dataType)
        {
            return this.DecimalValue == ((DecimalDataType)dataType).DecimalValue;
        }

        protected override bool IsMayorImplementation(DataType dataType)
        {
            return this.DecimalValue > ((DecimalDataType)dataType).DecimalValue;
        }

        protected override bool IsMinorImplementation(DataType dataType)
        {
            return this.DecimalValue < ((DecimalDataType)dataType).DecimalValue;
        }

        public override object GetDataValue()
        {
            return this.DecimalValue;
        }

        public override string ToString()
        {
            return this.DecimalValue.ToString();
        } 
    }
}