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
    public class CompetitorController : ControllerBase
    {
        ICompetitorLogic competitorLogic;

        public CompetitorController(ICompetitorLogic competitorLogic)
        {
            this.competitorLogic = competitorLogic;
        }

        // GET: api/<CompetitorController>
        [HttpGet]
        public IEnumerable<Competitor> Get()
        {
            return competitorLogic.GetAll();
        }

        // GET api/<CompetitorController>/5
        [HttpGet("{id}")]
        public Competitor Get(int id)
        {
            return competitorLogic.GetOne(id);
        }

        // POST api/<CompetitorController>
        [HttpPost]
        public void Post([FromBody] Competitor value)
        {
            competitorLogic.Create(value);
        }

        // PUT api/<CompetitorController>/5
        [HttpPut]
        public void Put([FromBody] Competitor value)
        {
            competitorLogic.Update(value);
        }

        // DELETE api/<CompetitorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            competitorLogic.Delete(id);
        }
    }
}
