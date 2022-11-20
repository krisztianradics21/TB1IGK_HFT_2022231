using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Logic;
using TB1IGK_HFT_2022231.Models;

namespace TB1IGK_HFT_2022231.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        ICompetitionLogic competitionLogic;

        public CompetitionController(ICompetitionLogic competitionLogic)
        {
            this.competitionLogic = competitionLogic;
        }

        // GET: api/<CompetitionController>
        [HttpGet]
        public IEnumerable<Competition> Get()
        {
            return competitionLogic.GetAll();
        }

        // GET api/<CompetitionController>/5
        [HttpGet("{id}")]
        public Competition Get(int id)
        {
            return competitionLogic.GetOne(id);
        }

        // POST api/<CompetitionController>
        [HttpPost]
        public void Post([FromBody] Competition value)
        {
            competitionLogic.Create(value);
        }

        // PUT api/<CompetitionController>/5
        [HttpPut]
        public void Put([FromBody] Competition value)
        {
            competitionLogic.Update(value);
        }

        // DELETE api/<CompetitionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            competitionLogic.Delete(id);
        }
    }
}
