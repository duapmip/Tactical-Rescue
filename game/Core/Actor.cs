using RLNET;
using RogueSharp;
using test_roguelike.Interfaces;

namespace test_roguelike.Core
{
    public class Actor : IActor, IDrawable, IScheduleable
    {
        // IActor
        private int _munition;
        private int _awareness;
        private int _degat;
        private double _accuracy;
        private int _health;
        private int _maxHealth;
        private string _name;
        private int _speed;

        public int Munition
        {
            get
            {
                return _munition;
            }
            set
            {
                _munition = value;
            }
        }


        public int Awareness
        {
            get
            {
                return _awareness;
            }
            set
            {
                _awareness = value;
            }
        }

        public int Degat
        {
            get
            {
                return _degat;
            }
            set
            {
                _degat = value;
            }
        }


        public double Accuracy
        {
            get
            {
                return _accuracy;
            }
            set
            {
                _accuracy = value;
            }
        }

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        // IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
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
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                // When not in field-of-view just draw a normal floor
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, (char)10);
            }
        }
        // IScheduleable
        public int Time
        {
            get
            {
                return Speed;
            }
        }
    }
}
