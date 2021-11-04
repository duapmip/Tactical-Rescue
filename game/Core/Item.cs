using System;
using RLNET;
using RogueSharp;

namespace test_roguelike.Core
{
    public class Item
    {
        public int quantityHealth = 10;
        public int quantityAccuracy = 5;
        public int X;
        public int Y;
        public char symbol;

        public bool GetItem(Item item)
        {
            Player player = Game.Player;
            if (item is HealthUp)
            {
                player.inventoryHealth++;
            }
            else
            {
                player.inventoryAccuracy++;
            }            
            return true;
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
                console.Set(X, Y, null, null, symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, symbol);
            }
        }
    }
}
