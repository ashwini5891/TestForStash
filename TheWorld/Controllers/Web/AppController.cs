using Microsoft.AspNet.Mvc;
using TheWorld.ViewModels;
using TheWorld.Models;
using System.Linq;
using LibGit2Sharp;
using System.Web.Script.Serialization;
using System;

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
            if (ModelState.IsValid)
            {

                ViewBag.Message = "Mail Sent. Thanks!";

            }

            var Quote = new Quote()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PostCode = model.PostCode
            };
            _context.Quotes.Add(Quote);
            _context.SaveChanges();
            return View();
        }

        public IActionResult GetAQuoteViaGit()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GetAQuoteViaGit(QuoteViewModel model)
        {
            if (ModelState.IsValid)
            {

                ViewBag.Message = "Mail Sent. Thanks!";

            }
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
                Signature author = new Signature("James", "@jugglingnutcase", DateTime.Now);
                Signature committer = author;

                // Commit to the repository
                Commit commit = repo.Commit("Here's a commit i made!", author, committer);
            }

            return View();
        }
    }
}
