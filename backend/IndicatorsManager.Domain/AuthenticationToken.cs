using System;

namespace IndicatorsManager.Domain
{
    public class AuthenticationToken
    {
        public Guid Id { get; set; }

        public Guid Token { get; set; }
        
        public virtual User User { get; set; }

        public AuthenticationToken(){}

        public AuthenticationToken Update(AuthenticationToken token)
        {
            Token = token.Token;
            User = token.User;
            return this;
        }
        
    }    
}