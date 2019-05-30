using System;

namespace IndicatorsManager.BusinessLogic.Visitors
{
    public abstract class DataType
    {
        public bool IsEquals(DataType dataType)
        {
            try
            {
                return this.IsEqualsImplementation(dataType);
            }
            catch(InvalidCastException)
            {
                return this.ToString() == dataType.ToString();
            }
        }

        public bool IsMinor(DataType dataType)
        {
            try
            {
                return this.IsMinorImplementation(dataType);
            }
            catch(InvalidCastException)
            {
                return this.ToString().CompareTo(dataType.ToString()) < 0;
            }
        }

        public bool IsMayor(DataType dataType)
        {
            try
            {
                return this.IsMayorImplementation(dataType);
            }
            catch(InvalidCastException)
            {
                return this.ToString().CompareTo(dataType.ToString()) > 0;
            }
        }
        
        public bool IsMinorOrEquals(DataType dataType)
        {
            return this.IsMinor(dataType) || this.IsEquals(dataType);
        }

        public bool IsMayorOrEquals(DataType dataType)
        {
            return this.IsMayor(dataType) || this.IsEquals(dataType);
        }

        protected abstract bool IsEqualsImplementation(DataType dataType);
        protected abstract bool IsMinorImplementation(DataType dataType);
        protected abstract bool IsMayorImplementation(DataType dataType);

    }
}