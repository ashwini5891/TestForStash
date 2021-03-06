using Microsoft.AspNet.Mvc;
using FunWithGit.ViewModels;
using FunWithGit.Models;
using System.Linq;
using LibGit2Sharp;
using System.Web.Script.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FunWithGit.Controllers.Web
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
      return View();
    }

    public IActionResult About()
    {
      return View();
    }

        public IActionResult RetrieveQuoteSql()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RetrieveQuoteSql(string quoteNumber)
        {
            var watch = Stopwatch.StartNew();
            Quote quote = _context.Quotes
                    .Where(b => b.QuoteNumber == quoteNumber)
                    .FirstOrDefault();

            QuoteViewModel viewModel = new QuoteViewModel()
            {
                FirstName = quote.FirstName,
                LastName = quote.LastName,
                Email = quote.Email
            };
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            ViewBag.Message = "Time taken: " + elapsedMs + " ms";
            return View("DisplayQuoteSql", viewModel);
        }

        public IActionResult RetrieveQuoteGit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RetrieveQuoteGit(string quoteNumber)
        {
            var watch = Stopwatch.StartNew();
            using (var repo = new Repository("C:\\temp\\rooted\\path"))
            {
                var commit = repo.Lookup<Commit>(quoteNumber);
                var treeEntry = commit["output.json"];

                var blob = (Blob)treeEntry.Target;

                var contentStream = blob.GetContentStream();

                using (var tr = new StreamReader(contentStream, Encoding.UTF8))
                {
                    string content = tr.ReadToEnd();
                    Quote jsondata = new JavaScriptSerializer().Deserialize<Quote>(content);
                    QuoteViewModel viewModel = new QuoteViewModel()
                    {
                        FirstName = jsondata.FirstName,
                        LastName = jsondata.LastName,
                        Email = jsondata.Email
                    };

                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    ViewBag.Message = "Time taken: " + elapsedMs + " ms";
                    return View("DisplayQuoteSql", viewModel);
                }
            }
        }

        public IActionResult DisplayQuoteSql(QuoteViewModel model)
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
                ViewBag.Message = "Quote number is: " + quoteNumber + " Time taken: " + elapsedMs + "ms";
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
                        ViewBag.Message = "Quote number is: " + commit.Id + " Time taken: " + elapsedMs + "ms";
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
