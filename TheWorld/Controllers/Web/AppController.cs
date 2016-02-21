using Microsoft.AspNet.Mvc;
using TheWorld.ViewModels;
using TheWorld.Models;
using System.Linq;
using LibGit2Sharp;
using System.Web.Script.Serialization;
using System;
using System.Diagnostics;

namespace TheWorld.Controllers.Web
{
  public class AppController : Controller
  {
        private QuoteContext _context;

        public AppController(QuoteContext context)
    {
            _context = context;
    }

    public IActionResult Index()
    {
            var quotes = _context.Quotes.OrderBy(t => t.Created).ToList();
      return View(quotes);
            //return View();
    }

    public IActionResult About()
    {
      return View();
    }

    public IActionResult GetAQuoteInSQL()
        {
            return View();
        }


    [HttpPost]
    public IActionResult GetAQuoteInSQL(QuoteViewModel model)
        {
            var watch = Stopwatch.StartNew();
            // the code that you want to measure comes here

            var Quote = new Quote()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PostCode = model.PostCode,
                Created = DateTime.Now
            };

            string quoteNumber = RandomString(8);
            Quote.QuoteNumber = quoteNumber;
            _context.Quotes.Add(Quote);
            _context.SaveChanges();
            var firstCompany = (from c in _context.Quotes select c).FirstOrDefault();
            var kvp = firstCompany.Id;

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            if (ModelState.IsValid)
            {

                ViewBag.Message = "Thanks! Your Quote is saved. Your quote number is: " + quoteNumber + "\\n" + "Time taken: " + elapsedMs + "ms";

            }
            return View();
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public IActionResult GetAQuoteViaGit()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetAQuoteViaGit(QuoteViewModel model)
        {
            var watch = Stopwatch.StartNew();

            string rootedPath = Repository.Init("C:\\temp\\rooted\\path");
            string jsondata = new JavaScriptSerializer().Serialize(model);
              
            //System.IO.File.WriteAllText(@"c:\\temp\\rooted\\" + "output.json", jsondata);

            using (var repo = new Repository("C:\\temp\\rooted\\path"))
            {
                // Write content to file system
                System.IO.File.WriteAllText(System.IO.Path.Combine(repo.Info.WorkingDirectory, "output.json"), jsondata);

                // Stage the file
                repo.Stage("output.json");

                // Create the committer's signature and commit
                Signature author = new Signature(model.FirstName, model.Email, DateTime.Now);
                Signature committer = author;

                // Commit to the repository
                try {
                    Commit commit = repo.Commit("Quote committed", author, committer);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    if (ModelState.IsValid)
                    {

                        ViewBag.Message = "Thanks! Your Quote is saved. Your quote number is: " + commit.Id + "\\n" + "Time taken: " + elapsedMs + "ms";

                    }

                }
                catch(Exception ex)
                {
                    ViewBag.Message = "Nothing to commit";
                }
                    

            }

            return View();
        }
    }
}
