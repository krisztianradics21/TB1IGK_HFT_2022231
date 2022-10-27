using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Models;
using TB1IGK_HFT_2022231.Repository.Interface;

namespace TB1IGK_HFT_2022231.Repository
{
    public class CategoryRepository : Repository<Category>, IRepository<Category>
    {
        public CategoryRepository(DbContext ctx) : base(ctx)
        {

        }
        public override void Create(Category input)
        {
            this._Ctx.Add(input);
            _Ctx.SaveChanges();
        }

        public override void Delete(int input)
        {
            Category c = GetOne(input);
            this._Ctx.Remove(c);
            _Ctx.SaveChanges();
        }

        public override Category GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.ID.Equals(id));
        }

        public override void Update(Category input)
        {
            Category category = GetOne(input.ID);
            category.AgeGroup = input.AgeGroup;
            category.BoatCategory = input.BoatCategory;
            _Ctx.SaveChanges();
        }
    }
}
