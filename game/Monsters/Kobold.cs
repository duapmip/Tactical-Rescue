using RogueSharp.DiceNotation;
using test_roguelike.Core;

namespace test_roguelike.Monsters
{
    public class Kobold : Monster
    {
        public static Kobold Create(int level)
        {
            int health = Dice.Roll("2D5");
            return new Kobold
            {
                Munition = Dice.Roll("1D3") + level / 3,
                Awareness = 10,
                Color = Colors.KoboldColor,
                Degat = Dice.Roll("1D3") + level / 3,
                Accuracy = level/3,
                Health = health,
                MaxHealth = health,
                Name = "Kobold",
                Speed = 14,
                Symbol = (char)7
            };
        }
    }
}
