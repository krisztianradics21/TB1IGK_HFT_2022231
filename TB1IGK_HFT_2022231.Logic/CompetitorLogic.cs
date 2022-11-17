using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Models;
using TB1IGK_HFT_2022231.Repository.Interface;

namespace TB1IGK_HFT_2022231.Logic
{
    public class CompetitorLogic : ICompetitorLogic
    {
        IRepository<Competitor> competitorRepo;
        IRepository<Competition> competitionRepo;
        IRepository<Category> categoryRepo;

        public CompetitorLogic(IRepository<Competitor> competitorRepo, IRepository<Competition> competitionRepo, IRepository<Category> categoryRepo)
        {
            this.competitorRepo = competitorRepo;
            this.competitionRepo = competitionRepo;
            this.categoryRepo = categoryRepo;
        }

        public void Create(Competitor input)
        {
            competitorRepo.Create(input);
        }

        public void Delete(int input)
        {
            competitorRepo.Delete(input);
        }

        public IQueryable<Competitor> GetAll()
        {
            return competitorRepo.GetAll();
        }

        public Competitor GetOne(int id)
        {
            return competitorRepo.GetOne(id);
        }

        public void Update(Competitor input)
        {
            competitorRepo.Update(input);
        }
        public double AVGAge()
        {
            return competitorRepo.GetAll()
                 .Average(t => t.Age);
        }

        public IEnumerable<object> CompetitorWithAllRelevantData()
        {
            return from competitor in GetAll()
                   join category in categoryRepo.GetAll() on competitor.CategoryID equals category.CategoryNumber
                   join competition in competitionRepo.GetAll() on competitor.CompetitonID equals competition.ID
                   select new
                   {
                       CompetitorName = competitor.Name,
                       CompetitorAge = competitor.Age,
                       CompetitorNation = competitor.Nation,
                       CompetitionLocation = competition.Location,
                       AgeGroup = category.AgeGroup,
                       BoatCategory = category.BoatCategory
                   };
        }
    }
}
