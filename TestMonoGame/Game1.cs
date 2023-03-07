using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Reflection.Metadata.Ecma335;

namespace TestMonoGame;

public class Game1 : Game
{
    Texture2D ballTexture;
    Vector2 ballPosition;
    float ballSpeed;
    Vector2 ballVelocity;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

   
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight);
        ballSpeed = 500f;


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        var kstate = Keyboard.GetState();
        var movement = Vector2.Zero;

        // Create a dictionary that maps keys to movement vectors
        var keyVectors = new Dictionary<Keys, Vector2>
        {
            { Keys.Up, new Vector2(0, -1) },
            { Keys.Down, new Vector2(0, 1) },
            { Keys.Left, new Vector2(-1, 0) },
            { Keys.Right, new Vector2(1, 0) }
        };

        // Sum up the movement vectors for all keys being pressed
        foreach (var keyVector in keyVectors)
        {
            if (kstate.IsKeyDown(keyVector.Key))
            {
                movement += keyVector.Value;
            }
        }

        if (movement != Vector2.Zero)
        {
            // Normalize the movement vector
            movement.Normalize();

            // Multiply the normalized movement vector by the ball speed
            movement *= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the ball in the final movement direction
            ballPosition += movement;
        }


        if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
        {
            ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        }
        else if (ballPosition.X < ballTexture.Width / 2)
        {
            ballPosition.X = ballTexture.Width / 2;
        }

        if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
        {
            ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        }
        else if (ballPosition.Y < ballTexture.Height / 2)
        {
            ballPosition.Y = ballTexture.Height / 2;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(ballTexture,
                          ballPosition,
                          null,
                          Color.White,
                          0f,
                          new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                          Vector2.One,
                          SpriteEffects.None,
                          0f);


        _spriteBatch.End();

        base.Draw(gameTime);
    }
}