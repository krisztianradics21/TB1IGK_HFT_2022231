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
    public class CategoryController : ControllerBase
    {
        ICategoryLogic categoryLogic;
        IHubContext<SignalRHub> hub;

        public CategoryController(ICategoryLogic categoryLogic, IHubContext<SignalRHub> hub)
        {
            this.categoryLogic = categoryLogic;
            this.hub = hub;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return categoryLogic.GetAll();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return categoryLogic.GetOne(id);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public void Post([FromBody] Category value)
        {
            categoryLogic.Create(value);
            hub.Clients.All.SendAsync("CategoryCreated", value);
        }

        // PUT api/<CategoryController>/5
        [HttpPut]
        public void Put([FromBody] Category value)
        {
            categoryLogic.Update(value);
            hub.Clients.All.SendAsync("CategoryUpdated", value);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var categoryToDelete = this.categoryLogic.GetOne(id);
            categoryLogic.Delete(id);
            hub.Clients.All.SendAsync("CategoryDeleted", categoryToDelete);
        }
    }
}
