using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using IndicatorsManager.DataAccess.Interface.Exceptions;
using IndicatorsManager.DataAccess.Visitors;
using IndicatorsManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndicatorsManager.DataAccess
{
    public class IndicatorItemRepository : BaseRepository<IndicatorItem>
    {
        public IndicatorItemRepository(DbContext context) : base(context) { }
        public override IndicatorItem Get(Guid id)
        {
            try
            {
                return this.context.Set<IndicatorItem>().Where(i => i.Id == id)
                    .Include(i => i.Indicator)
                    .Include(i => i.Condition)
                    .FirstOrDefault();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public override IEnumerable<IndicatorItem> GetAll()
        {
            try
            {
                return this.context.Set<IndicatorItem>()
                    .Include(i => i.Indicator)
                    .Include(i => i.Condition)
                    .ToList();
            }
            catch(SqlException ex)
            {
                throw new DataAccessException(CONNECTION_ERROR, ex);
            }
        }

        public override void Remove(IndicatorItem entity)
        {
            this.context.Set<Component>().RemoveRange(entity.Condition
                .Accept(new VisitorComponentToList()));
            this.context.Set<IndicatorItem>().Remove(entity);
        }
    }

}