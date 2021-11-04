using System;
using System.Linq;
using System.Collections.Generic;
using RogueSharp;
using RLNET;
using RogueSharp.DiceNotation;
using test_roguelike.Core;
using test_roguelike.Monsters;

namespace test_roguelike.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;

        private readonly DungeonMap _map;

        // Constructing a new MapGenerator requires the dimensions of the maps it will create
        // as well as the sizes and maximum number of rooms
        public MapGenerator(int width, int height, int maxRooms)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _map = new DungeonMap();
        }

        // Generate a new map that places rooms randomly
        public DungeonMap CreateMap()
        {
            // Set the properties of all cells to false
            _map.Initialize(_width, _height);

            // Try to place as many rooms as the specified maxRooms
            // Note: Only using decrementing loop because of WordPress formatting
            for (int r = _maxRooms; r > 0; r--)
            {
                // Determine the size and position of the room randomly
                int roomWidth = _width - 1;
                int roomHeight = _height - 1;
                int roomXPosition = 0;
                int roomYPosition = 0;

                // All of our rooms can be represented as Rectangles
                var newRoom = new Rectangle(roomXPosition, roomYPosition,
                  roomWidth, roomHeight);

                // Check to see if the room rectangle intersects with any other rooms
                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                // As long as it doesn't intersect add it to the list of rooms
                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }
            // Iterate through each room that we wanted placed
            // and dig out the room and create doors for it.
            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }
            PlacePlayer();
            PlaceEndZones(Game.Player.checkpoint);
            PlaceObstacles();
            PlaceMonsters();
            PlaceWeapons();
            PlaceItems();

            

            return _map;
        }


        // Find The left side of the map
        private void PlacePlayer()
        {
            Player player = Game.Player;
            if (player == null)
            {
                player = new Player();
            }

            player.X = _map.Rooms[0].Left + 1;
            player.Y = _map.Rooms[0].Center.Y;

            _map.AddPlayer(player);
        }

        private void PlaceMonsters()
        {
            foreach (var room in _map.Rooms)
            {
                // Generate between 30 and 40 monsters
                Random rand = new Random();
                var numberOfMonsters = rand.Next(30, 41);

                for (int i = 0; i < numberOfMonsters; i++)
                {
                    // Find a random walkable location in the room to place the monster
                    Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                    // It's possible that the room doesn't have space to place a monster
                    // In that case skip creating the monster
                    if (randomRoomLocation != null)
                    {
                        // Temporarily hard code this monster to be created at level 1
                        var monster = Kobold.Create(1);
                        monster.X = randomRoomLocation.X;
                        monster.Y = randomRoomLocation.Y;
                        _map.AddMonster(monster);
                    }
                }
            }
        }

        private void PlaceObstacles()
        {
            foreach (var room in _map.Rooms)
            {
                // Generate between 70 and 80 obstacles
                Random rand = new Random();
                int numberOfObstaclesGroup = rand.Next(70, 81);

                for (int i = 0; i < numberOfObstaclesGroup; i++)
                {
                    // Find a random walkable location in the room to place the monster
                    Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);

                    // It's possible that the room doesn't have space to place an obsatcle
                    // In that case skip creating the obstacle
                    // We also want a minimum of a 2-group by obsatcle so we check if space is available around
                    int X = randomRoomLocation.X;
                    int Y = randomRoomLocation.Y;

                    int typeObstacle = rand.Next(2);

                    if (randomRoomLocation != null && _map.IsSpaceAround(X, Y))
                    {
                        int numberOfObstacles = rand.Next(2, 11);
                        bool enoughSpace = true;
                        for (int j = 0; enoughSpace && j < numberOfObstacles; j++)
                        {
                            switch (typeObstacle)
                            {
                                case 0:
                                    _map.AddObstacle(new Obstacle(X, Y, 1));
                                    break;
                                case 1:
                                    _map.AddObstacle(new Obstacle(X, Y, 2));
                                    break;
                            }

                            if (_map.IsSpaceAround(X, Y))
                            {
                                bool nextLocationIsOk = false;
                                while (!nextLocationIsOk)
                                {
                                    int nextLocation = rand.Next(1, 5); //1 is up, 2 is right, 3 is bottom, 4 is left
                                    switch (nextLocation)
                                    {
                                        case 1:
                                            if (_map.IsWalkable(X, Y - 1))
                                            {
                                                nextLocationIsOk = true;
                                                Y = Y - 1;
                                            }
                                            break;
                                        case 2:
                                            if (_map.IsWalkable(X + 1, Y))
                                            {
                                                nextLocationIsOk = true;
                                                X = X + 1;
                                            }
                                            break;
                                        case 3:
                                            if (_map.IsWalkable(X, Y + 1))
                                            {
                                                nextLocationIsOk = true;
                                                Y = Y + 1;
                                            }
                                            break;
                                        case 4:
                                            if (_map.IsWalkable(X - 1, Y))
                                            {
                                                nextLocationIsOk = true;
                                                X = X - 1;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                enoughSpace = false; //there is no more space around to place more obstacles
                            }
                        }
                    }
                }
            }
        }

        private void PlaceWeapons()
        {
            foreach (var room in _map.Rooms)
            {
                // Generate between 40 and 50 obstacles
                Random rand = new Random();
                int numberOfWeapons = rand.Next(5, 10);

                for (int i = 0; i < numberOfWeapons; i++)
                {
                    // Find a random walkable location in the room to place the weapon
                    Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                    // It's possible that the room doesn't have space to place a weapon
                    // In that case skip creating the weapon
                    if (randomRoomLocation != null)
                    {
                        int typeWeapon = rand.Next(2);
                        switch (typeWeapon)
                        {
                            case 0:
                                _map.AddWeapon(new Sniper(randomRoomLocation.X, randomRoomLocation.Y));
                                break;
                            case 1:
                                _map.AddWeapon(new SubmachineGun(randomRoomLocation.X, randomRoomLocation.Y));
                                break;
                        }
                    }
                }
            }
        }

        private void PlaceItems()
        {
            foreach (var room in _map.Rooms)
            {
                // Generate between 40 and 50 obstacles
                Random rand = new Random();
                int numberOfItems = rand.Next(5, 10);

                for (int i = 0; i < numberOfItems; i++)
                {
                    // Find a random walkable location in the room to place the item
                    Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                    // It's possible that the room doesn't have space to place an item
                    // In that case skip creating the item
                    if (randomRoomLocation != null)
                    {
                        int typeItem = rand.Next(10);
                        if (typeItem < 7)
                        {
                            _map.AddItem(new HealthUp(randomRoomLocation.X, randomRoomLocation.Y));
                        }
                        else
                        {
                            _map.AddItem(new AccuracyUp(randomRoomLocation.X, randomRoomLocation.Y));
                        }
                    }
                }
            }
        }

        public void PlaceEndZones(int checkpoint)
        {
            foreach (var room in _map.Rooms)
            {
                Random rand = new Random();
                int x, y;
                if (checkpoint < 2)
                {
                    x = rand.Next(room.Right - 10, room.Right); //right side of the map
                    do
                    {
                        y = rand.Next(2, room.Bottom - 2); 
                    } while (!_map.IsWalkable(x, y) || _map.GetWeaponAt(x, y) != null || _map.GetItemAt(x, y) != null);
                    _map.AddEndZone(new EndZone(x, y, false));
                }
                else if (checkpoint == 2)
                {
                    x = rand.Next(1, room.Center.X - 20);
                    do
                    {
                        y = rand.Next(2, room.Bottom - 2);
                    } while (!_map.IsWalkable(x, y) || _map.GetWeaponAt(x, y) != null || _map.GetItemAt(x, y) != null);
                    _map.AddEndZone(new EndZone(x, y, true));
                }

            }
        }


        // Carve a tunnel out of the map parallel to the x-axis
        //private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        //{
        //    for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
        //    {
        //        _map.SetCellProperties(x, yPosition, true, true);
        //    }
        //}

        // Carve a tunnel out of the map parallel to the y-axis
        //private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        //{
        //    for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
        //    {
        //        _map.SetCellProperties(xPosition, y, true, true);
        //    }
        //}

        // Given a rectangular area on the map
        // set the cell properties for that area to true
        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, false);
                }
            }
        }
        private void CreateDoors(Rectangle room)
        {
            // The boundries of the room
            int xMin = room.Left;
            int xMax = room.Right;
            int yMin = room.Top;
            int yMax = room.Bottom;

            // Put the rooms border cells into a list
            List<Cell> borderCells = _map.GetCellsAlongLine(xMin, yMin, xMax, yMin).ToList();
            borderCells.AddRange(_map.GetCellsAlongLine(xMin, yMin, xMin, yMax));
            borderCells.AddRange(_map.GetCellsAlongLine(xMin, yMax, xMax, yMax));
            borderCells.AddRange(_map.GetCellsAlongLine(xMax, yMin, xMax, yMax));


            // Give a 50/50 chance of which 'L' shaped connecting hallway to tunnel out
            //if (Game.Random.Next(1, 2) == 1)
            //{
            //    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
            //    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
            //}
            //else
            //{
            //    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
            //    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
            //}

            // Go through each of the rooms border cells and look for locations to place doors.
            foreach (Cell cell in borderCells)
            {
                if (IsPotentialDoor(cell))
                {
                    // A door must block field-of-view when it is closed.
                    _map.SetCellProperties(cell.X, cell.Y, false, true);
                    _map.Doors.Add(new Door
                    {
                        X = cell.X,
                        Y = cell.Y,
                        IsOpen = false
                    });
                }
            }
        }
        public void CreateStairs()
        {
            Random rnd = new Random();
            int randomLocationX = rnd.Next(1, _map.Width-1);
            int randomLocationY = rnd.Next(1, _map.Height-1);
            _map.StairsDown = new Stairs
            {                
                X = randomLocationX,
                Y = randomLocationY,
                IsUp = false
            };

        }

        // Checks to see if a cell is a good candidate for placement of a door
        private bool IsPotentialDoor(Cell cell)
        {
            // If the cell is not walkable
            // then it is a wall and not a good place for a door
            if (!cell.IsWalkable)
            {
                return false;
            }

            // Store references to all of the neighboring cells 
            Cell right = _map.GetCell(cell.X + 1, cell.Y);
            Cell left = _map.GetCell(cell.X - 1, cell.Y);
            Cell top = _map.GetCell(cell.X, cell.Y - 1);
            Cell bottom = _map.GetCell(cell.X, cell.Y + 1);

            // Make sure there is not already a door here
            if (_map.GetDoor(cell.X, cell.Y) != null ||
                 _map.GetDoor(right.X, right.Y) != null ||
                 _map.GetDoor(left.X, left.Y) != null ||
                 _map.GetDoor(top.X, top.Y) != null ||
                 _map.GetDoor(bottom.X, bottom.Y) != null)
            {
                return false;
            }

            // This is a good place for a door on the left or right side of the room
            if (right.IsWalkable && left.IsWalkable && !top.IsWalkable && !bottom.IsWalkable)
            {
                return true;
            }

            // This is a good place for a door on the top or bottom of the room
            if (!right.IsWalkable && !left.IsWalkable && top.IsWalkable && bottom.IsWalkable)
            {
                return true;
            }
            return false;
        }
    }
}
