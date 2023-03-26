using System;
// ReSharper disable InvertIf

namespace ChessTest;

public abstract class Piece
{
    // Template for a chess piece class
    public readonly bool IsWhite;

    public int PieceX;
    public int PieceY;
    private readonly ChessBoard _board;

    public bool HasMoved;
    public bool JustMoved;

    // getPiece method which returns what type of piece it is
    public abstract Type GetPieceType();
    
    // Class constructor
    protected Piece(bool iswhite, int x, int y, ChessBoard board)
    {
        IsWhite = iswhite;
        PieceX = x;
        PieceY = y;
        _board = board;
    }
    
    protected abstract bool IsValidMove(int newX, int newY, ChessBoard board);
    protected abstract bool TestIsValidMove(int newX, int newY, ChessBoard board);
    public bool TestIsMoveValid(int newX, int newY)
    {
        // Check if the move is within the bounds of the board
        if (newX is < 0 or > 8 || newY is < 0 or > 8)
        {
            return false;
        }

        // Check if there is a piece at the destination 
        var pieceAtDestination = _board[newX, newY];
        if (pieceAtDestination != null && pieceAtDestination.IsWhite == IsWhite)
        {
            return false;
        }
        
        // Check if the move goes through any pieces on the board
        if (GetPieceType() == typeof(Knight)) return TestIsValidMove(newX, newY, _board);
        
        var dx = newX - PieceX;
        var dy = newY - PieceY;
        var steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
        var xStep = dx / steps;
        var yStep = dy / steps;
        for (var i = 1; i < steps; i++)
        {
            var x = PieceX + i * xStep;
            var y = PieceY + i * yStep;
            if (_board[x, y] != null)
            {
                return false;
            }
        }

        return TestIsValidMove(newX, newY, _board);
    }
    public bool IsMoveValid(int newX, int newY)
    {
        // Check if the move is within the bounds of the board
        if (newX is < 0 or > 8 || newY is < 0 or > 8)
        {
            return false;
        }

        // Check if there is a piece at the destination 
        var pieceAtDestination = _board[newX, newY];
        if (pieceAtDestination != null && pieceAtDestination.IsWhite == IsWhite)
        {
            return false;
        }
        
        // Check if the move goes through any pieces on the board
        if (GetPieceType() == typeof(Knight)) return IsValidMove(newX, newY, _board);
        
        var dx = newX - PieceX;
        var dy = newY - PieceY;
        var steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
        var xStep = dx / steps;
        var yStep = dy / steps;
        for (var i = 1; i < steps; i++)
        {
            var x = PieceX + i * xStep;
            var y = PieceY + i * yStep;
            if (_board[x, y] != null)
            {
                return false;
            }
        }

        return IsValidMove(newX, newY, _board);
    }
}

public class Pawn : Piece
{
    public override Type GetPieceType()
    {
        return typeof(Pawn);
    }
    public Pawn(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
        JustMoved = HasMoved = false;
    }

    protected override bool TestIsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is one square forward, or two squares forward if the pawn has not moved yet
        var dy = newY - PieceY;
        var dx = newX - PieceX;
        switch (dy)
        {
            // white pawn moving one square forward
            case -1 when newX == PieceX && board[newX, newY] == null && IsWhite:
                return true;
            // white pawn moving two squares forward from starting position
            case -2 when HasMoved == false && newX == PieceX && board[newX, newY] == null && IsWhite:
                return true;
            // black pawn moving one square forward
            case 1 when newX == PieceX && board[newX, newY] == null && !IsWhite:
                return true;
            // black pawn moving two squares forward from starting position
            case 2 when HasMoved == false && newX == PieceX && board[newX, newY] == null && !IsWhite:
                return true;

        }

        if (board[newX, newY] != null)
        {
            switch (dx)
            {
                // white pawn capturing to the right
                case 1 when dy == -1 && board[newX, newY].IsWhite != IsWhite && IsWhite:
                    return true;
                // white pawn capturing to the left
                case -1 when dy == -1 && board[newX, newY].IsWhite != IsWhite && IsWhite:
                    return true;
                // black pawn capturing to the right
                case 1 when dy == 1 && board[newX, newY].IsWhite != IsWhite && !IsWhite:
                    return true;
                // black pawn capturing to the left
                case -1 when dy == 1 && board[newX, newY].IsWhite != IsWhite && !IsWhite:
                    return true;
            }
        }

        switch (IsWhite)
        {
            case true when Math.Abs(dx) + Math.Abs(dy) == 2 && board[newX, newY] == null &&
                           board[newX, newY + 1] != null &&
                           board[newX, newY + 1].GetPieceType() == typeof(Pawn) &&
                           board.LastMovedPiece == board[newX, newY + 1] && board[newX, newY + 1].IsWhite != IsWhite && board[newX, newY + 1].JustMoved:
                return true;
            case false when Math.Abs(dx) + Math.Abs(dy) == 2 && board[newX, newY] == null &&
                            board[newX, newY - 1] != null &&
                            board[newX, newY - 1].GetPieceType() == typeof(Pawn) &&
                            board.LastMovedPiece == board[newX, newY - 1] && board[newX, newY - 1].IsWhite != IsWhite && board[newX, newY - 1].JustMoved:
                return true;
            default:
                return false;
        }
    }

    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is one square forward, or two squares forward if the pawn has not moved yet
        var dy = newY - PieceY;
        var dx = newX - PieceX;
        switch (dy)
        {
            // white pawn moving one square forward
            case -1 when newX == PieceX && board[newX, newY] == null && IsWhite:
                HasMoved = true;
                return true;
            // white pawn moving two squares forward from starting position
            case -2 when HasMoved == false && newX == PieceX && board[newX, newY] == null && IsWhite:
                JustMoved = true;
                HasMoved = true;
                return true;
            // black pawn moving one square forward
            case 1 when newX == PieceX && board[newX, newY] == null && !IsWhite:
                HasMoved = true;
                return true;
            // black pawn moving two squares forward from starting position
            case 2 when HasMoved == false && newX == PieceX && board[newX, newY] == null && !IsWhite:
                JustMoved = true;
                HasMoved = true;
                return true;
        }

        if (board[newX, newY] != null)
        {
            switch (dx)
            {
                // white pawn capturing to the right
                case 1 when dy == -1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    return true;
                // white pawn capturing to the left
                case -1 when dy == -1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    return true;
                // black pawn capturing to the right
                case 1 when dy == 1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    return true;
                // black pawn capturing to the left
                case -1 when dy == 1 && board[newX, newY].IsWhite != IsWhite:
                    HasMoved = true;
                    return true;
            }
        }

        switch (IsWhite)
        {
            //white pawn en passant
            case true when Math.Abs(dx) + Math.Abs(dy) == 2 && board[newX, newY] == null &&
                           board[newX, newY + 1] != null &&
                           board[newX, newY + 1].GetPieceType() == typeof(Pawn) &&
                           board.LastMovedPiece == board[newX, newY + 1] && board[newX, newY + 1].IsWhite != IsWhite:
                board[newX, newY + 1] = null;
                HasMoved = true;
                return true;
            case false when Math.Abs(dx) + Math.Abs(dy) == 2 && board[newX, newY] == null &&
                            board[newX, newY - 1] != null &&
                            board[newX, newY - 1].GetPieceType() == typeof(Pawn) &&
                            board.LastMovedPiece == board[newX, newY - 1] && board[newX, newY - 1].IsWhite != IsWhite:
                board[newX, newY - 1] = null;
                HasMoved = true;
                return true;
            default:
                return false;
        }
    }
}

public class King : Piece
{
    private bool _hasMoved;
    public King(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
        _hasMoved = false;
    }

    public override Type GetPieceType()
    {
        return typeof(King);
    }

    protected override bool TestIsValidMove(int newX, int newY, ChessBoard board)
    {
                //Check for castling
        if (!_hasMoved && newX == PieceX + 2 && newY == PieceY && board[newX, newY] == null && board[newX - 1, newY] == null)
        {
            if (board[newX + 1, newY] != null && board[newX + 1, newY].GetPieceType() == typeof(Rook) && 
                board[newX + 1, newY].IsWhite == IsWhite && !board[newX + 1, newY].HasMoved)
            {
                return true;
            }
        }

        if (!_hasMoved && newX == PieceX - 2 && newY == PieceY && board[newX, newY] == null &&
            board[newX - 1, newY] == null)
        {
            if (board[newX - 2, newY] != null && board[newX - 2, newY].GetPieceType() == typeof(Rook) &&
                board[newX - 2, newY].IsWhite == IsWhite && !board[newX - 2, newY].HasMoved)
            {
                return true;
            }
        }
        return Math.Abs(newX - PieceX) <= 1 && Math.Abs(newY - PieceY) <= 1;

    }
    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        //Check for castling
        if (!_hasMoved && newX == PieceX + 2 && newY == PieceY && board[newX, newY] == null &&
            board[newX - 1, newY] == null)
        {
            if (board[newX + 1, newY] != null && board[newX + 1, newY].GetPieceType() == typeof(Rook) &&
                board[newX + 1, newY].IsWhite == IsWhite && board[newX + 1, newY].HasMoved == false)
            {
                board[newX + 1, newY] = null;
                var rook =new Rook(IsWhite, newX - 1, newY, board);
                board.PlacePiece(rook);
                board[newX - 1, newY].HasMoved = true;
                _hasMoved = true;
                return true;
            }
        }
        //queen side castling
        if (!_hasMoved && newX == PieceX - 2 && newY == PieceY && board[newX, newY] == null &&
            board[newX - 1, newY] == null)
        {
            if (board[newX - 2, newY] != null && board[newX - 2, newY].GetPieceType() == typeof(Rook) &&
                board[newX - 2, newY].IsWhite == IsWhite && !board[newX - 2, newY].HasMoved)
            {
                board[newX - 2, newY] = null;
                var rook = new Rook(IsWhite, newX + 1, newY, board);
                board.PlacePiece(rook);
                board[newX + 1, newY].HasMoved = true;
                _hasMoved = true;
                return true;
            }
        }

        return Math.Abs(newX - PieceX) <= 1 && Math.Abs(newY - PieceY) <= 1;
    }
}

public class Queen : Piece
{
    public Queen(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override Type GetPieceType()
    {
        return typeof(Queen);
    }

    protected override bool TestIsValidMove(int newX, int newY, ChessBoard board)
    {
        return newX == PieceX || newY == PieceY || Math.Abs(newX - PieceX) == Math.Abs(newY - PieceY);
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
        HasMoved = false;
    }

    public override Type GetPieceType()
    {
        return typeof(Rook);
    }

    protected override bool TestIsValidMove(int newX, int newY, ChessBoard board)
    {
        return newX == PieceX || newY == PieceY;
    }
    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is along a straight line
        if (newX == PieceX || newY == PieceY)
        {
            HasMoved = true;
            return true;
        }
        return false;
    }
}

public class Bishop : Piece
{
    public Bishop(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
    {
    }

    public override Type GetPieceType()
    {
        return typeof(Bishop);
    }

    protected override bool TestIsValidMove(int newX, int newY, ChessBoard board)
    {
        return Math.Abs(newX - PieceX) == Math.Abs(newY - PieceY);
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

    public override Type GetPieceType()
    {
        return typeof(Knight);
    }

    protected override bool TestIsValidMove(int newX, int newY, ChessBoard board)
    {
        var dx = Math.Abs(newX - PieceX);
        var dy = Math.Abs(newY - PieceY);
        return (dx == 1 && dy == 2) || (dx == 2 && dy == 1);
    }
    protected override bool IsValidMove(int newX, int newY, ChessBoard board)
    {
        // Check if the move is in an L-shape
        var dx = Math.Abs(newX - PieceX);
        var dy = Math.Abs(newY - PieceY);
        return (dx == 1 && dy == 2) || (dx == 2 && dy == 1);
    }
}