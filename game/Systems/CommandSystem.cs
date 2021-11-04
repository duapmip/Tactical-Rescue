using System;
using System.Collections.Generic;
using System.Text;
using RLNET;
using RogueSharp;
using RogueSharp.DiceNotation;
using test_roguelike.Core;
using test_roguelike.Interfaces;

namespace test_roguelike.Systems
{
    public class CommandSystem 
    {
        public bool IsPlayerTurn { get; set; }


        // Return value is true if the player was able to move
        // false when the player couldn't move, such as trying to move into a wall
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            switch (direction)
            {
                case Direction.Up:
                    {
                        y = Game.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = Game.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = Game.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = Game.Player.X + 1;
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }


            if (Game.DungeonMap.SetActorPosition(Game.Player, x, y))
            {
                return true;
            }

            Monster monster = Game.DungeonMap.GetMonsterAt(x, y);

            if (monster != null)
            {                
                return Shoot(Game.Player, monster);
            }

            return false;
        }

        public bool Grab()
        {
            Player player = Game.Player;
            Weapon weapon = Game.DungeonMap.GetWeaponAt(player.X, player.Y);
            Item item = Game.DungeonMap.GetItemAt(player.X, player.Y);

            if (weapon != null)
            {
                Game.DungeonMap.RemoveWeapon(weapon);
                return weapon.ImprovePlayer();
            }
            else if (item != null)
            {
                Game.DungeonMap.RemoveItem(item);
                return item.GetItem(item);
            }

            return false;
        }

        public bool Heal()
        {
            Player player = Game.Player;
            if (player.inventoryHealth > 0)
            {
                player.Healing();
                player.inventoryHealth--;
                return true;
            }
            return false;
        }

        public bool ImproveAccuracy()
        {
            Player player = Game.Player;
            if (player.inventoryAccuracy > 0)
            {
                player.Accurate();
                player.inventoryAccuracy--;
                return true;
            }
            return false;

        }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        public void ActivateMonsters()
        {
            IScheduleable scheduleable = Game.SchedulingSystem.Get();
            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                Game.SchedulingSystem.Add(Game.Player);
            }
            else
            {
                Monster monster = scheduleable as Monster;

                if (monster != null)
                {
                    monster.PerformAction(this);
                    Game.SchedulingSystem.Add(monster);
                }

                ActivateMonsters();
            }
        }

        public void MoveMonster(Monster monster, Cell cell)
        {
            Game.DungeonMap.SetActorPosition(monster, cell.X, cell.Y);
            DiceExpression attackDice = new DiceExpression().Dice(1, 100);
            DiceResult attackResult = attackDice.Roll();
            if (attackResult.Value >= 100 - 60)
            {
                Shoot(monster, Game.Player);
            }
            

        }

        public bool Shoot(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);

            Game.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                Game.MessageLog.Add(defenseMessage.ToString());
            }

            ResolveDamage(defender, hits);
            return true;
        }

        // The attacker rolls based on his stats to see if he gets any hits
        private int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;
            int accuracy = ChanceToTouch(attacker, defender, attacker.Accuracy);
            attackMessage.AppendFormat("{0} attacks {1}", attacker.Name, defender.Name);

            // Roll a number of 100-sided dice equal to the Attack value of the attacking actor
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Munition, 100);
            DiceResult attackResult = attackDice.Roll();

            // Look at the face value of each single die that was rolled
            foreach (TermResult termResult in attackResult.Results)
            {
                // Compare the value to 100 minus the attack chance and add a hit if it's greater
                if (termResult.Value >= 100-accuracy)
                {
                    hits = hits + attacker.Degat;
                }
            }

            return hits;
        }


        public int ChanceToTouch(Actor attacker, Actor defender, double precison)
        {
            double distance = Math.Sqrt(Math.Pow((defender.X - attacker.X),2) + Math.Pow((defender.Y - attacker.Y),2));
            double distanceMax = 25;
            int obstacle = 0;
            int x = defender.X;
            int y = defender.Y;
            if (attacker.X < defender.X && attacker.Y < defender.Y)
            { 
                if (!Game.DungeonMap.GetCell(x - 1, y).IsWalkable)
                {
                    obstacle ++;
                }
                if (!Game.DungeonMap.GetCell(x, y-1).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X < defender.X && attacker.Y > defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x - 1, y).IsWalkable)
                {
                    obstacle++;
                }
                if (!Game.DungeonMap.GetCell(x, y + 1).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X > defender.X && attacker.Y > defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x + 1, y).IsWalkable)
                {
                    obstacle++;
                }
                if (!Game.DungeonMap.GetCell(x, y + 1).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X > defender.X && attacker.Y < defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x + 1, y).IsWalkable)
                {
                    obstacle++;
                }
                if (!Game.DungeonMap.GetCell(x, y - 1).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X == defender.X && attacker.Y > defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x , y + 1).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X == defender.X && attacker.Y < defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x, y - 1).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X < defender.X && attacker.Y == defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x - 1, y).IsWalkable)
                {
                    obstacle++;
                }
            }
            else if (attacker.X > defender.X && attacker.Y == defender.Y)
            {
                if (!Game.DungeonMap.GetCell(x + 1, y).IsWalkable)
                {
                    obstacle++;
                }
            }
            double probability = 100 * (1 - Math.Pow((distance / distanceMax), 2)) * (1 - precison / 100);

            if (obstacle == 2)
            {
                return 0;
            }
            else if (obstacle == 1)
            {
                return (int)(probability - 25);
            }
            return (int)probability;
        }

        // Apply any damage that wasn't blocked to the defender
        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                Game.MessageLog.Add($"  {defender.Name} was hit for {damage} damage");

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.Add($"  {defender.Name} dodge all damage");
            }
        }

        // Remove the defender from the map and add some messages upon death.
        private static void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.MessageLog.Add($"  {defender.Name} was killed, GAME OVER MAN!");
                DeathScreen deathScreen = new DeathScreen();
                deathScreen.Show();
            }
            else if (defender is Monster)
            {
                Game.DungeonMap.RemoveMonster((Monster)defender);



                List<Monster> monsters = Game.DungeonMap._monsters;
                List<Monster> monstersInFov = new List<Monster>();
                foreach (Monster monster in monsters)
                {
                    // When the monster is in the field-of-view
                    if (Game.DungeonMap.IsInFov(monster.X, monster.Y))
                    {
                        monstersInFov.Add(monster);
                    }
                }
                if (monstersInFov.Count != 0)
                {
                    int count = 0;
                    while (count < monstersInFov.Count - 1 && defender != monstersInFov[count])
                    {
                        count++;
                    }
                    if (count < monstersInFov.Count - 1)
                    {
                        count++;
                        defender = monstersInFov[count];
                    }
                    else
                    {
                        count = 0;
                        defender = monstersInFov[count];
                    }
                    defender.Color = Colors.KoboldColorSelect;
                }
                Game.MessageLog.Add($"  {defender.Name} died");
            }
        }
    }
}