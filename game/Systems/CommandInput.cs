using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;


namespace test_roguelike.Systems
{
    public class CommandInput
    {
        public CommandInput() { }
        public void Draw(RLConsole console)
        {
            console.Print(1, 5, "Up : ", RLColor.White);
            console.Set(10, 5, RLColor.White, null, (char)28);
            console.Print(1, 7, "Down : ", RLColor.White);
            console.Set(10, 7, RLColor.White, null, (char)29);
            console.Print(1, 9, "Right : ", RLColor.White);
            console.Set(10, 9, RLColor.White, null, (char)26);
            console.Print(1, 11, "Left : ", RLColor.White);
            console.Set(10, 11, RLColor.White, null, (char)27);

            console.Print(15, 5, "Shoot : ", RLColor.White);
            console.Print(35, 5, "S", RLColor.White);
            console.Print(15, 7, "Change target : ", RLColor.White);
            console.Print(35, 7, "Space", RLColor.White);
            console.Print(15, 9, "Pick item/weapon : ", RLColor.White);
            console.Print(35, 9, "G", RLColor.White);
            console.Print(15, 11, "Change level : ", RLColor.White);
            console.Print(35, 11, "U", RLColor.White);

            console.Print(45, 5, "Health : ", RLColor.White);
            console.Print(65, 5, "H", RLColor.White);
            console.Print(45, 7, "Upgrade accuracy: ", RLColor.White);
            console.Print(65, 7, "F", RLColor.White);
            console.Print(45, 9, "Upgrade health : ", RLColor.White);
            console.Print(65, 9, "H", RLColor.White);
        }
    }
}
