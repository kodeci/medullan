using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "";
            ViewBag.player = new User { SessionID = Guid.NewGuid().ToString() };
            return View(new Game { Next = "X" });
        }

        [HttpPost]
        public ActionResult Index(User player, string key, Dictionary<string, string> move)
        {
            ViewBag.Message = "";

            var existing = player;
            var board = DB.Games(key) ?? new Game { Key = key, Next = "X" };

            int x = -1, y = -1;

            if (move != null)
            {
                foreach (var item in move)
                {
                    var keys = item.Key.Split('-');

                    if (keys.Length != 2) continue;

                    x = int.Parse(keys[0]);
                    y = int.Parse(keys[1]);
                }
            }

            if (board.isValid(x, y))
            {
                board.assignPlayer(player);

                if (!board.isCurrentPlay(player))
                {
                    ViewBag.Message = "Not your Play!";
                }
                else
                {
                    if (!board.isMarked(x, y))
                    {
                        board.mark(x, y, player);
                    }
                    else
                    {
                        ViewBag.Message = "That Cell is already marked";
                    }
                }
            }

            var won = board.isWin();
            if (won != string.Empty)
            {
                if (won == player.Marker) ViewBag.Message = "You Won!";
                else ViewBag.Message = "You Lost!";
            }

            //DB.User(existing.SessionID, existing);
            DB.Games(key, board);

            ViewBag.player = existing;

            return View(board);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
