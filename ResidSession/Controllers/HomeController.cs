using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;
using System.Text;
using System.IO;

namespace RedisWeb.Controllers
{
    public class HomeController : Controller
    {
        private ConnectionMultiplexer RedisServers; // Handle to redis server (or multiple servers) should be stored and reused!
        private IDatabase _redisDb; // field for our Redis client class

        public HomeController()
        {
            RedisServers = ConnectionMultiplexer.Connect("localhost"); // Connecting to local instance on default port
            _redisDb = RedisServers.GetDatabase(); // here we get access object, it's cheap one so can be often created, it's simply wrapper arount API
        }

        public ActionResult Index()
        {
            Session.Add("Today", DateTime.UtcNow.DayOfWeek);

            if (Session["Something"] == null)
                ViewBag.SomethingImportant = "Not from session";
            else
                ViewBag.SomethingImportant = Session["Something"];

            return View();
        }

        public ActionResult BasicExample()
        {
            if (Session["Something"] == null)
                ViewBag.SomethingImportant = "Not from session";
            else
                ViewBag.SomethingImportant = Session["Something"];

            //ViewBag.SortedSet = getSortedSet();
            ViewBag.SortedSet = getSortedSetScores();

            return View();
        }

        public ActionResult SaveSet(string TextForSet, bool randomScores = false)
        {
            List<SortedSetEntry> rows = new List<SortedSetEntry>();
            Random rand = new Random();
            using(StringReader reader = new StringReader(TextForSet))
            {
                string line = String.Empty;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        if(randomScores)
                            rows.Add(new SortedSetEntry(line, rand.Next(0,50)));
                        else
                            rows.Add(new SortedSetEntry(line, 0.0));
                    }

                } while (line != null);
            }
            _redisDb.SortedSetAdd("OurTeam", rows.ToArray());

            return RedirectToAction("BasicExample");
        }

        public ActionResult ResetSortedSet()
        {
            _redisDb.KeyDelete("OurTeam");

            return RedirectToAction("BasicExample");
        }

        public ActionResult SessionExample()
        {
            ViewBag.Today = Session["Today"];

            if (Session["Something"] == null)
                ViewBag.SomethingImportant = "Not from session";
            else
                ViewBag.SomethingImportant = Session["Something"];

            return View();
        }

        public ActionResult AbandonExample()
        {
            Session.Abandon();

            return View();
        }

        [HttpPost]
        public ActionResult SaveSession(string NewValue)
        {
            Session["Something"] = NewValue;

            return RedirectToAction("SessionExample");
        }

        private string getSortedSet()
        {
            RedisValue[] sortedSet = _redisDb.SortedSetRangeByRank("OurTeam");

            if (sortedSet != null)
            {
                StringBuilder set = new StringBuilder();

                foreach (RedisValue row in sortedSet)
                {
                    set.Append(row);
                    set.Append("<br/>");
                }

                return set.ToString();
            }
            return String.Empty;
        }

        private string getSortedSetScores()
        {
            var sortedSet = _redisDb.SortedSetRangeByRankWithScores("OurTeam");
            if (sortedSet != null)
            {
                StringBuilder set = new StringBuilder();

                foreach (SortedSetEntry row in sortedSet)
                {
                    set.Append(row.Element);
                    set.Append(" - ");
                    set.Append(row.Score);
                    set.Append("<br/>");
                }
                return set.ToString();
            }
            return string.Empty;
        }
    }
}