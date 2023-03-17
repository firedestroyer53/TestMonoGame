using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessTest;

public class Game1 : Game
{
    private Texture2D blackBishopTexture;
    private Texture2D blackKingTexture;
    private Texture2D blackKnightTexture;
    private Texture2D blackPawnTexture;
    private Texture2D blackQueenTexture;
    private Texture2D blackRookTexture;
    private Texture2D whiteBishopTexture;
    private Texture2D whiteKingTexture;
    private Texture2D whiteKnightTexture;
    private Texture2D whitePawnTexture;
    private Texture2D whiteQueenTexture;
    private Texture2D whiteRookTexture;
    
    private readonly Color white = Color.White;
    private readonly Color blackSquareColor = Color.FromNonPremultiplied(119, 149, 86, 255);
    private readonly Color whiteSquareColor = Color.FromNonPremultiplied(235, 236, 208, 255);

    private readonly ChessBoard board = new ChessBoard();
    
    private Texture2D defaultTexture;
    
    private Piece selectedPiece;
    private SpriteBatch spriteBatch;

    private bool turn = true;
    private bool wasLeftButtonPressed;
    
    public Game1()
    {
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = 512;
        graphics.PreferredBackBufferHeight = 512;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        //setup blank board

        for (var i = 0; i < 8; i++)
        {
            // Create a new white pawn with the current pawn number and position.
            new Pawn(true, i, 6, board);
        }

        // Create eight black pawns.
        for (var i = 0; i < 8; i++)
        {
            // Create a new black pawn with the current pawn number and position.
            new Pawn(false, i, 1, board);
        }

        //initialize the rest of the pieces
        new King(true, 4, 7, board);
        new King(false, 4, 0, board);
        new Queen(true, 3, 7, board);
        new Queen(false, 3, 0, board);
        new Rook(true, 0, 7, board);
        new Rook(true, 7, 7, board);
        new Rook(false, 0, 0, board);
        new Rook(false, 7, 0, board);
        new Knight(true, 1, 7, board);
        new Knight(true, 6, 7, board);
        new Knight(false, 1, 0, board);
        new Knight(false, 6, 0, board);
        new Bishop(true, 2, 7, board);
        new Bishop(true, 5, 7, board);
        new Bishop(false, 2, 0, board);
        new Bishop(false, 5, 0, board);


        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        
        defaultTexture = Content.Load<Texture2D>("WhiteSquare");
        whitePawnTexture = Content.Load<Texture2D>("WhitePawn");
        blackPawnTexture = Content.Load<Texture2D>("BlackPawn");
        whiteKingTexture = Content.Load<Texture2D>("WhiteKing");
        blackKingTexture = Content.Load<Texture2D>("BlackKing");
        whiteQueenTexture = Content.Load<Texture2D>("WhiteQueen");
        blackQueenTexture = Content.Load<Texture2D>("BlackQueen");
        whiteBishopTexture = Content.Load<Texture2D>("WhiteBishop");
        blackBishopTexture = Content.Load<Texture2D>("BlackBishop");
        whiteKnightTexture = Content.Load<Texture2D>("WhiteKnight");
        blackKnightTexture = Content.Load<Texture2D>("BlackKnight");
        whiteRookTexture = Content.Load<Texture2D>("WhiteRook");
        blackRookTexture = Content.Load<Texture2D>("BlackRook");
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();


        if (mouseState.LeftButton == ButtonState.Pressed && !wasLeftButtonPressed)
        {
            // Get the position of the cell that the mouse is currently on
            var cellX = mouseState.X / 64;
            var cellY = mouseState.Y / 64;


            if (selectedPiece == null)
            {
                // If there's no piece currently selected, check if there's a piece at the clicked cell and select it
                var clickedPiece = board[cellX, cellY];

                if (clickedPiece != null) selectedPiece = clickedPiece;
            }
            else
            {
                // If there's a piece already selected, move it to the clicked cell
                try
                {
                    if (turn == selectedPiece.IsWhite)
                        if (selectedPiece.IsMoveValid(cellX, cellY))
                        {
                            ChessBoard.MovePiece(selectedPiece, cellX, cellY);
                            turn = !turn;
                        }
                    selectedPiece = null; // Deselect the piece after it's been moved
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        wasLeftButtonPressed = mouseState.LeftButton == ButtonState.Pressed;

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin();


        var cellSize = 64;

        for (var x = 0; x < ChessBoard.GetLengthX(); x++)
        for (var y = 0; y < ChessBoard.GetLengthY(); y++)
        {
            // Calculate the position of the cell based on its x and y index.
            var xPos = x * cellSize;
            var yPos = y * cellSize;

            // Determine the color of the cell based on its position.
            var cellColor = (x + y) % 2 == 0 ? whiteSquareColor : blackSquareColor;

            // Draw the cell with a colored border.
            spriteBatch.Draw(
                defaultTexture,
                new Rectangle(xPos, yPos, cellSize, cellSize),
                null,
                cellColor,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );
        }


        //draw the pawns onto the screen
        foreach (Piece piece in board)
            if (piece != null)
            {
                var pieceType = piece.GetPieceType();

                if (piece.IsWhite)
                    switch (pieceType)
                    {
                        case PieceType.Pawn:
                            spriteBatch.Draw(
                                whitePawnTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.King:
                            spriteBatch.Draw(
                                whiteKingTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Queen:
                            spriteBatch.Draw(
                                whiteQueenTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Bishop:
                            spriteBatch.Draw(
                                whiteBishopTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Knight:
                            spriteBatch.Draw(
                                whiteKnightTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Rook:
                            spriteBatch.Draw(
                                whiteRookTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                    }
                else
                    switch (pieceType)
                    {
                        case PieceType.Pawn:
                            spriteBatch.Draw(
                                blackPawnTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.King:
                            spriteBatch.Draw(
                                blackKingTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Queen:
                            spriteBatch.Draw(
                                blackQueenTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Bishop:
                            spriteBatch.Draw(
                                blackBishopTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Knight:
                            spriteBatch.Draw(
                                blackKnightTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case PieceType.Rook:
                            spriteBatch.Draw(
                                blackRookTexture,
                                new Rectangle(piece.PieceX * cellSize, piece.PieceY * cellSize, cellSize, cellSize),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                    }
            }

        spriteBatch.End();
        base.Draw(gameTime);
    }
}