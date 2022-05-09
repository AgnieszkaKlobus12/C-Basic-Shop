using Lista10.Data;
using Lista10.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lista11.API
{
    class ArticleRepository : IRepository<Article>
    {
        private readonly MyDbContext _context;
        private readonly IHostingEnvironment _env;
        private static string path;
        private static string defualt = "default.png";

        public ArticleRepository(MyDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _env = environment;
            path = Path.Combine(_env.WebRootPath, "upload");
        }

        public Article this[int id] => _context.Article
                .Include(a => a.Category)
                .FirstOrDefault(m => m.Id == id);

        public IEnumerable<Article> elements => _context.Article.Include(a => a.Category);

        public Article Add(Article element)
        {
            element.Image = defualt;
            _context.Add(element);
            _context.SaveChanges();
            return element;
        }

        public void Delete(int id)
        {
            var article = _context.Article.Find(id);
            _context.Article.Remove(article);
            _context.SaveChanges();
            if (article.Image != "default.png" && System.IO.File.Exists(path + article.Image))
            {
                System.IO.File.Delete(path + article.Image);
            }
        }

        public IEnumerable<Article> GetNext(int id, int cat)
        {
            return _context.Article.Where(s => s.Id > id).Where(s=> s.CategoryId == cat).OrderBy(s => s.Id).Take(3);
        }

        public Article Update(Article element)
        {
            element.Image = defualt;
            _context.Update(element);
            _context.SaveChanges();
            return element;
        }
    }
}
