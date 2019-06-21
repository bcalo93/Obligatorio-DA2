using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.WebApi.Models
{
    public class LoginModelOut
    {
        public UserGetModel User { get; set; }
        public Guid Token { get; set; }
        
        public LoginModelOut(AuthenticationToken authenticationToken)
        {
            User = new UserGetModel(authenticationToken.User);
            Token = authenticationToken.Token;
        }
    }
}