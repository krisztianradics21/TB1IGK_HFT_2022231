using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Models;
using TB1IGK_HFT_2022231.Repository.Interface;

namespace TB1IGK_HFT_2022231.Logic
{
    public class CompetitionLogic : ICompetitionLogic
    {
        IRepository<Competition> competitionRepo;
        IRepository<Category> categoryRepo;
        IRepository<Competitor> competitorRepo;

        public CompetitionLogic(IRepository<Competition> competitionRepo, IRepository<Category> categoryRepo, IRepository<Competitor> competitorRepo)
        {
            this.competitionRepo = competitionRepo;
            this.categoryRepo = categoryRepo;
            this.competitorRepo = competitorRepo;
        }


        public void Create(Competition input)
        {
            competitionRepo.Create(input);
        }

        public void Delete(int input)
        {
            competitionRepo.Delete(input);
        }

        public IQueryable<Competition> GetAll()
        {
            return competitionRepo.GetAll();
        }

        public Competition GetOne(int id)
        {
            return competitionRepo.GetOne(id);
        }

        public void Update(Competition input)
        {
            competitionRepo.Update(input);
        }

        public IEnumerable<object> CompetitionBasedOnDistance(int competition)
        {
            return (from comp in GetAll()
                    join competitor in competitorRepo.GetAll() on comp.CompetitorID equals competitor.Id
                    join opponent in competitorRepo.GetAll() on comp.OpponentID equals opponent.Id
                    where comp.ID == competition
                    select new
                    {
                        CompetitorName = competitor.Name,
                        OpponentName = opponent.Name,
                        Distance = comp.Distance
                    });
                   
        }

        public IEnumerable<object> Competition_BasedOnCompetitorsNameAndNation()
        {
            return from competition in GetAll()
                   join competitor in competitorRepo.GetAll() on competition.CompetitorID equals competitor.Id
                   join opponent in competitorRepo.GetAll() on competition.OpponentID equals opponent.Id
                   select new
                   {
                       CompetitionLocation = competition.Location,
                       CompetitorName = competitor.Name,
                       CompetitorNation = competitor.Nation,
                       OpponentName = opponent.Name,
                       OpponentNation = opponent.Nation
                   };
        }

        public IEnumerable<object> OpponentsByName()
        {
            return from competition in GetAll()
                   join competitor in competitorRepo.GetAll() on competition.CompetitorID equals competitor.Id
                   join opponent in competitorRepo.GetAll() on competition.OpponentID equals opponent.Id
                   select new
                   {
                       Competitor = competitor.Name,
                       Opponent = opponent.Name,
                   };
        }
    }
}
