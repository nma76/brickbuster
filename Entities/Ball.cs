using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using brickbuster.Entities.Blocks;
using System.Collections.Generic;
using System;
using brickbuster.Systems;

namespace brickbuster.Entities;

public class Ball
{
    // The ball's position, velocity, and radius
    public Vector2 Position;
    public Vector2 Velocity;
    public float Radius;

    public AudioSystem AudioSystem { get; private set; }

    // Indicates whether the ball has been launched or is still waiting on the paddle
    public bool IsLaunched { get; private set; } = false;

    // Events
    public event Action OnPaddleHit;

    private float _deltaTime;

    public Ball(Viewport viewport, AudioSystem audioSystem)
    {
        AudioSystem = audioSystem;

        // Set the ball's radius
        Radius = 8f;

        // Start the ball in the center of the screen
        Position = new Vector2(viewport.Width / 2f, viewport.Height / 2f);

        // Give the ball an initial velocity
        Velocity = new Vector2(350f, -350f);
    }

    public void AttachToPaddle(Rectangle paddleRect)
    {
        // Position the ball on top of the paddle, centered horizontally
        Position.X = paddleRect.X + paddleRect.Width / 2f;
        Position.Y = paddleRect.Y - Radius - 2;

        // Reset the ball's velocity
        Velocity = Vector2.Zero;

        // The ball is not launched yet
        IsLaunched = false;
    }

    public void Launch()
    {
        if (!IsLaunched)
        {
            // Give the ball an initial velocity to start moving
            Velocity = new Vector2(150f, -350f);
            IsLaunched = true;
        }
    }

    public void HandleWallCollision(Viewport viewport, int wallThickness = 10)
    {
        // Check for collision with the left wall
        if (Position.X - Radius < wallThickness)
        {
            Position.X = wallThickness + Radius;
            Velocity.X *= -1;
            AudioSystem.PlayWallHit();
        }

        // Check for collision with the right wall
        if (Position.X + Radius > viewport.Width - wallThickness)
        {
            Position.X = viewport.Width - wallThickness - Radius;
            Velocity.X *= -1;
            AudioSystem.PlayWallHit();
        }

        // Check for collision with the top wall
        if (Position.Y - Radius < wallThickness)
        {
            Position.Y = wallThickness + Radius;
            Velocity.Y *= -1;
            AudioSystem.PlayWallHit();
        }
    }

    public void HandlePaddleCollision(Rectangle paddleRect)
    {
        // Check if the ball is colliding with the paddle
        Rectangle ballRect = new Rectangle(
            (int)(Position.X - Radius),
            (int)(Position.Y - Radius),
            (int)(Radius * 2),
            (int)(Radius * 2)
        );

        if (ballRect.Intersects(paddleRect))
        {
            // Move the ball out of the paddle to prevent sticking
            Position.Y = paddleRect.Y - Radius;

            // Simple collision response: reverse the Y velocity
            Velocity.Y *= -1;

            // Add some horizontal velocity based on where the ball hit the paddle
            float paddleCenter = paddleRect.X + paddleRect.Width / 2f;
            float distanceFromCenter = Position.X - paddleCenter;

            // Normalize the distance to a value between -1 and 1
            float normalized = distanceFromCenter / (paddleRect.Width / 2f);

            // Scale the horizontal velocity based on the normalized distance
            Velocity.X = normalized * 250f;

            // Trigger event to let subscibers know ball hit the paddle
            OnPaddleHit?.Invoke();
        }
    }

    public void HandleBlockCollision(List<BlockBase> blocks)
    {
        float earliest = 1f;
        BlockBase hitBlock = null;
        Vector2 hitNormal = Vector2.Zero;

        // 1. Hitta tidigaste kollisionen
        foreach (var block in blocks)
        {
            if (block.IsDestroyed)
                continue;

            float t = SweptAABB(block.Rect, out Vector2 normal);

            if (t < earliest)
            {
                earliest = t;
                hitBlock = block;
                hitNormal = normal;
            }
        }

        // Ingen kollision denna frame
        if (hitBlock == null)
            return;

        // Play sound if block is hit
        AudioSystem.PlayBlockHit();

        // 2. Flytta bollen till kollisionstidpunkten
        Position += Velocity * _deltaTime * earliest;

        // 3. Reflektera hastigheten
        Velocity = Vector2.Reflect(Velocity, hitNormal);

        // 4. Skada blocket
        hitBlock.Hit();

        // 5. Flytta resterande del av rörelsen efter studsen
        float remaining = 1f - earliest;
        Position += Velocity * _deltaTime * remaining;
    }

    private float SweptAABB(Rectangle block, out Vector2 normal)
    {
        normal = Vector2.Zero;

        // Bollens rörelse denna frame
        Vector2 vel = Velocity * _deltaTime;

        // Utökad AABB (blocket expanderas med bollens diameter)
        float expandedLeft = block.Left - Radius * 2;
        float expandedRight = block.Right;
        float expandedTop = block.Top - Radius * 2;
        float expandedBottom = block.Bottom;

        // Start- och slutposition
        Vector2 start = new(Position.X - Radius, Position.Y - Radius);
        Vector2 end = start + vel;

        // Ray vs AABB
        float tEntryX, tEntryY;
        float tExitX, tExitY;

        if (vel.X > 0)
        {
            tEntryX = (expandedLeft - start.X) / vel.X;
            tExitX = (expandedRight - start.X) / vel.X;
        }
        else
        {
            tEntryX = (expandedRight - start.X) / vel.X;
            tExitX = (expandedLeft - start.X) / vel.X;
        }

        if (vel.Y > 0)
        {
            tEntryY = (expandedTop - start.Y) / vel.Y;
            tExitY = (expandedBottom - start.Y) / vel.Y;
        }
        else
        {
            tEntryY = (expandedBottom - start.Y) / vel.Y;
            tExitY = (expandedTop - start.Y) / vel.Y;
        }

        float tEntry = Math.Max(tEntryX, tEntryY);
        float tExit = Math.Min(tExitX, tExitY);

        // Ingen kollision
        if (tEntry > tExit || tEntry < 0f || tEntry > 1f)
            return 1f;

        // Bestäm normal
        if (tEntryX > tEntryY)
            normal = new Vector2(vel.X > 0 ? -1 : 1, 0);
        else
            normal = new Vector2(0, vel.Y > 0 ? -1 : 1);

        return tEntry;
    }



    public bool IsOutOfBounds(Viewport viewport)
    {
        // Check if the ball has fallen below the bottom of the screen
        return Position.Y - Radius > viewport.Height;
    }

    public void Reset(Viewport viewport)
    {
        Position = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        Velocity = new Vector2(350f, -350f);
    }

    public void Update(GameTime gameTime)
    {
        if (!IsLaunched)
        {
            // If the ball is not launched, it should not move
            return;
        }

        // Move the ball based on its velocity and the elapsed time since the last update
        _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += Velocity * _deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        // Draw the ball as a filled circle using the pixel texture
        // We can draw a circle by drawing a rectangle and using the radius to create a circular mask
        Rectangle rect = new Rectangle(
            (int)(Position.X - Radius),
            (int)(Position.Y - Radius),
            (int)(Radius * 2),
            (int)(Radius * 2)
        );

        spriteBatch.Draw(pixel, rect, Color.Yellow);
    }
}
