using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Repository.Interface;

namespace TB1IGK_HFT_2022231.Repository
{
    public abstract class Repository<T> : IRepository<T>
        where T : class
    {
        public Repository(DbContext ctx)
        {
            this._Ctx = ctx;
        }
        protected DbContext _Ctx;

        public abstract void Create(T input);
        public abstract void Delete(int id);
        public IQueryable<T> GetAll()
        {
            return this._Ctx.Set<T>();
        }
        public abstract T GetOne(int id);
        public abstract void Update(T input);
        
    }
}
