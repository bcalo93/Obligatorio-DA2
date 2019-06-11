using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using System.Data.SqlClient;

namespace IndicatorsManager.DataAccess
{
    public class AreaRepository : BaseRepository<Area>, IAreaQuery
    {
        public AreaRepository(DbContext context): base(context) { }

        public override Area Get(Guid id)
        {
            try
            {
                return context.Set<Area>().Where(x => x.Id == id).FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }
        
        public override IEnumerable<Area> GetAll()
        {
            try
            {
                return context.Set<Area>().ToList();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public Area GetByName(string name)
        {
            try
            {
                return this.context.Set<Area>().Where(a => a.Name == name).FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

    }
}