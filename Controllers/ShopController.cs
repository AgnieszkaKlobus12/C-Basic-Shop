using Lista10.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lista10.Controllers
{
    public class ShopController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public ShopController(MyDbContext context, IHttpContextAccessor accessor, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<IActionResult> ShopAsync(int? id)
        {
            var myDbContext = _context.Category.Include(a => a.Articles.OrderBy(b => b.Id));
            var list = myDbContext.ToList();
            if (list.Count > 0)
            {
                ViewData["Cat"] = myDbContext.ToList().ElementAt(0).Name;
            }
            foreach (var cat in list)
            {
                if (cat.Id == id)
                {
                    ViewData["Cat"] = cat.Name;
                }
            }
            return View(await myDbContext.ToListAsync());
        }

        public void SetCookie(string key, string value, int days = 7)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(days);
            Response.Cookies.Append(key, value, options);
        }

        [Authorize(Policy = "BasketPolicy")]
        public async Task<IActionResult> More(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            string value_s = Request.Cookies["art" + article.Id.ToString()];
            int value = 1;
            if (value_s != null)
            {
                value = Int32.Parse(value_s);
                value++;
            }
            SetCookie("art" + article.Id.ToString(), value.ToString());
            return RedirectToAction("Basket");
        }

        [Authorize(Policy = "BasketPolicy")]

        public async Task<IActionResult> Less(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = await _context.Article
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            string value_s = Request.Cookies["art" + article.Id.ToString()];
            if (value_s != null)
            {
                int value = Int32.Parse(value_s);
                value--;
                if (value < 1)
                {
                    SetCookie("art" + article.Id.ToString(), value.ToString(), -1);
                }
                else
                {
                    SetCookie("art" + article.Id.ToString(), value.ToString());
                }
            }
            return RedirectToAction("Basket");
        }

        [Authorize(Policy = "BasketPolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = await _context.Article
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            string value_s = Request.Cookies["art" + article.Id.ToString()];
            if (value_s != null)
            {
                SetCookie("art" + article.Id.ToString(), "0", -1);
            }
            return RedirectToAction("Basket");
        }

        [Authorize(Policy = "BasketPolicy")]
        public async Task<IActionResult> Basket()
        {
            ICollection<string> keyes = Request.Cookies.Keys;
            ICollection<int> keyes_i = new LinkedList<int>();
            double total = 0;
            foreach (string key in keyes)
            {
                if (key.Contains("art"))
                {
                    double value = Int32.Parse(Request.Cookies[key]);
                    string keyN = key.Substring(3);
                    if (value > 0)
                    {
                        int key_i = Int32.Parse(keyN);
                        var article = await _context.Article.FirstOrDefaultAsync(m => m.Id == key_i);
                        if (article != null)
                        {
                            keyes_i.Add(key_i);
                            ViewData[keyN] = value;
                            ViewData["total" + keyN] = value * article.Price;
                            total += value * article.Price;
                        }
                        else
                        {
                            SetCookie(key, "0", -1);
                        }
                    }
                }
            }
            if (keyes_i.Count < 1)
            {
                return View("Empty");
            }
            ViewData["total"] = total;
            var myDbContext = _context.Article.Include(a => a.Category).Where(a => keyes_i.Contains(a.Id));
            return View(await myDbContext.ToListAsync());
        }

        [Authorize]
        [Authorize(Policy = "BasketPolicy")]
        public async Task<IActionResult> Show()
        {
            ICollection<string> keyes = Request.Cookies.Keys;
            ICollection<int> keyes_i = new LinkedList<int>();
            double total = 0;
            foreach (string key in keyes)
            {
                if (key.Contains("art"))
                {
                    double value = Int32.Parse(Request.Cookies[key]);
                    string keyN = key.Substring(3);
                    if (value > 0)
                    {
                        int key_i = Int32.Parse(keyN);
                        var article = await _context.Article.FirstOrDefaultAsync(m => m.Id == key_i);
                        if (article != null)
                        {
                            keyes_i.Add(key_i);
                            ViewData[keyN] = value;
                            ViewData["total" + keyN] = value * article.Price;
                            total += value * article.Price;
                        }
                        else
                        {
                            SetCookie(key, "0", -1);
                        }
                    }
                }
            }
            string[] payments = { "GooglePay", "Karta Kredytowa", "Gotówka" };
            ViewBag.Payment = payments;
            ViewData["total"] = total;
            var myDbContext = _context.Article.Include(a => a.Category).Where(a => keyes_i.Contains(a.Id));
            return View(await myDbContext.ToListAsync());
        }

        [Authorize]
        [Authorize(Policy ="BasketPolicy")]
        public async Task<IActionResult> Confirm([Bind("name")] string name, 
            [Bind("street")] string street, [Bind("house")] string house, [Bind("flat")] string flat, 
            [Bind("postalCode")] string postalCode, [Bind("city")] string city, [Bind("payment")] string payment)
        {
            ICollection<string> keyes = Request.Cookies.Keys;
            ICollection<int> keyes_i = new LinkedList<int>();
            double total = 0;
            foreach (string key in keyes)
            {
                if (key.Contains("art"))
                {
                    double value = Int32.Parse(Request.Cookies[key]);
                    string keyN = key.Substring(3);
                    if (value > 0)
                    {
                        int key_i = Int32.Parse(keyN);
                        var article = await _context.Article.FirstOrDefaultAsync(m => m.Id == key_i);
                        if (article != null)
                        {
                            keyes_i.Add(key_i);
                            ViewData[keyN] = value;
                            ViewData["total" + keyN] = value * article.Price;
                            total += value * article.Price;
                        }
                        SetCookie(key, "0", -1);
                    }
                }
            }
            ViewBag.Name = name;
            ViewBag.Street = street;
            ViewBag.House = house;
            ViewBag.Flat = flat;
            ViewBag.PostalCode = postalCode;
            ViewBag.City = city;
            ViewBag.Payment = payment;
            ViewData["total"] = total;
            var myDbContext = _context.Article.Include(a => a.Category).Where(a => keyes_i.Contains(a.Id));
            return View(await myDbContext.ToListAsync());
        }
    }
}