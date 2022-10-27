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
    public class CompetitionRepository : Repository<Competition>, IRepository<Competition>
    {
        public CompetitionRepository(DbContext ctx) : base(ctx)
        {

        }
        public override void Create(Competition input)
        {
            this._Ctx.Add(input);
            _Ctx.SaveChanges();
        }

        public override void Delete(int id)
        {
            Competition competition = GetOne(id);
            this._Ctx.Remove(competition);
            _Ctx.SaveChanges();
        }

        public override Competition GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.ID.Equals(id));
        }

        public override void Update(Competition input)
        {
            Competition update = GetOne(input.ID);
            update.CompetitorID = input.CompetitorID;
            update.OpponentID = input.OpponentID;
            update.NumberOfRacesAgainstEachOther = input.NumberOfRacesAgainstEachOther;
            update.Location = input.Location;
            update.Distance = input.Distance;
            _Ctx.SaveChanges();
        }
    }
}
