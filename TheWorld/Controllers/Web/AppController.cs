using Microsoft.AspNet.Mvc;
using TheWorld.ViewModels;
using TheWorld.Models;
using System.Linq;
using LibGit2Sharp;
using System.Web.Script.Serialization;

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

            //string jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            string jsondata = new JavaScriptSerializer().Serialize(model);
              
            System.IO.File.WriteAllText(@"c:\\temp\\rooted\\" + "output.json", jsondata);
            //string rootedPath = Repository.Init("C:\\temp\\rooted");
            //File.WriteAllText(@"c:\user.json", json);
            //System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\temp\\rooted\\test.txt");
            //file.WriteLine(@"c:\\temp\\rooted\\test.txt", "sgsfgs");
            //file.WriteAllText(@"c:\\temp\\rooted\\user.json", jsonObject);
            return View();
        }
    }
}
