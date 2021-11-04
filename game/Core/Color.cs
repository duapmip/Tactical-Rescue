using RLNET;

namespace test_roguelike.Core
{
    public class Colors
    {
        public static RLColor FloorBackground = RLColor.Black;
        public static RLColor Floor = Swatch.AlternateDarkest;
        public static RLColor FloorBackgroundFov = Swatch.DbDark;
        public static RLColor FloorFov = Swatch.Alternate;

        public static RLColor WallBackground = Swatch.SecondaryDarkest;
        public static RLColor Wall = Swatch.Secondary;
        public static RLColor WallBackgroundFov = Swatch.SecondaryDarker;
        public static RLColor WallFov = Swatch.SecondaryLighter;

        public static RLColor ObstacleBackground = Swatch.SecondaryDarkest;
        public static RLColor Obstacle = Swatch.Secondary;
        public static RLColor ObstacleBackgroundFov = Swatch.SecondaryDarker;
        public static RLColor ObstacleFov = Swatch.SecondaryLighter;

        public static RLColor Player = Swatch.DbLight;

        public static RLColor TextHeading = RLColor.White;
        public static RLColor Text = Swatch.DbLight;
        public static RLColor Gold = Swatch.DbSun;


        public static RLColor KoboldColor = Swatch.DbWood;
        public static RLColor KoboldColorSelect = Swatch.DbLight;

        public static RLColor DoorBackground = RLColor.Black;
        public static RLColor Door = Swatch.AlternateDarkest;
        public static RLColor DoorBackgroundFov = Swatch.DbDark;
        public static RLColor DoorFov = Swatch.Alternate;
    }
}