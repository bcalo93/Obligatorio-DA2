using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;
using IndicatorsManager.DataAccess.Interface;
using IndicatorsManager.DataAccess.Interface.Exceptions;

namespace IndicatorsManager.DataAccess
{
    public class AreaRepository : BaseRepository<Area>, IAreaQuery
    {
        public AreaRepository(DbContext context): base(context) { }

        public override Area Get(Guid id)
        {
            return context.Set<Area>().Where(x => x.Id == id).FirstOrDefault();
        }
        
        public override IEnumerable<Area> GetAll()
        {
           return context.Set<Area>().ToList();
        }

        public Area GetByName(string name)
        {
            return this.context.Set<Area>().Where(a => a.Name == name).FirstOrDefault();
        }

    }
}