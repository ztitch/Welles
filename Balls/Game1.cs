using Balls.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Balls
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const bool GRAVITY = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<Ball> balls;
        List<Texture2D> textures;

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
            balls = new List<Ball>();
            textures = new List<Texture2D>();
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
            textures.Add(Content.Load<Texture2D>("Sprites/ball"));
            //textures.Add(Content.Load<Texture2D>("Sprites/donald"));
            //textures.Add(Content.Load<Texture2D>("Sprites/kim"));
            //textures.Add(Content.Load<Texture2D>("Sprites/putin"));
            //textures.Add(Content.Load<Texture2D>("Sprites/reee"));

            balls.Add(new Ball(textures[0], new Vector2(100, 100), new Vector2(1.5f, 1f), 20, 1, Color.Green));
            balls.Add(new Ball(textures[0], new Vector2(200, 800), new Vector2(1.5f, -2f), 20, 2, Color.Yellow));
            balls.Add(new Ball(textures[0], new Vector2(800, 100), new Vector2(0.5f, -1f), 20, 3, Color.Red));
            balls.Add(new Ball(textures[0], new Vector2(300, 500), new Vector2(1.5f, -1.5f), 20, 4, Color.Pink));
            balls.Add(new Ball(textures[0], new Vector2(100, 650), new Vector2(1.5f, 1f), 20, 1, Color.Green));
            balls.Add(new Ball(textures[0], new Vector2(700, 800), new Vector2(1.5f, -2f), 20, 2, Color.Yellow));
            //balls.Add(new Ball(textures[0], new Vector2(800, 500), new Vector2(0.5f, -1f), 20, 3, Color.Red));
            //balls.Add(new Ball(textures[0], new Vector2(500, 500), new Vector2(1.5f, -1.5f), 20, 4, Color.Pink));
            //balls.Add(new Ball(textures[0], new Vector2(750, 55), new Vector2(1.5f, 1f), 20, 1, Color.Green));
            //balls.Add(new Ball(textures[0], new Vector2(450, 275), new Vector2(1.5f, -2f), 20, 2, Color.Yellow));
            //balls.Add(new Ball(textures[0], new Vector2(755, 200), new Vector2(0.5f, -1f), 20, 3, Color.Red));
            //balls.Add(new Ball(textures[0], new Vector2(520, 350), new Vector2(1.5f, -1.5f), 20, 4, Color.Pink));
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

            for (int i = 0; i <= balls.Count - 1; i++)
            {
                for (int j = 0; j <= balls.Count - 1; j++)
                {
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
    }
}
