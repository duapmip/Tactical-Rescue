using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_roguelike.Core
{
    class Sniper : Weapon
    {
        public Sniper(int x, int y)
        {
            X = x;
            Y = y;
            base.nomArme = "Sniper";
            base.accuracy = 3;
            base.munition = 1;
            base.symbol = (char)5;
        }
    }
}
