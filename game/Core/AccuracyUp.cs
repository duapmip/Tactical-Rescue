using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_roguelike.Core
{
    class AccuracyUp : Item
    {
        public AccuracyUp(int x, int y)
        {
            X = x;
            Y = y;
            base.symbol = (char)25;
        }
    }
}
