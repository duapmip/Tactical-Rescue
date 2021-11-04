

namespace test_roguelike.Core
{
    class SubmachineGun : Weapon
    {
        public SubmachineGun(int x, int y)
        {
            X = x;
            Y = y;
            base.nomArme = "Submachine Gun";
            base.accuracy = 0.5;
            base.munition = 3;
            base.symbol = (char)4;
        }
    }
}
