using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Logic;

namespace TB1IGK_HFT_2022231.Endpoint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StatController : ControllerBase
    {
        ICategoryLogic categoryLogic;
        ICompetitorLogic competitorLogic;
        ICompetitionLogic competitionLogic;

        public StatController(ICategoryLogic categoryLogic, ICompetitorLogic competitorLogic, ICompetitionLogic competitionLogic)
        {
            this.categoryLogic = categoryLogic;
            this.competitorLogic = competitorLogic;
            this.competitionLogic = competitionLogic;
        }


        // GET: api/<StatController>
        [HttpGet]
        public IEnumerable<KeyValuePair<string, string>> CompetitorsByBoatCategory()
        {
            return categoryLogic.CompetitorsByBoatCategory();
        }

       
        [HttpGet]
        public IEnumerable<object> CompetitorWithAllRelevantData()
        {
            return competitorLogic.CompetitorWithAllRelevantData();
        }

        
        [HttpGet]
        public double AVGAge()
        {
            return competitorLogic.AVGAge();
        }

       
        [HttpGet]
        public IEnumerable<object> Competition_BasedOnCompetitorsNameAndNation()
        {
            return competitionLogic.Competition_BasedOnCompetitorsNameAndNation();
        }

       
        [HttpGet]
        public object CompetitionBasedOnDistance(int competition)
        {
            return competitionLogic.CompetitionBasedOnDistance(competition);
        }


        [HttpGet]
        public IEnumerable<object> OpponentsByName()
        {
            return competitionLogic.OpponentsByName();
        }
    }
}
