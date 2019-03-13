using Balls.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Balls
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private const int AMOUNT_OF_BALLS = 500;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random rnd;

        List<Ball> balls;
        List<Texture2D> textures;
        Color[] colors;

        Rectangle window;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1060;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 960;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            rnd = new Random();
            balls = new List<Ball>();
            textures = new List<Texture2D>();
            colors = new Color[5] { Color.Blue, Color.Red, Color.Yellow, Color.LightGreen, Color.Pink };
            window = GraphicsDevice.PresentationParameters.Bounds;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            textures.Add(Content.Load<Texture2D>("Sprites/smallball"));

            for (int i = 0; i < AMOUNT_OF_BALLS; i++)
            {
                balls.Add(new Ball(
                    textures[0],
                    new Vector2(rnd.Next(25, window.Width), rnd.Next(25, window.Height)), 
                    new Vector2(rnd.Next(-5, 5), rnd.Next(-5, 5)), 
                    rnd.Next(1, 10), 
                    colors[rnd.Next(0, colors.Length)]                    
                ));
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            for (int i = 0; i < balls.Count - 1; i++)
            {
                balls[i].Update(gameTime);
            }

            Direction direction = CheckDirectionalButtons();

            for (int i = 0; i <= balls.Count - 1; i++)
            {
                if (direction != Direction.None)
                    balls[i].ApplyGravity(direction);

                for (int j = 0; j <= balls.Count - 1; j++)
                {
                    if (i == j)
                        continue;

                    balls[i].CalculateCollision(balls[j]);
                }

                balls[i].Collides(window);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (Ball ball in balls)
            {
                ball.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private Direction CheckDirectionalButtons()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                return Direction.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                return Direction.Right;
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                return Direction.Up;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                return Direction.Down;
            return Direction.None;
        }
    }
}
