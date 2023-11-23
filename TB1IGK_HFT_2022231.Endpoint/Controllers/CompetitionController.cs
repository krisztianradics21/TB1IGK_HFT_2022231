using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TB1IGK_HFT_2022231.Endpoint.Services;
using TB1IGK_HFT_2022231.Logic;
using TB1IGK_HFT_2022231.Models;

namespace TB1IGK_HFT_2022231.Endpoint.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        ICompetitionLogic competitionLogic;
        IHubContext<SignalRHub> hub;

        public CompetitionController(ICompetitionLogic competitionLogic, IHubContext<SignalRHub> hub)
        {
            this.competitionLogic = competitionLogic;
            this.hub = hub;
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
            hub.Clients.All.SendAsync("CompetitionCreated", value);
        }

        // PUT api/<CompetitionController>/5
        [HttpPut]
        public void Put([FromBody] Competition value)
        {
            competitionLogic.Update(value);
            hub.Clients.All.SendAsync("CompetitionUpdated", value);
        }

        // DELETE api/<CompetitionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var competitionToDelete = this.competitionLogic.GetOne(id);
            competitionLogic.Delete(id);
            hub.Clients.All.SendAsync("CompetitionDeleted", competitionToDelete);
        }
    }
}
