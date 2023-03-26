using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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
    private Texture2D ratTexture;
    private Texture2D defaultTexture;

    private readonly List<SoundEffect> soundEffects = new();
    
    private readonly Color white = Color.White;
    private readonly Color blackSquareColor = Color.FromNonPremultiplied(81,42,42,255);
    private readonly Color whiteSquareColor = Color.FromNonPremultiplied(124,76,62,255);

    private readonly ChessBoard board = new();
    
    public static readonly List<Tuple<Piece, Vector2>> ValidMoves = new();
    private readonly List<Vector2> possibleMoves = new();
    private readonly List<Vector2> highlightedSquares = new();
    private List<Tuple<Piece, Vector2>> moveHistory = new();
    
    private readonly List<Piece> pieces = new();

    private Piece selectedPiece;
    private SpriteBatch spriteBatch;

    private bool turn = true;
    private bool wasLeftButtonPressed;
    private bool wasRightButtonPressed;
    
    private const int cellSize = 96;

    
    
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
        
// Create a blank board and initialize the pieces
        for (var i = 0; i < 8; i++)
        {
            pieces.Add(new Pawn(true, i, 6, board)); // white pawns
            pieces.Add(new Pawn(false, i, 1, board)); // black pawns
        }

        pieces.AddRange(new Piece[]
        {
            new King(true, 4, 7, board),    // white king
            new King(false, 4, 0, board),   // black king
            new Queen(true, 3, 7, board),   // white queen
            new Queen(false, 3, 0, board),  // black queen
            new Rook(true, 0, 7, board),    // white rooks
            new Rook(true, 7, 7, board),
            new Rook(false, 0, 0, board),   // black rooks
            new Rook(false, 7, 0, board),
            new Knight(true, 1, 7, board),  // white knights
            new Knight(true, 6, 7, board),
            new Knight(false, 1, 0, board), // black knights
            new Knight(false, 6, 0, board),
            new Bishop(true, 2, 7, board),  // white bishops
            new Bishop(true, 5, 7, board),
            new Bishop(false, 2, 0, board), // black bishops
            new Bishop(false, 5, 0, board),
        });

        pieces.ForEach(piece => board.PlacePiece(piece)); // place pieces on the board

        foreach (Piece piece in board)
        {
            if (piece == null) continue;
            for(var x = 0; x<8; x++)
            for(var y = 0; y<8; y++)
            {
                if(piece.TestIsMoveValid(x, y))
                {
                    ValidMoves.Add(new Tuple<Piece, Vector2>(piece, new Vector2(x, y)));
                }
            }
        }
        Window.Title = "Chess";

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
        ratTexture = Content.Load<Texture2D>("Rat");
        
        soundEffects.Add(Content.Load<SoundEffect>("MoveSound"));
        soundEffects.Add(Content.Load<SoundEffect>("CaptureSound"));
    }

    protected override void Update(GameTime gameTime)
    {
        // Get the position of the cell that the mouse is currently on
        var mouseState = Mouse.GetState();
        var cellX = mouseState.X / cellSize;
        var cellY = mouseState.Y / cellSize;

        if (mouseState.LeftButton == ButtonState.Pressed && !wasLeftButtonPressed)
        {
            highlightedSquares.Clear();
            
            if (selectedPiece == null || !selectedPiece.TestIsMoveValid(cellX, cellY) && selectedPiece.IsWhite == turn)
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
                        if (!selectedPiece.TestIsMoveValid(x, y)) continue;
                        if (turn != selectedPiece.IsWhite) continue;
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
                            if (board[cellX, cellY] != null)
                                soundEffects[1].Play();
                            else
                                soundEffects[0].Play();
                            board.MovePiece(selectedPiece, cellX, cellY, board);
                            
                            board.LastMovedPiece = selectedPiece;
                            moveHistory.Add(new Tuple<Piece, Vector2>(selectedPiece, new Vector2(cellX, cellY)));
                            turn = !turn;
                            
                            ValidMoves.Clear();
                            foreach(Piece piece in board)
                            {
                                if (piece == null) continue;
                                for(var x = 0; x<8; x++)
                                for(var y = 0; y<8; y++)
                                {
                                    if(piece.TestIsMoveValid(x, y))
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
        
        // when a piece is selected, highlight the piece
        if (selectedPiece != null && selectedPiece.IsWhite == turn)
        {
            spriteBatch.Draw(
                defaultTexture,
                new Rectangle(selectedPiece.PieceX * cellSize, selectedPiece.PieceY * cellSize, cellSize, cellSize),
                null,
                Color.FromNonPremultiplied(0,255,0,125),
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
                        case { } when pieceType == typeof(Pawn):
                            spriteBatch.Draw(
                                whitePawnTexture,
                                new Rectangle(piece.PieceX * cellSize + 12, piece.PieceY * cellSize + 10, cellSize - 24, cellSize - 20),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(King):
                            spriteBatch.Draw(
                                whiteKingTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Queen):
                            spriteBatch.Draw(
                                whiteQueenTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Bishop):
                            spriteBatch.Draw(
                                whiteBishopTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Knight):
                            spriteBatch.Draw(
                                whiteKnightTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Rook):
                            spriteBatch.Draw(
                                whiteRookTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Rat):
                            spriteBatch.Draw(
                                ratTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
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
                        case { } when pieceType == typeof(Pawn):
                            spriteBatch.Draw(
                                blackPawnTexture,
                                new Rectangle(piece.PieceX * cellSize + 12, piece.PieceY * cellSize + 10, cellSize - 24, cellSize - 20),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(King):
                            spriteBatch.Draw(
                                blackKingTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Queen):
                            spriteBatch.Draw(
                                blackQueenTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Bishop):
                            spriteBatch.Draw(
                                blackBishopTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Knight):
                            spriteBatch.Draw(
                                blackKnightTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
                                null,
                                white,
                                0f,
                                Vector2.Zero,
                                SpriteEffects.None,
                                0f
                            );
                            break;
                        case { } when pieceType == typeof(Rook):
                            spriteBatch.Draw(
                                blackRookTexture,
                                new Rectangle(piece.PieceX * cellSize + 6, piece.PieceY * cellSize + 6, cellSize - 12, cellSize - 12),
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