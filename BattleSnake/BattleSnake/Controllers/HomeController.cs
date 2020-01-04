using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using BattleSnake.Models;


namespace BattleSnake.Controllers
{
    public class HomeController : Controller
    {

        
        private static readonly List<LobbyModel> lobbies;

        private const int MAXWIDTH = 24;
        private const int MAXHEIGHT = 24;
        static HomeController()
        {
            lobbies = new List<LobbyModel>();
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HostGame(UserModel user)
        {
            bool alreadyHosting = false;
            foreach(LobbyModel lobby in lobbies)
            {
                if (lobby.Host.Equals(user.username))
                {
                    alreadyHosting = true;
                }
            }
            if (!alreadyHosting)
            {
                lobbies.Add(new LobbyModel
                {
                    Host = user.username,
                    Member = "",
                    Ready = false
                });
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult HostPage(UserModel user)
        {
            return View(user);
        }

        public ActionResult CheckStatus(UserModel user)
        {
            foreach(LobbyModel lobby in lobbies)
            {
                if (user.username.Equals(lobby.Host))
                {
                    return Json(lobby.Member, JsonRequestBehavior.AllowGet); 
                }
            }
            return Json("You aren't a host", JsonRequestBehavior.AllowGet );
        }
        [HttpPost]
        public ActionResult JoinHost(LobbyModel sentLobby)
        {
            for(int i = 0; i < lobbies.Count; i++)
            {
                var lobby = lobbies[i];
                if (sentLobby.Host.Equals(lobby.Host))
                {
                    lobbies[i] = sentLobby;
                    return Json(lobbies[i].Host, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("could not find host", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHosts()
        {
            return Json(lobbies, JsonRequestBehavior.AllowGet);
        }

        public ActionResult JoinPage(UserModel user)
        {
           
            return View(user);
        }

        public ActionResult CheckStart(UserModel user)
        {
            for (int i = 0; i < lobbies.Count; i++)
            {
                var lobby = lobbies[i];
                if (user.username.Equals(lobby.Member))
                {
                    if (lobby.Ready)
                    {
                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("couldnt find game", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Game(UserModel user)
        {
            int index = getIndex(user.username);
            if(index == -1)
            {
                return View(new UserModel { username = "usernotfound" });
            }
            lobbies[index].Ready = true;
            lobbies[index].HostSnake = new SnakeModel
            {
                X = 6,
                Y = 12
            };
            lobbies[index].MemberSnake = new SnakeModel
            {
                X = 18,
                Y = 12
            };
            Random rand = new Random();
            int x, y;
            while (true)
            {
                x = rand.Next(24);
                y = rand.Next(24); 

                if(y != 12 && x != 6 && x != 18)
                {
                    break;
                }
            }
            lobbies[index].Fruit = new SnakeModel
            {
                X = x,
                Y = y
            };
            lobbies[index].HostBody = new List<SnakeModel>();
            lobbies[index].MemBody = new List<SnakeModel>();
            lobbies[index].HostWalls = new List<SnakeModel>();
            lobbies[index].MemWalls = new List<SnakeModel>();
            lobbies[index].MemSize = 0;
            lobbies[index].HostSize = 0;
            lobbies[index].lost = "no";


            lobbies[index].Board = new string[25,25];
            lobbies[index].Board[lobbies[index].Fruit.X, lobbies[index].Fruit.Y] = "fruit";
            lobbies[index].Board[lobbies[index].HostSnake.X, lobbies[index].HostSnake.Y] = "hHead";
            lobbies[index].Board[lobbies[index].MemberSnake.X, lobbies[index].MemberSnake.Y] = "mHead";
            return View(user);
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult GetBoard(UserModel model)
        {
            return Json(lobbies[getIndex(model.username)], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult KeyPush(KeyModel model)
        {
            LobbyModel lobby = lobbies[getIndex(model.Username)];
            SnakeModel hostPieceAdded = null;
            SnakeModel memPieceAdded = null;

            if(lobby.lost != "no")
            {
                return Json(lobby, JsonRequestBehavior.AllowGet);
            }
            int x;
            int y;
            bool isHost = false;
            if (lobby.Host == model.Username)
            {
                isHost = true;
                x = lobby.HostSnake.X;
                y = lobby.HostSnake.Y;
            }
            else
            {
                x = lobby.MemberSnake.X;
                y = lobby.MemberSnake.Y;
            }

            string key = model.Key;
            if (model.Drop)
            {
                if (isHost)
                {
                    lobby.HostWalls.AddRange(lobby.HostBody);
                    lobby.HostBody = new List<SnakeModel>();
                }
                else
                {
                    lobby.MemWalls.AddRange(lobby.MemBody);
                    lobby.HostBody = new List<SnakeModel>();
                }
            }

            if(key.CompareTo("w") == 0)
            {
                
                y = y - 1;
                if (y < 0)
                {
                    y = 0;
                }
                
                
            } 
            else if(key.CompareTo("a") == 0)
            {
                x = x - 1;
                if (x < 0)
                {
                    x = 0;
                }
                
            }
            else if (key.CompareTo("s") == 0)
            {
                y = y + 1;
                if (y > MAXHEIGHT)
                {
                    y = MAXHEIGHT;
                }
                
            }
            else if (key.CompareTo("d") == 0)
            {
                x += 1;
                if(x > MAXWIDTH)
                {
                   x = MAXWIDTH;
                }
                
            }
            if (isHost)
            {
                lobby.HostBody.Add(new SnakeModel
                {
                    X = lobby.HostSnake.X,
                    Y = lobby.HostSnake.Y
                });
                hostPieceAdded = lobby.HostSnake;
                lobby.HostBody.RemoveAt(0);
                lobby.HostSnake.X = x;
                lobby.HostSnake.Y = y;
                lobby.Board[x, y] = "hHead";
            }
            else
            {
                lobby.MemBody.Add(new SnakeModel
                {
                    X = lobby.MemberSnake.X,
                    Y = lobby.MemberSnake.Y
                });
                memPieceAdded = lobby.MemberSnake;
                lobby.MemBody.RemoveAt(0);
                lobby.MemberSnake.X = x;
                lobby.MemberSnake.Y = y;
                lobby.Board[x, y] = "hHead";
            }
            if(lobby.MemberSnake.X == lobby.Fruit.X && lobby.MemberSnake.Y == lobby.Fruit.Y)
            {
                lobby.MemSize += 1;
                lobby.MemBody.Add(memPieceAdded);
                lobby.Fruit = newFruitLocation(lobby);

            }
            else if (lobby.HostSnake.X == lobby.Fruit.X && lobby.HostSnake.Y == lobby.Fruit.Y)
            {
                lobby.HostSize += 1;
                lobby.HostBody.Add(hostPieceAdded);
                lobby.Fruit = newFruitLocation(lobby);
            }

            if (checkWin(lobby, isHost))
            {
                if (isHost)
                {
                    lobby.lost = lobby.Member;
                }
                else
                {
                    lobby.lost = lobby.Host;
                }
            }

            lobbies[getIndex(model.Username)] = lobby;
            return Json(lobby, JsonRequestBehavior.AllowGet);
        }

        private static bool checkWin(LobbyModel lobby, bool isHost)
        {
            bool hasLost = false;
            SnakeModel head;

            if (isHost)
            {
                head = lobby.HostSnake;
                if (head.Compare(lobby.MemberSnake))
                {
                    hasLost = true;
                }
                foreach(var point in lobby.MemBody)
                {
                    if (head.Compare(point))
                    {
                        hasLost = true;
                    }
                }
                foreach(var point in lobby.MemWalls)
                {
                    if (head.Compare(point))
                    {
                        hasLost = true;
                    }
                }
            }
            else
            {
                head = lobby.MemberSnake;
                if (head.Compare(lobby.HostSnake))
                {
                    hasLost = true;
                }
                foreach (var point in lobby.HostBody)
                {
                    if (head.Compare(point))
                    {
                        hasLost = true;
                    }
                }
                foreach (var point in lobby.HostWalls)
                {
                    if (head.Compare(point))
                    {
                        hasLost = true;
                    }
                }
            }

            
            return hasLost;
        }
        private static SnakeModel newFruitLocation(LobbyModel lobby)
        {
            Random rand = new Random();
            int x = rand.Next(0, 24);
            int y = rand.Next(0, 24);

            return new SnakeModel { X = x, Y = y };
        }

        private static int getIndex(String username)
        {
            for (int i = 0; i < lobbies.Count; i++)
            {
                var lobby = lobbies[i];

                if (username.Equals(lobby.Host) || username.Equals(lobby.Member))
                {
                    return lobbies.IndexOf(lobby);
                }
            }
            return -1;
        }
    }
}