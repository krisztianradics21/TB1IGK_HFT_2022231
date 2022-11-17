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
    public class CompetitorRepository : Repository<Competitor>, IRepository<Competitor>
    {
        public CompetitorRepository(DbContext ctx) : base(ctx)
        {

        }
        public override void Create(Competitor input)
        {
            this._Ctx.Add(input);
            _Ctx.SaveChanges();
        }

        public override void Delete(int input)
        {
            Competitor c = GetOne(input);
            this._Ctx.Remove(c);
            _Ctx.SaveChanges();
        }

        public override Competitor GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public override void Update(Competitor input)
        {
            Competitor competitor = GetOne(input.Id);
            competitor.CompetitonID = input.CompetitonID;
            competitor.Name = input.Name;
            competitor.CategoryID = input.CategoryID;
            competitor.Nation = input.Nation;
            competitor.Age = input.Age;
            _Ctx.SaveChanges();
        }
    }
}
