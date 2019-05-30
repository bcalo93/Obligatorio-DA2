using System;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class StringDataType : DataType
    {
        public string StringValue { get; set; }
        protected override bool IsEqualsImplementation(DataType dataType)
        {
            return this.StringValue == dataType.ToString();
        }

        protected override bool IsMayorImplementation(DataType dataType)
        {
            return this.StringValue.CompareTo(dataType.ToString()) > 0;
        }

        protected override bool IsMinorImplementation(DataType dataType)
        {
            return this.StringValue.CompareTo(dataType.ToString()) < 0;
        }

        public override string ToString()
        {
            return this.StringValue;
        }
    }
}