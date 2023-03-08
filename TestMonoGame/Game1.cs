using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace TestMonoGame;

public class Game1 : Game
{
    Texture2D whiteSquare;
    Texture2D blackSquare;
    Texture2D whitePawnTexture;
    Texture2D blackPawnTexture;



    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;
    Color white = Color.White;
    Color black = Color.Black;
    List<Piece> whitePawns = new List<Piece>();
    List<Piece> blackPawns = new List<Piece>();
    


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
        MouseState mstate = Mouse.GetState();

        // Check if the mouse is currently over any of the white pawns.
        foreach (Piece whitePawn in whitePawns)
        {
            if (whitePawn.isMouseOver(mstate) && mstate.LeftButton == ButtonState.Pressed)
            {
                
            }
        }

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