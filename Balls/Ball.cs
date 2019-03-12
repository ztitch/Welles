using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balls
{
    class Ball
    {
        private const float MAX_SPEED = 3f;
        private const bool LOCKED_MAX_SPEED = true;
        private const bool GRAVITY_ENABLED = false; // TODO

        private Texture2D texture; // Ball texture, duhh
        public Vector2 position; // Ball position
        public Vector2 center; // Center of the ball
        public Vector2 velocity; // The speed and direction at which the ball moves

        private float radius; // No pun intended
        private float mass;

        private Color color;

        public Ball(Texture2D texture, Vector2 position, Vector2 velocity, float radius, float mass, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.color = color;
            this.radius = texture.Width;
            this.center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Width / 2);
        }

        public void Update(GameTime gameTime) // Every object should have a Update method, so the main Update stays clean
        {
            // For collision it's very important to update the position first and then check if it collides with something in the next frame
            this.center += this.velocity;
            this.position += this.velocity;

            if(LOCKED_MAX_SPEED)
            {
                if (velocity.X > MAX_SPEED)
                    velocity.X = MAX_SPEED;
                if (velocity.Y > MAX_SPEED)
                    velocity.Y = MAX_SPEED;
            }
        }

        public void Draw(SpriteBatch spriteBatch) // Same counts for Draw
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

            spriteBatch.Draw(texture, position, sourceRectangle, this.color, 0f, origin, 2f, SpriteEffects.None, 0f);
        }

        #region Gravity
        // TO DO
        #endregion

        #region Collision
        public void CalculateCollision(Ball ball)
        {
            if(Collides(ball))
            {
                Collide(ball);
            }
        }

        public bool Collides(Ball ball) // Check if the balls are actually colliding
        {
            var distance = Math.Sqrt(
                ((this.center.X - ball.center.X) * (this.center.X - ball.center.X)) + 
                ((this.center.Y - ball.center.Y) * (this.center.Y - ball.center.Y)));

            if (distance < this.radius + ball.radius)
                return true;
            return false;
        }

        public void Collides(Rectangle window)
        {
            bool collided = false;
            if (this.center.X + this.radius / 2 > window.Width)
            {
                velocity.X *= -1;
            }
            else if (this.center.X - this.radius < 0)
            {
                velocity.X *= -1;
            }

            if (this.center.Y + this.radius / 2 > window.Height)
            {
                velocity.Y *= -1;
            }
            else if (this.center.Y - this.radius < 0)
            {
                velocity.Y *= -1;
            }

            if (collided)
            {
                this.center += this.velocity;
                this.position += this.velocity;
            }
        }

        public void Collide(Ball ball) // And finnaly calculates which direction the balls must go
        {
            float newVelX1 = (this.velocity.X * (this.mass - ball.mass) + (2 * ball.mass * ball.velocity.X)) / (this.mass + ball.mass);
            float newVelY1 = (this.velocity.Y * (this.mass - ball.mass) + (2 * ball.mass * ball.velocity.Y)) / (this.mass + ball.mass);
            float newVelX2 = (ball.velocity.X * (ball.mass - this.mass) + (2 * this.mass * this.velocity.X)) / (this.mass + ball.mass);
            float newVelY2 = (ball.velocity.Y * (ball.mass - this.mass) + (2 * this.mass * this.velocity.Y)) / (this.mass + ball.mass);

            this.velocity = new Vector2(newVelX1, newVelY1);
            ball.velocity = new Vector2(newVelX2, newVelY2);

            this.center += this.velocity;
            this.position += this.velocity;

            ball.center += ball.velocity;
            ball.position += ball.velocity;
        }
        #endregion
    }
}
