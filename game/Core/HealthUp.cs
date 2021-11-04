using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_roguelike.Core
{
    class HealthUp : Item
    {
        public HealthUp (int x, int y)
        {
            X = x;
            Y = y;
            Random rand = new Random();
            int skinHealthUp = rand.Next(3);
            switch(skinHealthUp)
            {
                case 0:
                    base.symbol = (char)16;
                    break;

                case 1:
                    base.symbol = (char)17;
                    break;

                case 2:
                    base.symbol = (char)18;
                    break;
            }
        }
    }
}
