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
        private const float MAX_SPEED = 5f;
        private const bool LOCKED_MAX_SPEED = false;

        private const float GRAVITATIONAL_CONST = 0.0981f;

        private Texture2D texture; // Ball texture, duhh
        public Vector2 position; // Ball position
        public Vector2 center; // Center of the ball
        public Vector2 velocity; // The speed and direction at which the ball moves

        private float radius; // No pun intended
        private float mass;

        private Color color;

        public Ball(Texture2D texture, Vector2 position, Vector2 velocity, float mass, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.color = color;
            this.radius = texture.Width / 2;
            this.center = new Vector2(position.X + texture.Width / 2, position.Y + texture.Width / 2);
        }

        public void Update(GameTime gameTime) // Every object should have a Update method, so the main Update stays clean
        {
            // For collision it's very important to update the position first and then check if it collides with something in the next frame
            this.center += this.velocity;
            this.position += this.velocity;

            if (LOCKED_MAX_SPEED)
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

            spriteBatch.Draw(texture, position, sourceRectangle, this.color, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        #region Gravity
        public void ApplyGravity(Direction direction)
        {
            Vector2 force = new Vector2();
            switch (direction)
            {
                case Direction.Down:
                    force.Y = mass * GRAVITATIONAL_CONST;       // The normal gravity F = m * g - I divided the value by 100 because otherwise the balls would fly away in an instance
                    break;
                case Direction.Up:
                    force.Y = mass * GRAVITATIONAL_CONST * -1;
                    break;
                case Direction.Left:
                    force.X = mass * GRAVITATIONAL_CONST * -1;
                    break;
                case Direction.Right:
                    force.X = mass * GRAVITATIONAL_CONST;
                    break;
                case Direction.None:
                default:
                    break;
            }
            
            this.velocity += force;
        }
        #endregion

        #region Collision
        public void CalculateCollision(Ball ball)
        {
            if (Collides(ball))
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
            // Elastic collision https://en.wikipedia.org/wiki/Elastic_collision
            float newVelX1 = (this.velocity.X * (this.mass - ball.mass) + (2 * ball.mass * ball.velocity.X)) / (this.mass + ball.mass);
            float newVelY1 = (this.velocity.Y * (this.mass - ball.mass) + (2 * ball.mass * ball.velocity.Y)) / (this.mass + ball.mass);
            float newVelX2 = (ball.velocity.X * (ball.mass - this.mass) + (2 * this.mass * this.velocity.X)) / (this.mass + ball.mass);
            float newVelY2 = (ball.velocity.Y * (ball.mass - this.mass) + (2 * this.mass * this.velocity.Y)) / (this.mass + ball.mass);

            this.velocity = new Vector2(newVelX1, newVelY1);
            ball.velocity = new Vector2(newVelX2, newVelY2);

            // Move before next update to reduce clumping
            this.center += this.velocity;
            this.position += this.velocity;

            ball.center += ball.velocity;
            ball.position += ball.velocity;
        }
        #endregion
    }
}
