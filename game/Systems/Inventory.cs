using RLNET;
using test_roguelike.Core;

namespace test_roguelike.Systems
{    public class Inventory
    {
        public Inventory()
        {

        }
        public void Draw(RLConsole console)
        {
            Player player = Game.Player;
            console.Print(7, 5, "Health inventory : " + player.inventoryHealth, RLColor.White);
            console.Set(1, 5, RLColor.White, null, (char)19);
            console.Set(3, 5, RLColor.White, null, (char)20);
            console.Set(5, 5, RLColor.White, null, (char)21);
            console.Set(1, 8, RLColor.White, null, (char)25);
            console.Print(3, 8, "Accuracy inventory : " + player.inventoryAccuracy, RLColor.White);
            console.Set(1, 11, RLColor.White, null, (char)24);
            console.Print(3, 11, "Weapon level : " + player.weaponLevel, RLColor.White);
        }
    }
}
