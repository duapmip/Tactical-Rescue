using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueSharp;

namespace test_roguelike.Core
{
    public class EndZone
    {
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public EndZone(int x, int y, bool end)
        {
            X = x;
            Y = y;
            if (!end) { Symbol = (char)1; }
            else { Symbol = (char)1; }

        }

        public void Draw(RLConsole console, IMap map)
        {
            // Don't draw actors in cells that haven't been explored
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            // Only draw the actor with the color and symbol when they are in field-of-view
            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, null, null, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, Symbol);
            }
        }
    }
}
