using System;
using System.Linq.Expressions;
using Core.DataAccess;
using Entities.Concrete;

namespace Shared.DataAccess.Abstract
{
    public interface IOrderRepository : IEntityRepository<Order>
    {
        public Order? GetWithRelated(Expression<Func<Order, bool>> filter);
    }
}

