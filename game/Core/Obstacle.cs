
using RLNET;
using RogueSharp;
using test_roguelike.Interfaces;

namespace test_roguelike.Core
{
    public class Obstacle
    {
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Obstacle(int x, int y, int type)
        {
            X = x;
            Y = y;
            if(type == 1)
            {
                Symbol = (char)3;
            }
            else if(type == 2)
            {
                Symbol = (char)4;
            }
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
