using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewGame
{
    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    };

    public class World
    {
        public int Height = 27;
        public int Width = 27;
        public int Speed { get; set; }
        public int Score { get; set; }
        public Direction MoveDirection { get; set; }
        public bool GameOver { get; set; }
        public List<RectPoint> Snake = new List<RectPoint>();
        public RectPoint Food = new RectPoint();
        public int Level { get; set; }
        public int BlocksCount { get; set; }
        public List<Wall> Walls = new List<Wall>();

        public World()
        {
            BlocksCount = 1;
            Level = 1;
            Speed = 12;
            Score = 0;
            GameOver = false;
            MoveDirection = Direction.Down;
            Walls.Add(new Wall());
        }
        public void GenerateFood()
        {
            var newFood = TryToGenerateFood();
            while (IsFoodOnSnake(newFood) || IsFoodOnBlocks(newFood))
            {
                newFood = TryToGenerateFood();
            }
            Food = newFood;
        }

        private bool IsFoodOnBlocks(RectPoint newFood)
        {
            for (var x = 0; x < Walls.Count; x++)
                for (var y = 0; y < Walls[x].Blocks.Length; y++)
                    if (Walls[x].Blocks[y].X == newFood.X && Walls[x].Blocks[y].Y == newFood.Y)
                        return true;
            return false;
        }

        private bool IsFoodOnSnake(RectPoint newFood)
        {
            for (var i = 0; i < Snake.Count; i++)
                if (Snake[i].X == newFood.X && Snake[i].Y == newFood.Y)
                    return true;
            return false;
        }

        private RectPoint TryToGenerateFood()
        {
            var newFood = new RectPoint();
            int maxXPos = Width;
            int maxYPos = Height;
            Random random = new Random();
            newFood.X = random.Next(0, maxXPos);
            newFood.Y = random.Next(0, maxYPos);
            return newFood;
        }

        public void GenerateBlocks()
        {
            for (var i = 0; i < Walls.Count; i++)
            {
                TryToGenerateBlocks(i);
                while (IsBlockOnSnake(i))
                    TryToGenerateBlocks(i);
            }
        }

        private bool IsBlockOnSnake(int i)
        {
            for (var y = 0; y < Walls[i].Blocks.Length; y++)
                for (var j = 0; j < Snake.Count; j++)
                    if (Walls[i].Blocks[y].X == Snake[j].X && Walls[i].Blocks[y].Y == Snake[j].Y)
                        return true;
            return false;
        }

        private void TryToGenerateBlocks(int i)
        {
            Thread.Sleep(10);
            var rnd = new Random();
            Walls[i].Blocks[1].X = rnd.Next(1, Width - 1);
            Walls[i].Blocks[1].Y = rnd.Next(1, Height - 1);
            if (rnd.Next(0, 2) == 1)
            {
                Walls[i].Blocks[0].X = Walls[i].Blocks[1].X;
                Walls[i].Blocks[2].X = Walls[i].Blocks[1].X;
                Walls[i].Blocks[0].Y = Walls[i].Blocks[1].Y + 1;
                Walls[i].Blocks[2].Y = Walls[i].Blocks[1].Y - 1;
            }
            else
            {
                Walls[i].Blocks[0].Y = Walls[i].Blocks[1].Y;
                Walls[i].Blocks[2].Y = Walls[i].Blocks[1].Y;
                Walls[i].Blocks[0].X = Walls[i].Blocks[1].X + 1;
                Walls[i].Blocks[2].X = Walls[i].Blocks[1].X - 1;
            }
        }

        public void UpdateLevel()
        {
            if (Level <= 5)
                Level++;
            BlocksCount++;
            for (var i = 0; i < BlocksCount; i++)
                Walls.Add(new Wall());
            GenerateBlocks();
        }

        public void MoveSnake()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (MoveDirection)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }
                    int maxXPos = Width;
                    int maxYPos = Height;

                    if (HeatBlocks(i))
                    {
                        Die();
                    }

                    if (HeatBorder(i, maxXPos, maxYPos))
                    {
                        Die();
                    }

                    if (HeatSnake(i))
                    {
                        Die();
                    }

                    if (Snake[0].X == Food.X && Snake[0].Y == Food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private bool HeatSnake(int i)
        {
            for (int j = 1; j < Snake.Count; j++)
                if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                    return true;
            return false;
        }

        private bool HeatBorder(int i, int maxXPos, int maxYPos)
        {
            if (Snake[i].X < 0 || Snake[i].Y < 0
                || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                return true;
            return false;
        }

        private bool HeatBlocks(int i)
        {
            for (var x = 0; x < Walls.Count; x++)
                for (var y = 0; y < Walls[x].Blocks.Length; y++)
                    if (Walls[x].Blocks[y].X == Snake[i].X && Walls[x].Blocks[y].Y == Snake[i].Y)
                        return true;
            return false;
        }

        public void Die()
        {
            GameOver = true;
        }

        public void Eat()
        {
            RectPoint rectangle = new RectPoint
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(rectangle);
            Score += 100;
            if (Score % 300 == 0)
                UpdateLevel();
            GenerateFood();
        }
    }
}
