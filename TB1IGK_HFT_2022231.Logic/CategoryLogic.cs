using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Models;
using TB1IGK_HFT_2022231.Repository.Interface;

namespace TB1IGK_HFT_2022231.Logic
{
    public class CategoryLogic : ICategoryLogic
    {
        IRepository<Category> categoryRepo;
        IRepository<Competitor> competitorRepo;

        public CategoryLogic(IRepository<Category> categoryRepo, IRepository<Competitor> competitorRepo)
        {
            this.categoryRepo = categoryRepo;
            this.competitorRepo = competitorRepo;
        }

        public void Create(Category input)
        {
            categoryRepo.Create(input);
        }

        public void Delete(int id)
        {
            categoryRepo.Delete(id);
        }

        public IQueryable<Category> GetAll()
        {
            return categoryRepo.GetAll();
        }

        public Category GetOne(int id)
        {
            return categoryRepo.GetOne(id);
        }

        public void Update(Category input)
        {
            categoryRepo.Update(input);
        }
        public IEnumerable<KeyValuePair<string, string>> CompetitorsByBoatCategory()
        {
            var boatCat = from competitor in competitorRepo.GetAll()
                          join category in categoryRepo.GetAll() on competitor.CategoryID equals category.CategoryNumber
                          select new KeyValuePair<string, string>(category.BoatCategory, competitor.Name);

            return boatCat;
        }
    }
}
