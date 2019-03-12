using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls.Classes
{
    class SpriteObject // PASOP, deze class is poep
    {
        private Texture2D texture;
        public Rectangle rectangle;
        public Vector2 centre;

        private Vector2 position;
        private Vector2 velocity;

        private float mass;
        private float size;
        private float gravity;
        private float drag;
        private bool hasGravity;

        public SpriteObject(Texture2D texture, Vector2 position, Vector2 velocity, float mass, float size, bool hasGravity)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.size = size;
            this.gravity = 0;
            this.drag = 0;
            this.hasGravity = hasGravity;
        }

        public void Update(GameTime gameTime, List<SpriteObject> sprites, Rectangle window)
        {
            if (this.hasGravity)
                UpdateGravity();

            CheckCollision(window);

            foreach (SpriteObject sprite in sprites)
            {
                if (sprite == this)
                    continue;

                if ((this.IsTouchingLeft(sprite)) || 
                    (this.IsTouchingRight(sprite)))
                    this.velocity.X *= -1;

                if ((this.IsTouchingTop(sprite)) ||
                    (this.IsTouchingBottom(sprite)))
                    this.velocity.Y *= -1;
            }

            Move();
        }

        private void Move()
        {
            this.position += this.velocity;
            this.rectangle.Location = new Point((int)position.X, (int)position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            this.rectangle = sourceRectangle;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            centre = origin;

            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, origin, size, SpriteEffects.None, 0f);
        }

        // This collision sucks, https://gamedevelopment.tutsplus.com/tutorials/when-worlds-collide-simulating-circle-circle-collisions--gamedev-769 for the real shit
        public bool CheckCollision(SpriteObject obj)
        {
            bool collided = false;

            if(IsTouchingLeft(obj) || IsTouchingRight(obj)) // Collision for left side
            {
                this.velocity.X *= -1;
                obj.velocity.X *= -1;
                collided = true;
            }

            else if (IsTouchingTop(obj) || IsTouchingBottom(obj)) // Collision for top side
            {
                this.velocity.Y *= -1;
                obj.velocity.Y *= -1;
                collided = true;
            }

            return collided;
        }

        protected bool IsTouchingLeft(SpriteObject sprite)
        {
            return this.rectangle.Right + this.velocity.X > sprite.rectangle.Left &&
              this.rectangle.Left < sprite.rectangle.Left &&
              this.rectangle.Bottom > sprite.rectangle.Top &&
              this.rectangle.Top < sprite.rectangle.Bottom;
        }

        protected bool IsTouchingRight(SpriteObject sprite)
        {
            return this.rectangle.Left + this.velocity.X < sprite.rectangle.Right &&
              this.rectangle.Right > sprite.rectangle.Right &&
              this.rectangle.Bottom > sprite.rectangle.Top &&
              this.rectangle.Top < sprite.rectangle.Bottom;
        }

        protected bool IsTouchingTop(SpriteObject sprite)
        {
            return this.rectangle.Bottom + this.velocity.Y > sprite.rectangle.Top &&
              this.rectangle.Top < sprite.rectangle.Top &&
              this.rectangle.Right > sprite.rectangle.Left &&
              this.rectangle.Left < sprite.rectangle.Right;
        }

        protected bool IsTouchingBottom(SpriteObject sprite)
        {
            return this.rectangle.Top + this.velocity.Y < sprite.rectangle.Bottom &&
              this.rectangle.Bottom > sprite.rectangle.Bottom &&
              this.rectangle.Right > sprite.rectangle.Left &&
              this.rectangle.Left < sprite.rectangle.Right;
        }

        public void CheckCollision(Rectangle window)
        {
            if (position.X + velocity.X > window.Width)
            {
                velocity.X *= -1;
                position.X = window.Width;
            }
            else if (position.X + velocity.X < 0)
            {
                velocity.X *= -1;
                position.X = 0;
            }

            if (position.Y + velocity.Y > window.Height)
            {
                velocity.Y *= -1;
                position.Y = window.Height;
            }
            else if (position.Y + velocity.Y < 0)
            {
                velocity.Y *= -1;
                position.Y = 0;
            }
        }

        private void UpdateGravity()
        {
            gravity -= 0.001f;
            Console.WriteLine(gravity);
            drag -= 0.0001f;

            // Not really air resistance 
            // Real air resistance should be: F = 1/2 * p * v² * A * Cʷ
            // F = Force
            // p = density
            // v = relative speed
            // A = surface facing the direction
            // Cʷ = Resistance coefficient - dimensionless number
            // https://en.wikipedia.org/wiki/Drag_(physics)

            if (velocity.X > 0 && drag < 0)
                drag *= -1;
            else if(velocity.X < 0 && drag > 0)
                drag *= -1;

            // Also not 'real' gravity. It just makes it looks like it by reducing the Y axis of the object more and more.
            // Real gravity should be: F = m * g
            // F = force
            // m = mass
            // G = gravitational constant (around 10 for earth)
            // https://en.wikipedia.org/wiki/Gravity

            // These numbers could be applied everywhere for a 'real' simulation of how objects behave on earth or other spaces

            velocity = new Vector2(velocity.X -= drag, velocity.Y - gravity);
        }

        public void SetVelocity(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }
}
