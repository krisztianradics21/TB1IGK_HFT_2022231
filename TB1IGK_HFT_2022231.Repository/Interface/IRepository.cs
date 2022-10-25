using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB1IGK_HFT_2022231.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        //CRUD methods
        T GetOne(int id);
        IQueryable<T> GetAll();
        void Create(T input);
        void Delete(int id);
        void Update(T input);
    }
}
