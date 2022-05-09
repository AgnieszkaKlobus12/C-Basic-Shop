using Lista10.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lista11.API
{

    [Route("api/category")]
    [ApiController]
    public class APICategoryController : ControllerBase
    {
        private IRepository<Category> repository;

        public APICategoryController(IRepository<Category> repo) => repository = repo;

        // GET: api/Category
        [HttpGet]
        public IEnumerable<Category> Get() => repository.elements;

        // GET: api/Category/5
        [HttpGet("{id}")]
        public Category Get(int id) => repository[id];

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public Category Put([FromBody] Category cat) => repository.Update(cat);

        // POST: api/Category
        [HttpPost]
        public Category Post([FromBody] Category category) => repository.Add(new Category
        {
            Id = category.Id,
            Name = category.Name
        });

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public void Delete(int id) => repository.Delete(id);

    }

}
