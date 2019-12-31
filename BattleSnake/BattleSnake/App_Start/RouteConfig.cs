using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BattleSnake
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "keyPush",
                url: "keypush",
                defaults: new { controller = "Home", action = "KeyPush" }
            );

            routes.MapRoute(
                name: "getBoard",
                url: "getboard",
                defaults: new { controller = "Home", action = "GetBoard" }
            );

            routes.MapRoute(
                name: "hostGame",
                url: "hostgame",
                defaults: new { controller = "Home", action = "HostGame" }
            );

            routes.MapRoute(
                name: "getHosts",
                url: "gethosts",
                defaults: new { controller = "Home", action = "GetHosts" }
            );

            routes.MapRoute(
                name: "joinPage",
                url: "joinpage",
                defaults: new { controller = "Home", action = "JoinPage" }
            );

            routes.MapRoute(
                name: "hostPage",
                url: "hostpage",
                defaults: new { controller = "Home", action = "HostPage" }
            );

            routes.MapRoute(
                name: "checkStatus",
                url: "checkstatus",
                defaults: new { controller = "Home", action = "CheckStatus" }
            );

            routes.MapRoute(
                name: "checkStart",
                url: "checkstart",
                defaults: new { controller = "Home", action = "CheckStart" }
            );

            routes.MapRoute(
                name: "joinHost",
                url: "joinhost",
                defaults: new { controller = "Home", action = "JoinHost" }
            );

            routes.MapRoute(
                name: "game",
                url: "game",
                defaults: new { controller = "Home", action = "Game" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
