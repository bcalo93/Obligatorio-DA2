using System;
using System.Collections.Generic;
using System.Linq;
using IndicatorsManager.DataAccess.Interface;
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
            return this.context.Set<IndicatorItem>().Where(i => i.Id == id)
                .Include(i => i.Indicator)
                .Include(i => i.Condition)
                .FirstOrDefault();
        }

        public override IEnumerable<IndicatorItem> GetAll()
        {
            return this.context.Set<IndicatorItem>()
                .Include(i => i.Indicator)
                .Include(i => i.Condition)
                .ToList();
        }

        public override void Remove(IndicatorItem entity)
        {
            this.context.Set<Component>().RemoveRange(entity.Condition.Accept(new VisitorComponentToList()));
            this.context.Set<IndicatorItem>().Remove(entity);
        }
    }

}