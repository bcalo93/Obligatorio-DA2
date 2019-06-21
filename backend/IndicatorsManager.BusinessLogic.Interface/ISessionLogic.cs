using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.BusinessLogic.Interface
{
    public interface ISessionLogic
    {
 
        bool IsValidToken(Guid token);

        AuthenticationToken CreateToken(string username, string password);

        bool HasLevel(Guid token, Role role);

        User GetUser(Guid token);

    }
}