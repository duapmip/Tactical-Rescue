using System;
using System.Collections.Generic;
using RLNET;
using RogueSharp.Random;
using test_roguelike.Core;
using test_roguelike.Systems;

namespace test_roguelike
{
    public static class Game
    {
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 140;
        private static readonly int _screenHeight = 60;
        public static RLRootConsole _rootConsole;

        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 100;
        private static readonly int _mapHeight = 45;
        private static RLConsole _mapConsole;

        // The message console is to the bottom right of the map and displays attack rolls and other information
        private static readonly int _messageWidth = 60;
        private static readonly int _messageHeight = 15;
        private static RLConsole _messageConsole;

        // The stat console is to the top right of the map and display player and monster stats
        private static readonly int _statWidth = 60;
        private static readonly int _statHeight = 15;
        private static RLConsole _statConsole;

        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 20;
        private static readonly int _inventoryHeight = 15;
        private static RLConsole _inventoryConsole;

        // Above the map on the left is the command console which shows the different keys to play
        private static readonly int _commandWidth = 40;
        private static readonly int _commandHeight = 15;
        private static RLConsole _commandConsole;

        public static Player Player { get; set; }
        public static DungeonMap DungeonMap { get; private set; }

        private static bool _renderRequired = true;
        public static CommandSystem CommandSystem { get; private set; }

        public static MessageLog MessageLog { get; private set; }

        public static SchedulingSystem SchedulingSystem { get; private set; }

        public static CommandInput CommandInput { get; private set; }

        public static Inventory Inventory { get; private set; }

        private static int _mapLevel = 1;

        public static Monster monsterchoose;

        private static MapGenerator mapGenerator;



        // Singleton of IRandom used throughout the game when generating random numbers
        public static IRandom Random { get; private set; }


        public static void Main()
        {
            // This must be the exact name of the bitmap font file we are using or it will error.
            string fontFileName = "terminal16x16.png";

            // Establish the seed for the random number generator from the current time
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            System.Media.SoundPlayer sound = new System.Media.SoundPlayer();
            sound.SoundLocation = "test.wav";
            sound.Play();

            // The title will appear at the top of the console window 
            // also include the seed used to generate the level
            string consoleTitle = $"Tactical  - Level {_mapLevel} - Seed {seed}";

            // Tell RLNet to use the bitmap font that we specified and that each tile is 8 x 8 pixels
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 16, 16, 0.5f, consoleTitle);

            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);
            _commandConsole = new RLConsole(_commandWidth, _commandHeight);

            SchedulingSystem = new SchedulingSystem();

            // The next two lines already existed
            mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 1);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.UpdatePlayerFieldOfView();
            CommandSystem = new CommandSystem();
            CommandInput = new CommandInput();
            Inventory = new Inventory();

            // Set up a handler for RLNET's Update event
            _rootConsole.Update += OnRootConsoleUpdate;

            // Set up a handler for RLNET's Render event
            _rootConsole.Render += OnRootConsoleRender;

            // Create a new MessageLog and print the random seed used to generate the level
            MessageLog = new MessageLog();
            MessageLog.Add("G.I. Joe arrives on level " + _mapLevel);

            BeginningScreen begin = new BeginningScreen();
            begin.Show();

            // Begin RLNET's game loop
            _rootConsole.Run();
        }
        // Event handler for RLNET's Update event
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();


            if (CommandSystem.IsPlayerTurn)
            {
                List<Monster> monsters = Game.DungeonMap._monsters;
                List<Monster> monstersInFov = new List<Monster>();
                Monster monsterChoose = null;
                bool selectionMonster = false;
                foreach (Monster monster in monsters)
                {
                    // When the monster is in the field-of-view
                    if (Game.DungeonMap.IsInFov(monster.X, monster.Y))
                    {
                        monstersInFov.Add(monster);
                    }
                }
                foreach (Monster monster in monstersInFov)
                {
                    if (monster.Color.Equals(Colors.KoboldColorSelect))
                    {
                        monsterChoose = monster;
                        selectionMonster = true;
                    }                   
                }
                if (!selectionMonster && monstersInFov.Count>0)
                {
                    monsterChoose = monstersInFov[0];
                }
                if (monstersInFov.Count>0)
                {

                    monsterChoose.Color = Colors.KoboldColorSelect;
                }
                
                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.Up)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                    }
                    else if (keyPress.Key == RLKey.Down)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                    }
                    else if (keyPress.Key == RLKey.Left)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                    }
                    else if (keyPress.Key == RLKey.Right)
                    {
                        didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                    }
                    else if (keyPress.Key == RLKey.S)
                    {
                        if (monstersInFov.Count > 0)
                        {
                            didPlayerAct = CommandSystem.Shoot(Player, monsterChoose);
                            monsterChoose.Color = Colors.KoboldColorSelect;
                        }
                    }
                    else if (keyPress.Key == RLKey.Space)
                    {
                        if (monstersInFov.Count > 0)
                        {
                            int count = 0;
                            while (count < monstersInFov.Count - 1 && monsterChoose != monstersInFov[count])
                            {
                                count++;
                            }
                            monsterChoose.Color = Colors.KoboldColor;
                            if (count < monstersInFov.Count - 1)
                            {
                                count++;
                                monsterChoose = monstersInFov[count];
                            }
                            else
                            {
                                count = 0;
                                monsterChoose = monstersInFov[count];
                            }
                            monsterChoose.Color = Colors.KoboldColorSelect;
                        }
                    }
                    else if (keyPress.Key == RLKey.U)
                    {
                        if (DungeonMap.CanMoveDownToNextLevel())
                        {
                            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight, 1);
                            DungeonMap = mapGenerator.CreateMap();
                            MessageLog = new MessageLog();
                            CommandSystem = new CommandSystem();
                            _mapLevel++;
                            _rootConsole.Title = $"RougeSharp RLNet Tutorial - Level {_mapLevel}";
                            didPlayerAct = true;
                        }
                    }
                    else if (keyPress.Key == RLKey.G)
                    {
                        didPlayerAct = CommandSystem.Grab();
                    }
                    else if (keyPress.Key == RLKey.H)
                    {
                        didPlayerAct = CommandSystem.Heal();
                    }
                    else if (keyPress.Key == RLKey.F)
                    {
                        didPlayerAct = CommandSystem.ImproveAccuracy();
                    }
                    else if (keyPress.Key == RLKey.Escape)
                    {
                        _rootConsole.Close();
                    }
                }
                if(DungeonMap._endZones.Count != 0 && Player.X == DungeonMap._endZones[0].X && Player.Y == DungeonMap._endZones[0].Y)
                {
                    Player.checkpoint++;
                    if(Player.checkpoint < 2)
                    {
                        Checkpoint information = new Checkpoint();
                        information.Show();
                        DungeonMap._endZones.RemoveAt(0);
                        mapGenerator.CreateStairs();
                    }
                    else if(Player.checkpoint == 2)
                    {
                        Checkpoint2 codex = new Checkpoint2();
                        codex.Show();
                        DungeonMap._endZones.RemoveAt(0);
                        mapGenerator.PlaceEndZones(Player.checkpoint);
                    }
                }
                foreach(Monster monster in monsters)
                {
                    if (!Game.DungeonMap.IsInFov(monster.X, monster.Y))
                    {
                        monster.Color = Colors.KoboldColor;
                    }
                }
                
                if (didPlayerAct)
                {
                    _renderRequired = true;
                    CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                CommandSystem.ActivateMonsters();
                _renderRequired = true;
            }
        }

        // Event handler for RLNET's Render event
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            // Don't bother redrawing all of the consoles if nothing has changed.
            if (_renderRequired)
            {
                _mapConsole.Clear();
                _statConsole.Clear();
                _messageConsole.Clear();

                _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.Alternate);
                _messageConsole.Print(1, 1, "MESSAGE", Colors.TextHeading);

                _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood);
                _inventoryConsole.Print(1, 1, "INVENTORY", Colors.TextHeading);

                _statConsole.SetBackColor(0, 0, _statWidth, _statHeight, Swatch.DbMetal);
                _statConsole.Print(1, 1, "STATS", Colors.TextHeading);

                _commandConsole.SetBackColor(0, 0, _commandWidth, _commandHeight, Swatch.Secondary);
                _commandConsole.Print(1, 1, "INPUTS", Colors.TextHeading);


                DungeonMap.Draw(_mapConsole, _statConsole);
                Player.Draw(_mapConsole, DungeonMap);
                Player.DrawStats(_statConsole);
                MessageLog.Draw(_messageConsole);
                CommandInput.Draw(_commandConsole);
                Inventory.Draw(_inventoryConsole);

                // Blit the sub consoles to the root console in the correct locations
                RLConsole.Blit(_mapConsole, 50, 50, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, _mapWidth, _statHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, _commandWidth, 0);
                RLConsole.Blit(_commandConsole, 0, 0, _commandWidth, _commandHeight, _rootConsole, 0, 0);

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();

                _renderRequired = false;
            }
        }
    }
}
