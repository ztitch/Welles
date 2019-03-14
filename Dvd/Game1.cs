using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Dvd
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D t;
        Vector2 pos;

        float speedY, speedX;

        List<Color> colors;

        Color color;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 900;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 800;   // set this value to the desired height of your window
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
            float mainSpeed = 3f;

            speedX = mainSpeed;
            speedY = mainSpeed;

            colors = new List<Color>
            {
                Color.Purple,
                Color.Yellow,
                Color.Orange,
                Color.Pink,
            };

            color = Color.Blue;

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
            t = Content.Load<Texture2D>("textures/dvd-100");

            // TODO: use this.Content to load your game content here
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
            //speedY += 0.5f;
            pos.Y += speedY;
            pos.X += speedX;

            System.Random rnd = new System.Random();

            // vertical screen size boundries
            if (pos.Y + t.Height >= GraphicsDevice.PresentationParameters.BackBufferHeight || pos.Y <= 0)
            {
                speedY *= -1;
                pos.Y += speedY;

                int temp = rnd.Next(0, colors.Count);

                colors.Add(color);
                color = colors[temp];
                colors.RemoveAt(temp);

            }

            // horizontal ..
            if (pos.X + t.Width >= GraphicsDevice.PresentationParameters.BackBufferWidth || pos.X <= 0)
            {
                speedX *= -1;

                int temp = rnd.Next(0, colors.Count);

                colors.Add(color);
                color = colors[temp];
                colors.RemoveAt(temp);
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            Rectangle sourceRectangle = new Rectangle(0, 0, t.Width, t.Height);
            Vector2 origin = new Vector2(t.Width / 2, t.Height / 2);

            spriteBatch.Draw(t, pos, null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
            // TODO: Add your drawing code here new Vector2(t.Width, t.Height)

            base.Draw(gameTime);
        }
    }
}
