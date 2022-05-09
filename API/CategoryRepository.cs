using Lista10.Data;
using Lista10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lista11.API
{
    class CategoryRepository : IRepository<Category>
    {
        private readonly MyDbContext _context;
        public CategoryRepository(MyDbContext context)
        {
            _context = context;
        }

        public Category this[int id] => _context.Category
                .FirstOrDefault(m => m.Id == id);

        public IEnumerable<Category> elements => _context.Category;

        public Category Add(Category element)
        {
            _context.Add(element);
            _context.SaveChanges();
            return element;
        }

        public void Delete(int id)
        {
            var category = _context.Category.Find(id);
            var art = _context.Article.Where(a => a.CategoryId == id).Any();
            if (art)
            {
                throw new ArgumentException();
            }
            _context.Category.Remove(category);
            _context.SaveChanges();
        }

        public Category Update(Category element)
        {
            _context.Update(element);
            _context.SaveChanges();
            return element;
        }


        public IEnumerable<Category> GetNext(int id, int cat)
        {
            return _context.Category.Where(s => s.Id > id).OrderBy(s => s.Id).Take(3);
        }
    }
}
