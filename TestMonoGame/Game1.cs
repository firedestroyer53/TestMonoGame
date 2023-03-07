using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;

namespace TestMonoGame;

public class Game1 : Game
{
    Texture2D whiteSquare;
    Texture2D blackSquare;
    Texture2D whitePawnTexture;
    Texture2D blackPawnTexture;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    
    


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = 512;
        _graphics.PreferredBackBufferHeight = 512;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        


        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        whiteSquare = Content.Load<Texture2D>("WhiteSquare");
        blackSquare = Content.Load<Texture2D>("BlackSquare");
        whitePawnTexture = Content.Load<Texture2D>("WhitePawn");
        blackPawnTexture = Content.Load<Texture2D>("BlackPawn");
    }

    protected override void Update(GameTime gameTime)
    {
        //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
        //    Keyboard.GetState().IsKeyDown(Keys.Escape))
        //    Exit();
        //var kstate = Keyboard.GetState();
        //var movement = Vector2.Zero;
        //var mstate = Mouse.GetState();

        //// Create a dictionary that maps keys to movement vectors
        //var keyVectors = new Dictionary<Keys, Vector2>
        //{
        //    { Keys.Up, new Vector2(0, -1) },
        //    { Keys.Down, new Vector2(0, 1) },
        //    { Keys.Left, new Vector2(-1, 0) },
        //    { Keys.Right, new Vector2(1, 0) }
        //};

        //// Sum up the movement vectors for all keys being pressed
        //foreach (var keyVector in keyVectors)
        //{
        //    if (kstate.IsKeyDown(keyVector.Key))
        //    {
        //        movement += keyVector.Value;
        //    }
        //}

        //if (movement != Vector2.Zero)
        //{
        //    // Normalize the movement vector
        //    movement.Normalize();

        //    // Multiply the normalized movement vector by the ball speed
        //    movement *= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        //    // Move the ball in the final movement direction
        //    ballPosition += movement;
        //}


        //if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
        //{
        //    ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        //}
        //else if (ballPosition.X < ballTexture.Width / 2)
        //{
        //    ballPosition.X = ballTexture.Width / 2;
        //}

        //if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
        //{
        //    ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        //}
        //else if (ballPosition.Y < ballTexture.Height / 2)
        //{
        //    ballPosition.Y = ballTexture.Height / 2;
        //}

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin();

        ChessBoard array2D = new ChessBoard();
        int cellSize = array2D.cellSize;

        for (int x = 0; x < array2D.getLengthX(); x++)
        {
            for (int y = 0; y < array2D.getLengthY(); y++)
            {
                int cellValue = array2D[x, y];

                // Calculate the position of the cell based on its x and y index.
                int xPos = x * cellSize;
                int yPos = y * cellSize;

                // Determine the color of the cell based on its position.
                Color cellColor = ((x + y) % 2 == 0) ? Color.AliceBlue : Color.Chocolate;

                // Draw the cell with a colored border.
                spriteBatch.Draw(
                    texture: whiteSquare,
                    destinationRectangle: new Rectangle(xPos, yPos, cellSize, cellSize),
                    sourceRectangle: null,
                    color: cellColor,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                );
            }
        }
        Color white = Color.White;
        Color black = Color.Black;
        List<Piece> whitePawns = new List<Piece>();
        List<Piece> blackPawns = new List<Piece>();
        // Create eight pawns for each color.
        
        for (int i = 1; i <= 8; i++)
        {
            // Create a new white pawn with the current pawn number and position.
            Piece whitePawn = new Piece(true, i, i, 2);

            // Create a new black pawn with the current pawn number and position.
            

            // Add the pawns to a list or array for later use.
            // For example:
            whitePawns.Add(whitePawn);
        }
        // Create eight black pawns.
        for (int i = 1; i <= 8; i++)
        {
            // Create a new black pawn with the current pawn number and position.
            Piece blackPawn = new Piece(false, i, i, 7);

            // Add the pawn to the list of black pawns.
            blackPawns.Add(blackPawn);
        }

        // Position the black pawns on the 7th rank of the board.
        for (int i = 0; i < blackPawns.Count; i++)
        {
            blackPawns[i].pieceY = 7;
            blackPawns[i].pieceX = i + 1;
        }


        //draw the pawns onto the screen
        foreach (Piece whitePawn in whitePawns)
        {
            spriteBatch.Draw(texture: whitePawnTexture,
                             destinationRectangle: new Rectangle((whitePawn.pieceX - 1) * cellSize, (whitePawn.pieceY - 1) * cellSize, cellSize, cellSize),
                             sourceRectangle: null,
                             color: white,
                             rotation: 0f,
                             origin: Vector2.Zero,
                             effects: SpriteEffects.None,
                             layerDepth: 0f);
        }
        //draw the black pawns
        foreach (Piece blackPawn in blackPawns)
        {
            spriteBatch.Draw(texture: blackPawnTexture,
                             destinationRectangle: new Rectangle((blackPawn.pieceX - 1) * cellSize, (blackPawn.pieceY - 1) * cellSize, cellSize, cellSize),
                             sourceRectangle: null,
                             color: black,
                             rotation: 0f,
                             origin: Vector2.Zero,
                             effects: SpriteEffects.None,
                             layerDepth: 0f);
        }

        spriteBatch.End();
    base.Draw(gameTime);
    }
}