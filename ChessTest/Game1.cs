using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
// ReSharper disable ObjectCreationAsStatement

// ReSharper disable InconsistentNaming

namespace ChessTest;

[SuppressMessage("Performance", "CA1806:Do not ignore method results")]
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
    private Texture2D defaultTexture;

    private readonly List<SoundEffect> soundEffects = new();
    
    private readonly Color white = Color.White;
    private readonly Color blackSquareColor = Color.FromNonPremultiplied(119, 149, 86, 255);
    private readonly Color whiteSquareColor = Color.FromNonPremultiplied(235, 236, 208, 255);

    private readonly ChessBoard board = new();
    
    public static readonly List<Tuple<Piece, Vector2>> ValidMoves = new();
    private readonly List<Vector2> possibleMoves = new();
    private readonly List<Vector2> highlightedSquares = new();
    private Piece selectedPiece;
    private SpriteBatch spriteBatch;

    private bool turn = true;
    private bool wasLeftButtonPressed;
    private bool wasRightButtonPressed;
    
    private const int cellSize = 64;

    public Game1()
    {
        var graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        graphics.PreferredBackBufferWidth = cellSize*8;
        graphics.PreferredBackBufferHeight = cellSize*8;
        graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        //setup blank board
        var pieces = new List<Piece>();
        for (var i = 0; i < 8; i++)
        {
            // Create a new white pawn with the current pawn number and position.
            pieces.Add(new Pawn(true, i, 6, board));
        }

        // Create eight black pawns.
        for (var i = 0; i < 8; i++)
        {
            // Create a new black pawn with the current pawn number and position.
            pieces.Add(new Pawn(false, i, 1, board));
        }

        //initialize the rest of the pieces
        pieces.Add(new King(true, 4, 7, board)); //White King
        pieces.Add(new King(false, 4, 0, board)); //Black King
        pieces.Add(new Queen(true, 3, 7, board)); //White Queen
        pieces.Add(new Queen(false, 3, 0, board)); //Black Queen
        pieces.Add(new Rook(true, 0, 7, board)); //White Rook
        pieces.Add(new Rook(true, 7, 7, board)); //White Rook
        pieces.Add(new Rook(false, 0, 0, board)); //Black Rook
        pieces.Add(new Rook(false, 7, 0, board)); //Black Rook
        pieces.Add(new Knight(true, 1, 7, board)); //White Knight
        pieces.Add(new Knight(true, 6, 7, board)); //White Knight
        pieces.Add(new Knight(false, 1, 0, board)); //Black Knight
        pieces.Add(new Knight(false, 6, 0, board)); //Black Knight
        pieces.Add(new Bishop(true, 2, 7, board)); //White Bishop
        pieces.Add(new Bishop(true, 5, 7, board)); //White Bishop
        pieces.Add(new Bishop(false, 2, 0, board)); //Black Bishop
        pieces.Add(new Bishop(false, 5, 0, board)); //Black Bishop

        foreach (var piece in pieces)
        {
            board.PlacePiece(piece);
        }
        
        foreach (Piece piece in board)
        {
            if (piece == null) continue;
            for(var x = 0; x<8; x++)
            for(var y = 0; y<8; y++)
            {
                if(piece.testIsMoveValid(x, y))
                {
                    ValidMoves.Add(new Tuple<Piece, Vector2>(piece, new Vector2(x, y)));
                }
            }
        }
        
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
        soundEffects.Add(Content.Load<SoundEffect>("MoveSound"));
        soundEffects.Add(Content.Load<SoundEffect>("CaptureSound"));
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        var cellX = mouseState.X / cellSize;
        var cellY = mouseState.Y / cellSize;

        if (mouseState.LeftButton == ButtonState.Pressed && !wasLeftButtonPressed)
        {
            // Get the position of the cell that the mouse is currently on
            highlightedSquares.Clear();
            
            if (selectedPiece == null || !selectedPiece.testIsMoveValid(cellX, cellY))
            {
                // If there's no piece currently selected, check if there's a piece at the clicked cell and select it
                var clickedPiece = board[cellX, cellY];
                if (clickedPiece == null || clickedPiece != selectedPiece)
                {
                    possibleMoves.Clear();
                }
                selectedPiece = clickedPiece;
                // add possible moves for selected piece to possibleMoves
                if (selectedPiece != null)
                {
                    for(var x = 0; x < 8; x++)
                    for(var y = 0; y < 8; y++)
                    {
                        if (!selectedPiece.testIsMoveValid(x, y)) continue;
                        // check if already in possibleMoves
                        if (!possibleMoves.Contains(new Vector2(x, y))) possibleMoves.Add(new Vector2(x, y));
                    }
                }
            }
            else
            {
                // If there's a piece already selected, move it to the clicked cell
                try
                {
                    if (turn == selectedPiece.IsWhite)
                        if (selectedPiece.IsMoveValid(cellX, cellY))
                        {
                            // If move was an en passant, play the capture sound effect
                            if (board[cellX, cellY] != null || (selectedPiece is Pawn && Math.Abs(cellY - selectedPiece.PieceY) == 2))
                                soundEffects[1].Play();
                            else
                                soundEffects[0].Play();
                            board.MovePiece(selectedPiece, cellX, cellY);
                            board.LastMovedPiece = selectedPiece;
                            turn = !turn;
                            ValidMoves.Clear();
                            foreach(Piece piece in board)
                            {
                                if (piece == null) continue;
                                if (piece.IsWhite != turn) continue;
                                for(var x = 0; x<8; x++)
                                for(var y = 0; y<8; y++)
                                {
                                    if(piece.testIsMoveValid(x, y))
                                    {
                                        ValidMoves.Add(new Tuple<Piece, Vector2>(piece, new Vector2(x,y)));
                                    }
                                }
                            }
                        }
                    selectedPiece = null; // Deselect the piece after it's been moved
                    possibleMoves.Clear();
                    ValidMoves.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        if (mouseState.RightButton == ButtonState.Pressed && !wasRightButtonPressed)
        {
            // Check if already highlighted before adding
            if (!highlightedSquares.Contains(new Vector2(cellX, cellY)))
                highlightedSquares.Add(new Vector2(cellX, cellY));
            else highlightedSquares.Remove(new Vector2(cellX, cellY));
        }

        wasLeftButtonPressed = mouseState.LeftButton == ButtonState.Pressed;
        wasRightButtonPressed = mouseState.RightButton == ButtonState.Pressed;

        base.Update(gameTime);
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin();
        
        for (var x = 0; x < board.GetLengthX(); x++)
        for (var y = 0; y < board.GetLengthY(); y++)
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

        

        foreach (var square in highlightedSquares)
        {
            spriteBatch.Draw(
                defaultTexture,
                new Rectangle((int)square.X * cellSize, (int)square.Y * cellSize, cellSize, cellSize),
                null,
                Color.FromNonPremultiplied(255,0,0,125),
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );
        }
        //draw the pieces onto the screen
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
        
        foreach (var move in possibleMoves)
        {
            spriteBatch.Draw(
                defaultTexture,
                new Rectangle((int)move.X * cellSize + cellSize / 4, (int)move.Y * cellSize + cellSize / 4, cellSize / 2, cellSize / 2),
                null,
                Color.FromNonPremultiplied(214,214,189,125),
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0f
            );
        }
        
        spriteBatch.End();
        base.Draw(gameTime);
    }
}