using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestMonoGame;

public class Game1 : Game
{
    Texture2D whiteSquare;
    Texture2D blackSquare;
    Texture2D whitePawnTexture;
    Texture2D blackPawnTexture;

    Piece selectedPiece = null;
    private bool wasLeftButtonPressed = false;

    ChessBoard board = new ChessBoard();

    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;

    Color white = Color.White;
    Color black = Color.Black;

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
        //setup blank board
        
        // TODO: Add your initialization logic here
        for (int i = 0; i < 8; i++)
        {
            // Create a new black pawn with the current pawn number and position.
            Piece whitePawn = new(true, i, i, 2);

            // Add the pawns to a list or array for later use.
            // For example:
            board.placePiece(whitePawn);
        }
        // Create eight white pawns.
        for (int i = 0; i < 8; i++)
        {
            // Create a new white pawn with the current pawn number and position.
            Piece blackPawn = new(false, i, i, 7);

            // Add the pawn to the list of white pawns.
            board.placePiece(blackPawn);
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
        int cellX;
        int cellY;

        if (mstate.LeftButton == ButtonState.Pressed && !wasLeftButtonPressed)
        {
            // Get the position of the cell that the mouse is currently on
            cellX = mstate.X / 64;
            cellY = (mstate.Y / 64) + 1;


            if (selectedPiece == null)
            {
                // If there's no piece currently selected, check if there's a piece at the clicked cell and select it
                Piece clickedPiece = board[cellX, cellY];
                if (clickedPiece != null)
                {
                    selectedPiece = clickedPiece;
                }
            }
            else
            {
                // If there's a piece already selected, move it to the clicked cell
                try
                {
                    board.movePiece(selectedPiece, cellX, cellY);
                    selectedPiece = null; // Deselect the piece after it's been moved
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        wasLeftButtonPressed = mstate.LeftButton == ButtonState.Pressed;

        base.Update(gameTime);
    }




    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin();

        
        int cellSize = 64;

        for (int x = 0; x < board.getLengthX(); x++)
        {
            for (int y = 0; y < board.getLengthY(); y++)
            {
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
        foreach (Piece piece in board)
        {
            if (piece != null)
            {
                
                if (piece.isWhite)
                {
                    spriteBatch.Draw(
                        texture: whitePawnTexture,
                        destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY - 1) * cellSize, cellSize, cellSize),
                        sourceRectangle: null,
                        color: white,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        effects: SpriteEffects.None,
                        layerDepth: 0f
                    );

                }
                else
                {
                    spriteBatch.Draw(texture: blackPawnTexture,
                                 destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY-1) * cellSize, cellSize, cellSize),
                                 sourceRectangle: null,
                                 color: black,
                                 rotation: 0f,
                                 origin: Vector2.Zero,
                                 effects: SpriteEffects.None,
                                 layerDepth: 0f);
                }
            }
            

        }


        spriteBatch.End();
        base.Draw(gameTime);
    }
}
