using System;
using IndicatorsManager.Domain;

namespace IndicatorsManager.DataAccess.Interface
{
    public interface ITokenRepository
    {
        void Add(AuthenticationToken entity);

        void Update(AuthenticationToken entity);

        AuthenticationToken GetByToken(Guid token);

        AuthenticationToken GetByUser(User user);

        void Save();

    }
}