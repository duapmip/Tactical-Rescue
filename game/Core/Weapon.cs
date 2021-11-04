using System;
using RLNET;
using RogueSharp;

namespace test_roguelike.Core
{
    public class Weapon
    {
        public int X;
        public int Y;
        public string nomArme;
        public double accuracy;
        public int munition;
        public int degat;
        public char symbol;


        public bool ImprovePlayer()
        {
            Player player = Game.Player;
            player.Munition= player.Munition + munition;
            player.Accuracy = player.Accuracy + accuracy;
            player.Degat = player.Degat + degat;
            player.weaponLevel++;
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
