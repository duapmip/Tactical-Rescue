using RogueSharp;
using RLNET;
using test_roguelike.Interfaces;

namespace test_roguelike.Core
{
    public class Door : IDrawable
    {
        public Door()
        {
            Symbol = (char)9;
            Color = Colors.Door;
            BackgroundColor = Colors.DoorBackground;
        }
        public bool IsOpen { get; set; }

        public RLColor Color { get; set; }
        public RLColor BackgroundColor { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole console, IMap map)
        {
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            Symbol = IsOpen ? (char)10 : (char)9;
            if (map.IsInFov(X, Y))
            {
                Color = Colors.DoorFov;
                BackgroundColor = Colors.DoorBackgroundFov;
            }
            else
            {
                Color = Colors.Door;
                BackgroundColor = Colors.DoorBackground;
            }

            console.Set(X, Y, Color, null, Symbol);
        }
    }
}
