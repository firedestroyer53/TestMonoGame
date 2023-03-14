using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessTest;

public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private Color black = Color.Black;
    private Texture2D blackBishopTexture;
    private Texture2D blackKingTexture;
    private Texture2D blackKnightTexture;
    private Texture2D blackPawnTexture;
    private Texture2D blackQueenTexture;
    private Texture2D blackRookTexture;
    private readonly Color blackSquareColor = Color.FromNonPremultiplied(119, 149, 86, 255);

    private readonly ChessBoard board = new();


    private Piece selectedPiece;
    private SpriteBatch spriteBatch;

    public bool turn = true;
    public List<Rectangle> validMoves = new();
    private bool wasLeftButtonPressed;

    private readonly Color white = Color.White;
    private Texture2D whiteBishopTexture;
    private Texture2D whiteKingTexture;
    private Texture2D whiteKnightTexture;
    private Texture2D whitePawnTexture;
    private Texture2D whiteQueenTexture;
    private Texture2D whiteRookTexture;

    private readonly Color whiteSquareColor = Color.FromNonPremultiplied(235, 236, 208, 255);
    private Texture2D whiteSquareTexture;

    private DebugTextWriter writer = new();

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
        for (var i = 0; i < 8; i++)
        {
            // Create a new black pawn with the current pawn number and position.
            Pawn whitePawn = new(true, i, 6, board);
        }

        // Create eight white pawns.
        for (var i = 0; i < 8; i++)
        {
            // Create a new white pawn with the current pawn number and position.
            Pawn blackPawn = new(false, i, 1, board);
        }

        //initialize the rest of the pieces
        King whiteKing = new(true, 4, 7, board);
        King blackKing = new(false, 4, 0, board);
        Queen whiteQueen = new(true, 3, 7, board);
        Queen blackQueen = new(false, 3, 0, board);
        Rook whiteRook1 = new(true, 0, 7, board);
        Rook whiteRook2 = new(true, 7, 7, board);
        Rook blackRook1 = new(false, 0, 0, board);
        Rook blackRook2 = new(false, 7, 0, board);
        Knight whiteKnight1 = new(true, 1, 7, board);
        Knight whiteKnight2 = new(true, 6, 7, board);
        Knight blackKnight1 = new(false, 1, 0, board);
        Knight blackKnight2 = new(false, 6, 0, board);
        Bishop whiteBishop1 = new(true, 2, 7, board);
        Bishop whiteBishop2 = new(true, 5, 7, board);
        Bishop blackBishop1 = new(false, 2, 0, board);
        Bishop blackBishop2 = new(false, 5, 0, board);


        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        whiteSquareTexture = Content.Load<Texture2D>("WhiteSquare");
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
        var mstate = Mouse.GetState();

        int cellX;
        int cellY;

        if (mstate.LeftButton == ButtonState.Pressed && !wasLeftButtonPressed)
        {
            // Get the position of the cell that the mouse is currently on
            cellX = mstate.X / 64;
            cellY = mstate.Y / 64;


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
                    if (turn == selectedPiece.isWhite)
                        if (selectedPiece.IsMoveValid(cellX, cellY))
                        {
                            board.movePiece(selectedPiece, cellX, cellY);
                            turn = !turn;
                            board.lastMoved = selectedPiece;
                        }

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


        var cellSize = 64;

        for (var x = 0; x < board.getLengthX(); x++)
        for (var y = 0; y < board.getLengthY(); y++)
        {
            // Calculate the position of the cell based on its x and y index.
            var xPos = x * cellSize;
            var yPos = y * cellSize;

            // Determine the color of the cell based on its position.
            var cellColor = (x + y) % 2 == 0 ? whiteSquareColor : blackSquareColor;

            // Draw the cell with a colored border.
            spriteBatch.Draw(
                whiteSquareTexture,
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

                if (piece.isWhite)
                    switch (pieceType)
                    {
                        case PieceType.Pawn:
                            spriteBatch.Draw(
                                whitePawnTexture,
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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
                                new Rectangle(piece.pieceX * cellSize, piece.pieceY * cellSize, cellSize, cellSize),
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

        foreach (var rect in validMoves) spriteBatch.Draw(whiteSquareTexture, rect, Color.Red);


        spriteBatch.End();
        base.Draw(gameTime);
    }
}