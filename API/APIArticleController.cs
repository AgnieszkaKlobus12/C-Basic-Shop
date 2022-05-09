using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lista10.Data;
using Lista10.Models;

namespace Lista11.API
{
    [Route("api/article")]
    [ApiController]
    public class APIArticleController : ControllerBase
    {
        private IRepository<Article> repository;

        public APIArticleController(IRepository<Article> repo) => repository = repo;

        // GET: api/Article
        [HttpGet]
        public IEnumerable<Article> Get() => repository.elements;

        // GET: api/Article/5
        [HttpGet("{id}")]
        public Article Get(int id) => repository[id];

        // PUT: api/Article/5
        [HttpPut("{id}")]
        public Article Put([FromBody] Article article) => repository.Update(article);

        // POST: api/Article
        [HttpPost]
        public Article Post([FromBody] Article article) => repository.Add(new Article
        {
            Id = article.Id,
            Name = article.Name,
            CategoryId = article.CategoryId,
            Price = article.Price
        });

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public void Delete(int id) => repository.Delete(id);

        [HttpGet("next/{cat}/{id}")]
        public IEnumerable<Article> GetNext(int cat, int id) => repository.GetNext(id, cat);

    }
}
