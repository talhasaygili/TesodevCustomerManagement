using System;
using Core.DataAccess.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.Abstract;

namespace Shared.DataAccess.Concrete.EntityFramework
{
    public class EfCustomerRepository : EfEntityRepositoryBase<Customer>, ICustomerRepository
    {
        public EfCustomerRepository(DbContext context) : base(context)
        {


        }
    }
}