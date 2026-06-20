using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using brickbuster.Entities.Blocks;
using System.Collections.Generic;
using System;
using brickbuster.Systems;
using brickbuster.Config;

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
        Radius = GameConstants.BallRadius;

        // Start the ball in the center of the screen
        Position = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
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
            Velocity = new Vector2(GameConstants.InitialBallSpeedX, GameConstants.InitialBallSpeedY);
            IsLaunched = true;
        }
    }

    public void HandleWallCollision(Viewport viewport, int wallThickness = GameConstants.BorderThickness)
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
        float remainingTime = 1f;

        const int MaxCollisionsPerFrame = 4;

        for (int iteration = 0; iteration < MaxCollisionsPerFrame; iteration++)
        {
            float earliest = 1f;
            BlockBase hitBlock = null;
            Vector2 hitNormal = Vector2.Zero;

            Vector2 velocityStep =
                Velocity * _deltaTime * remainingTime;

            foreach (var block in blocks)
            {
                if (block.IsDestroyed)
                    continue;

                float t = SweptAABB(
                    block.Rect,
                    velocityStep,
                    out Vector2 normal);

                if (t < earliest)
                {
                    earliest = t;
                    hitBlock = block;
                    hitNormal = normal;
                }
            }

            // Ingen mer träff denna frame
            if (hitBlock == null)
            {
                Position += velocityStep;
                return;
            }

            // Flytta fram till träffen
            Position += velocityStep * earliest;

            AudioSystem.PlayBlockHit();

            hitBlock.Hit();

            Velocity = Vector2.Reflect(Velocity, hitNormal);

            // Litet offset för att undvika att fastna exakt på kanten
            const float Epsilon = 0.01f;
            Position += hitNormal * Epsilon;

            remainingTime *= (1f - earliest);

            if (remainingTime < 0.0001f)
                return;
        }

        // Om vi nått max antal kollisioner
        // lämnar vi resten av rörelsen för nästa frame.
    }
    private float SweptAABB(Rectangle block, Vector2 velocityStep, out Vector2 normal)
    {
        normal = Vector2.Zero;

        float left = block.Left - Radius;
        float right = block.Right + Radius;
        float top = block.Top - Radius;
        float bottom = block.Bottom + Radius;

        float xEntry, xExit;
        float yEntry, yExit;

        // X
        if (velocityStep.X > 0)
        {
            xEntry = (left - Position.X) / velocityStep.X;
            xExit = (right - Position.X) / velocityStep.X;
        }
        else if (velocityStep.X < 0)
        {
            xEntry = (right - Position.X) / velocityStep.X;
            xExit = (left - Position.X) / velocityStep.X;
        }
        else
        {
            xEntry = float.NegativeInfinity;
            xExit = float.PositiveInfinity;
        }

        // Y
        if (velocityStep.Y > 0)
        {
            yEntry = (top - Position.Y) / velocityStep.Y;
            yExit = (bottom - Position.Y) / velocityStep.Y;
        }
        else if (velocityStep.Y < 0)
        {
            yEntry = (bottom - Position.Y) / velocityStep.Y;
            yExit = (top - Position.Y) / velocityStep.Y;
        }
        else
        {
            yEntry = float.NegativeInfinity;
            yExit = float.PositiveInfinity;
        }

        float entryTime = Math.Max(xEntry, yEntry);
        float exitTime = Math.Min(xExit, yExit);

        if (entryTime > exitTime)
            return 1f;

        if (entryTime < 0f || entryTime > 1f)
            return 1f;

        if (xEntry > yEntry)
        {
            normal = velocityStep.X > 0
                ? new Vector2(-1, 0)
                : new Vector2(1, 0);
        }
        else
        {
            normal = velocityStep.Y > 0
                ? new Vector2(0, -1)
                : new Vector2(0, 1);
        }

        return entryTime;
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

    public void UpdateDeltaTime(GameTime gameTime)
    {
        if (!IsLaunched)
        {
            // If the ball is not launched, it should not move
            return;
        }

        // Move the ball based on its velocity and the elapsed time since the last update
        _deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
