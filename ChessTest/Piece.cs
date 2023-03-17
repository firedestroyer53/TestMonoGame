using Microsoft.Xna.Framework;
using System;

namespace ChessTest;

public enum PieceType
{
    Pawn,
    King,
    Queen,
    Bishop,
    Knight,
    Rook
}

public abstract class Piece
{
    // Template for a chess piece class
    public readonly bool IsWhite;
    public int PieceX;
    public int PieceY;

    protected bool JustMoved;
    //Position vector of X and Y
    private readonly Vector2 position;
    private readonly ChessBoard board;
        

    // getPiece method which returns what type of piece it is
    public abstract PieceType GetPieceType();


    // Class constructor
    protected Piece(bool iswhite, int x, int y, ChessBoard board)
    {
        this.IsWhite = iswhite;
        this.PieceX = x;
        this.PieceY = y;
        this.position = new Vector2(x, y);
        this.board = board;
        this.board.PlacePiece(this);
        this.JustMoved = false;
    }

    // Get bounds method
    public Rectangle GetBounds()
    {
        return new Rectangle((int)position.X, (int)position.Y, 64, 64);
    }

    protected abstract bool IsValidMove(int newX, int newY, ChessBoard board);
    public bool IsMoveValid(int newX, int newY)
    {
        // Check if the move is within the bounds of the board
        if (newX is < 0 or > 8 || newY is < 0 or > 8)
        {
            return false;
        }

        // Check if there is a piece at the destination 
        var pieceAtDestination = board[newX, newY];
        if (pieceAtDestination != null && pieceAtDestination.IsWhite == IsWhite)
        {
            return false;
        }

        // Check if the move goes through any pieces on the board
        if (GetPieceType() == PieceType.Knight) return IsValidMove(newX, newY, board);
        var dx = newX - PieceX;
        var dy = newY - PieceY;
        var steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
        var xStep = dx / steps;
        var yStep = dy / steps;
        for (var i = 1; i < steps; i++)
        {
            var x = PieceX + i * xStep;
            var y = PieceY + i * yStep;
            if (board[x, y] != null)
            {
                return false;
            }
        }

        return IsValidMove(newX, newY, board);
    }


}

public class Pawn : Piece
{
    private bool HasMoved { get; set; }
    public override PieceType GetPieceType()
    {
        return PieceType.Pawn;
    }
    public Pawn(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
        JustMoved = false;
        HasMoved = false;
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is one square forward, or two squares forward if the pawn has not moved yet
        var dy = newY - PieceY;
        var dx = newX - PieceX;
        switch (dy)
        {
            // white pawn moving one square forward
            case -1 when newX == PieceX && board[newX, newY] == null:
                HasMoved = true;
                JustMoved = false;
                return true;
            // white pawn moving two squares forward from starting position
            case -2 when HasMoved == false && newX == PieceX && board[newX, newY] == null:
                JustMoved = true;
                HasMoved = true;
                return true;
            // black pawn moving one square forward
            case 1 when newX == PieceX && board[newX, newY] == null:
                HasMoved = true;
                JustMoved = false;
                return true;
            // black pawn moving two squares forward from starting position
            case 2 when HasMoved == false && newX == PieceX && board[newX, newY] == null:
                JustMoved = true;
                HasMoved = true;
                return true;
        }

        if (board[newX,newY] != null)
        {
            switch (dx)
            {
                // white pawn capturing to the right
                case 1 when dy == -1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    JustMoved = false;
                    return true;
                // white pawn capturing to the left
                case -1 when dy == -1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    JustMoved = false;
                    return true;
                // black pawn capturing to the right
                case 1 when dy == 1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    JustMoved = false;
                    return true;
                // black pawn capturing to the left
                case -1 when dy == 1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    JustMoved = false;
                    return true;
            }
        }
        switch (JustMoved)
        {
            // white pawn en passant
            case true when board[newX, newY] == null && dx == 1 && dy == -1 && board[newX, newY + 1] != null 
                           && board[newX, newY + 1].IsWhite != IsWhite && board[newX, newY + 1].GetPieceType() == PieceType.Pawn:
                board[newX, newY + 1] = null;
                HasMoved = true;
                JustMoved = false;
                return true;
            // white pawn en passant
            case true when board[newX, newY] == null && dx == -1 && dy == -1 && board[newX, newY + 1] != null 
                           && board[newX, newY + 1].IsWhite != IsWhite && board[newX, newY + 1].GetPieceType() == PieceType.Pawn:
                board[newX, newY + 1] = null;
                HasMoved = true;
                JustMoved = false;
                return true;
            // black pawn en passant
            case true when board[newX, newY] == null && dx == 1 && dy == 1 && board[newX, newY - 1] != null 
                           && board[newX, newY - 1].IsWhite != IsWhite && board[newX, newY - 1].GetPieceType() == PieceType.Pawn:
                board[newX, newY - 1] = null;
                HasMoved = true;
                JustMoved = false;
                return true;
            // black pawn en passant
            case true when board[newX, newY] == null && dx == -1 && dy == 1 && board[newX, newY - 1] != null 
                           && board[newX, newY - 1].IsWhite != IsWhite && board[newX, newY - 1].GetPieceType() == PieceType.Pawn:
                board[newX, newY - 1] = null;
                HasMoved = true;
                JustMoved = false;
                return true;
            default:
                return false;
        }
    }
}

public class King : Piece
{
    public King(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override PieceType GetPieceType()
    {
        return PieceType.King;
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is within one square in any direction
        return Math.Abs(newX - PieceX) <= 1 && Math.Abs(newY - PieceY) <= 1;
    }
}

public class Queen : Piece
{
    public Queen(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override PieceType GetPieceType()
    {
        return PieceType.Queen;
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is along a straight line in any direction
        return newX == PieceX || newY == PieceY || Math.Abs(newX - PieceX) == Math.Abs(newY - PieceY);
    }
}

public class Rook : Piece
{
    public Rook(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override PieceType GetPieceType()
    {
        return PieceType.Rook;
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is along a straight line
        return newX == PieceX || newY == PieceY;
    }
}

public class Bishop : Piece
{
    public Bishop(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override PieceType GetPieceType()
    {
        return PieceType.Bishop;
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is along a diagonal line
        return Math.Abs(newX - PieceX) == Math.Abs(newY - PieceY);
    }
}

public class Knight : Piece
{
    public Knight(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override PieceType GetPieceType()
    {
        return PieceType.Knight;
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is in an L-shape
        var dx = Math.Abs(newX - PieceX);
        var dy = Math.Abs(newY - PieceY);
        return (dx == 1 && dy == 2) || (dx == 2 && dy == 1);
    }
}