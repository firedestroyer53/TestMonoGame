﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace ChessTest;

public class Game1 : Game
{
    Texture2D whiteSquareTexture;
    Texture2D whitePawnTexture;
    Texture2D blackPawnTexture;
    Texture2D whiteKingTexture;
    Texture2D blackKingTexture;
    Texture2D whiteQueenTexture;
    Texture2D blackQueenTexture;
    Texture2D whiteBishopTexture;
    Texture2D blackBishopTexture;
    Texture2D whiteKnightTexture;
    Texture2D blackKnightTexture;
    Texture2D whiteRookTexture;
    Texture2D blackRookTexture;


    Piece selectedPiece = null;
    private bool wasLeftButtonPressed = false;
    public List<Rectangle> validMoves = new List<Rectangle>();

    public bool turn = true;

    ChessBoard board = new ChessBoard();

    private GraphicsDeviceManager _graphics;
    private SpriteBatch spriteBatch;

    DebugTextWriter writer = new DebugTextWriter();

    Color white = Color.White;
    Color black = Color.Black;

    Color whiteSquareColor = Color.FromNonPremultiplied(235, 236, 208, 255);
    Color blackSquareColor = Color.FromNonPremultiplied(119, 149, 86, 255);
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
            Pawn whitePawn = new(true, i, 6, board);
        }
        // Create eight white pawns.
        for (int i = 0; i < 8; i++)
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
        MouseState mstate = Mouse.GetState();

        int cellX;
        int cellY;

        if (mstate.LeftButton == ButtonState.Pressed && !wasLeftButtonPressed)
        {
            // Get the position of the cell that the mouse is currently on
            cellX = mstate.X / 64;
            cellY = (mstate.Y / 64);


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
                    if(turn == selectedPiece.isWhite)
                    {
                        if (selectedPiece.IsMoveValid(cellX, cellY))
                        {
                            board.movePiece(selectedPiece, cellX, cellY);
                            turn = !turn;
                        }
                        
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


        int cellSize = 64;

        for (int x = 0; x < board.getLengthX(); x++)
        {
            for (int y = 0; y < board.getLengthY(); y++)
            {
                // Calculate the position of the cell based on its x and y index.
                int xPos = x * cellSize;
                int yPos = y * cellSize;

                // Determine the color of the cell based on its position.
                Color cellColor = ((x + y) % 2 == 0) ? whiteSquareColor : blackSquareColor;

                // Draw the cell with a colored border.
                spriteBatch.Draw(
                    texture: whiteSquareTexture,
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
                PieceType pieceType = piece.GetPieceType();

                if (piece.isWhite)
                {
                    switch (pieceType)
                    {
                        case PieceType.Pawn:
                            spriteBatch.Draw(
                                texture: whitePawnTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.King:
                            spriteBatch.Draw(
                                texture: whiteKingTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Queen:
                            spriteBatch.Draw(
                                texture: whiteQueenTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Bishop:
                            spriteBatch.Draw(
                                texture: whiteBishopTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Knight:
                            spriteBatch.Draw(
                                texture: whiteKnightTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Rook:
                            spriteBatch.Draw(
                                texture: whiteRookTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                    }
                    

                }
                else
                {
                    switch (pieceType)
                    {
                        case PieceType.Pawn:
                            spriteBatch.Draw(
                                texture: blackPawnTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.King:
                            spriteBatch.Draw(
                                texture: blackKingTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Queen:
                            spriteBatch.Draw(
                                texture: blackQueenTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Bishop:
                            spriteBatch.Draw(
                                texture: blackBishopTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Knight:
                            spriteBatch.Draw(
                                texture: blackKnightTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                        case PieceType.Rook:
                            spriteBatch.Draw(
                                texture: blackRookTexture,
                                destinationRectangle: new Rectangle((piece.pieceX) * cellSize, (piece.pieceY) * cellSize, cellSize, cellSize),
                                sourceRectangle: null,
                                color: white,
                                rotation: 0f,
                                origin: Vector2.Zero,
                                effects: SpriteEffects.None,
                                layerDepth: 0f
                            );
                            break;
                    }
                }
            }


        }
        foreach (Rectangle rect in validMoves)
        {
            spriteBatch.Draw(whiteSquareTexture, rect, Color.Red);
        }


        spriteBatch.End();
        base.Draw(gameTime);
    }
}