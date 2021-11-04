using System;
using System.Collections.Generic;
using System.Linq;
using RLNET;
using RogueSharp;

namespace test_roguelike.Core
{
    public class House : Rectangle
    {        
        public House(int xMin, int yMin, int lengthSide)
        {
            base.X = xMin;
            base.Y = yMin;
            base.Width = lengthSide;
            base.Height = lengthSide;
        }

        public void Draw(RLConsole console, IMap map)
        {
            int distance = (base.Width / 2) + 1;
            List<Cell> borderCells = map.GetBorderCellsInArea(base.Center.X, base.Center.Y, distance).ToList();

            foreach(Cell cell in borderCells)
            {
                if (!cell.IsExplored)
                {
                    return;
                }

                // When a cell is currently in the field-of-view it should be drawn with ligher colors

                else if (map.IsInFov(cell.X, cell.Y))
                {
                    // Choose the symbol to draw based on if the cell is walkable or not, transparent or not
                    // '.' for floor, '#' for walls and 'o' for obstacles
                    if (cell.IsWalkable)
                    {
                        console.Set(cell.X, cell.Y, null, null, (char)178);
                    }
                    else if (!cell.IsTransparent)
                    {
                        console.Set(cell.X, cell.Y, null, Swatch.DbGrass, '#');
                    }
                }
                else
                {
                    if (cell.IsWalkable)
                    {
                        console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, (char)178);
                    }
                    else if (!cell.IsTransparent)
                    {
                        console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
                    }
                }
            }
        }
    }
}
