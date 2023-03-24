using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Xna.Framework;

namespace ChessTest;

public class ChessBoard : IEnumerable
{
    // class for a chessboard

    
    public Piece[,] Board { get; private set; }
    
    public Piece LastMovedPiece { get; set; }
    public void PlacePiece(Piece piece)
    {
        // method to add a piece to the board

        Board[piece.PieceX, piece.PieceY] = piece;
    }
    
    //support to use foreach loop on chessboard
    public IEnumerator GetEnumerator()
    {
        return Board.Cast<Piece>().GetEnumerator();
    }

    public void MovePiece(Piece piece, int x, int y)
    {
        // method to move a piece on the board
        Board[piece.PieceX, piece.PieceY] = null;

        piece.PieceX = x;
        piece.PieceY = y;
        Board[piece.PieceX, piece.PieceY] = piece;
        
    }

    // isInCheck method here
    public bool IsInCheck(bool isWhite)
    {
        var king = GetKing(isWhite);
        if (king == null)
        {
            return false;
        }

        foreach (Piece piece in this)
        {
            Vector2 kingCoords = new(king.PieceX, king.PieceY);
            if (piece != null && piece.IsWhite != isWhite &&
                Game1.ValidMoves.Contains(new Tuple<Piece, Vector2>(piece, kingCoords)))
            {
                return true;
            }
        }

        return false;
    }
    
    // get length of x function here
    public int GetLengthX()
    {
        return Board.GetLength(0);
    }

    // get length of y function here
    public int GetLengthY()
    {
        return Board.GetLength(1);
    }

    // array2D[x, y]
    public Piece this[int x, int y]
    {
        get
        {
            if (x >= 0 && x < Board.GetLength(0) && y >= 0 && y < Board.GetLength(1))
            {
                return Board[x, y];
            }
            return null;
        }
        set
        {
            if (x >= 0 && x < Board.GetLength(0) && y >= 0 && y < Board.GetLength(1))
            {
                Board[x, y] = value;
            }
        }
    }
    
    //board.GetKing()
    public Piece GetKing(bool isWhite)
    {
        foreach (var piece in this)
        {
            if (piece is King king && king.IsWhite == isWhite)
            {
                return king;
            }
        }
        return null;
    }
    
    // class constructor here
    public ChessBoard()
    {
        // initialize the board
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                Board[i, j] = null;
            }
        }
    }

    
}