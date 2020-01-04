using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleSnake.Models
{
    public class LobbyModel
    {
        public String Host { get; set; }
        public String Member { get; set; }

        public Boolean Ready { get; set; }

        public SnakeModel HostSnake { get; set; }
        public SnakeModel MemberSnake { get; set; }

        public SnakeModel Fruit { get; set; }

        public int HostSize { get; set; }

        public int MemSize { get; set; }

        public List<SnakeModel> MemBody { get; set; }
        public List<SnakeModel> HostBody { get; set; }

        public string[,] Board { get; set; }
        public List<SnakeModel> HostWalls { get; internal set; }
        public List<SnakeModel> MemWalls { get; internal set; }

        public string lost { get; set; }
    }
}