using RLNET;
using System.Collections.Generic;

namespace test_roguelike.Core
{
    public class Player : Actor
    {
        public int inventoryHealth;
        public int inventoryAccuracy;
        public int weaponLevel;
        public int checkpoint;

        public Player()
        {
            Munition = 1;
            Awareness = 20;
            Color = Colors.Player;
            Degat = 1;
            Accuracy = 0;
            Health = 100;
            MaxHealth = 100;
            Name = "G.I. Joe";
            Speed = 10;
            Symbol = (char)2;
            inventoryHealth = 0;
            inventoryAccuracy = 0;
            weaponLevel = 0;
            checkpoint = 0;
        }

        public void Healing()
        {            
            Player player = Game.Player;
            Item item = new Item();
            if (player.Health + item.quantityHealth <= 100)
            {
                player.Health = player.Health + item.quantityHealth;
            }
            else
            {
                player.Health = 100;
            }
        }

        public void Accurate()
        {
            Player player = Game.Player;
            Item item = new Item();
            if (player.Accuracy + item.quantityAccuracy <= 30)
            {
                player.Accuracy = player.Accuracy + item.quantityAccuracy;
            }
            else
            {
                player.Accuracy = 30;
            }
        }

        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 3, $"Name:    {Name}", Colors.Text);
            statConsole.Print(1, 5, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 7, $"Accuracy:  {Accuracy}", Colors.Text);
        }
    }
}