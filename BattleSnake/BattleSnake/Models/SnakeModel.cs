using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleSnake.Models
{
    public class SnakeModel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Compare(SnakeModel other)
        {
            if (this.X == other.X && this.Y == other.Y)
            {
                return true;
            }
            return false;
        }
    }
}