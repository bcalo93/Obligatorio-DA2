using System;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public class DateDataType : DataType
    {
        public DateTime DateValue { get; set; }
        protected override bool IsEqualsImplementation(DataType dataType)
        {
            return DateTime.Compare(this.DateValue, ((DateDataType)dataType).DateValue) == 0;
        }

        protected override bool IsMayorImplementation(DataType dataType)
        {
            return DateTime.Compare(this.DateValue, ((DateDataType)dataType).DateValue) > 0;
        }

        protected override bool IsMinorImplementation(DataType dataType)
        {
            return DateTime.Compare(this.DateValue, ((DateDataType)dataType).DateValue) < 0;
        }

        public override string ToString()
        {
            return this.DateValue.ToString();
        }
    }
}