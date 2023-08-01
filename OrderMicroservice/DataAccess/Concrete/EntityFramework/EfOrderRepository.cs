using System;
using Core.DataAccess;
using System.Linq.Expressions;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.Abstract;

namespace Shared.DataAccess.Concrete.EntityFramework
{
    public class EfOrderRepository : EfEntityRepositoryBase<Order>, IOrderRepository
    {
        public EfOrderRepository(DbContext context) : base(context)
        {

        }

        // Get the order with all the related other data in the tables.
        public Order? GetWithRelated(Expression<Func<Order, bool>> filter)
        {
            return Context.Set<Order>()
                .Include(x => x.Address)
                .Include(x => x.Customer)
                .Include(x => x.Product)
                .AsNoTracking()
                .SingleOrDefault(filter);
        }
    }
}

