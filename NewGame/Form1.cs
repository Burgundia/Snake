using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewGame
{
    public partial class GameForm : Form
    {
        const int ElementSize = 16;
        public World FirstWorld;
        public Timer gameTimer;
        public GameForm()
        {
            BackgroundImage = new Bitmap("back.png");
            FirstWorld = new World();
            DoubleBuffered = true;
            ClientSize = new Size(FirstWorld.Width * ElementSize, FirstWorld.Height * ElementSize);
            InitializeComponent();
            gameTimer = new Timer();
            gameTimer.Interval = 1000 / FirstWorld.Speed;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            gameTimer.Tick += UpdateScreen;
            Paint += PaintSnake;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            gameTimer.Start();
            StartGame();
        }

        private void StartGame()
        {
            FirstWorld = new World();
            FirstWorld.Snake.Clear();
            RectPoint head = new RectPoint { X = 10, Y = 5 };
            FirstWorld.Snake.Add(head);
            FirstWorld.GenerateBlocks();
            FirstWorld.GenerateFood();
        }

        protected void PaintSnake(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!FirstWorld.GameOver)
            {
                for (int i = 0; i < FirstWorld.Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;
                    else
                        snakeColour = Brushes.Green;

                    canvas.FillRectangle(snakeColour,
                        new Rectangle(FirstWorld.Snake[i].X * ElementSize,
                                      FirstWorld.Snake[i].Y * ElementSize,
                                      ElementSize, ElementSize));
                }
                Image img = new Bitmap("apple.png");
                canvas.DrawImage(img, new Rectangle(FirstWorld.Food.X * ElementSize,
                        FirstWorld.Food.Y * ElementSize, ElementSize, ElementSize));
                for (var x = 0; x < FirstWorld.Walls.Count; x++)
                    for (var y = 0; y < FirstWorld.Walls[x].Blocks.Length; y++)
                    {
                        canvas.DrawImage(FirstWorld.Walls[x].img, new Rectangle(FirstWorld.Walls[x].Blocks[y].X * ElementSize,
                                FirstWorld.Walls[x].Blocks[y].Y * ElementSize,
                                ElementSize, ElementSize));
                    }
            }
            else
            {
                string gameOver = "Game over \nYour score is: " + FirstWorld.Score + "\nPress Enter to try again";
                canvas.Clear(Color.White);
                canvas.DrawString(gameOver, new Font("Arial", 16), Brushes.Black, new Point((FirstWorld.Width * ElementSize) / 2 - 100, (FirstWorld.Height * ElementSize) / 2 - 100));
            }
        }
        private void UpdateScreen(object sender, EventArgs e)
        {
            if (FirstWorld.GameOver)
            {
                if (InputManager.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {

                if (InputManager.KeyPressed(Keys.Right) && FirstWorld.MoveDirection != Direction.Left)
                    FirstWorld.MoveDirection = Direction.Right;
                else if (InputManager.KeyPressed(Keys.Left) && FirstWorld.MoveDirection != Direction.Right)
                    FirstWorld.MoveDirection = Direction.Left;
                else if (InputManager.KeyPressed(Keys.Up) && FirstWorld.MoveDirection != Direction.Down)
                    FirstWorld.MoveDirection = Direction.Up;
                else if (InputManager.KeyPressed(Keys.Down) && FirstWorld.MoveDirection != Direction.Up)
                    FirstWorld.MoveDirection = Direction.Down;
                FirstWorld.MoveSnake();
            }
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            InputManager.ChangeState(e.KeyCode, true);


        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            InputManager.ChangeState(e.KeyCode, false);
            if (e.KeyCode == Keys.Escape)
                PauseGame(e);
        }

        private void PauseGame(KeyEventArgs e)
        {
            gameTimer.Enabled = !gameTimer.Enabled;
            if (gameTimer.Enabled)
                Text = "Snake";
        }

        public static void playSimpleSound()
        {
            SoundPlayer simpleSound = new SoundPlayer("zvuk.wav");
            simpleSound.Play();
        }
    }
}
