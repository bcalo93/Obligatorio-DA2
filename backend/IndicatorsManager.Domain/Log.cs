using System;
using System.Collections.Generic;

namespace IndicatorsManager.Domain
{
    public class Log
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }
        
        public virtual User User { get; set; }

        public Log(){}   

        public Log Update(Log log)
        {
            DateTime = log.DateTime;
            User = log.User;
            return this;
        }     
        
    }    
}