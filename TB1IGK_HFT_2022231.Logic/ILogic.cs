using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TB1IGK_HFT_2022231.Logic
{
    public interface ILogic<T> 
    {
        T GetOne(int id);
        IQueryable<T> GetAll();
        void Create(T input);
        void Delete(int id);
        void Update(T input);
    }
}
