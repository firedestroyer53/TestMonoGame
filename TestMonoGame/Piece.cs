using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TestMonoGame
{
    

    public abstract class Piece
    {
        
        // Template for a chess piece class
        public bool isWhite;
        public int pieceX;
        public int pieceY;
        //Position vector of X and Y
        public Vector2 position;
        private ChessBoard board;

        // Class constructor
        public Piece(bool iswhite, int x, int y, ChessBoard board)
        {
            this.isWhite = iswhite;
            this.pieceX = x;
            this.pieceY = y;
            this.position = new Vector2(x, y);
            this.board = board;
            this.board.placePiece(this);
        }

        // Get bounds method
        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }

        public abstract bool IsValidMove(int newX, int newY, ChessBoard board);
        public bool IsMoveValid(int newX, int newY)
        {
            // Check if the move is within the bounds of the board
            if (newX < 0 || newX > 7 || newY < 0 || newY > 7)
            {
                return false;
            }

            // Check if there is a piece at the destination
            Piece pieceAtDestination = board[newX, newY];
            if (pieceAtDestination != null && pieceAtDestination.isWhite == isWhite)
            {
                return false;
            }

            // Check if the move goes through any pieces on the board
            int dx = newX - pieceX;
            int dy = newY - pieceY;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            int xStep = dx / steps;
            int yStep = dy / steps;
            for (int i = 1; i < steps; i++)
            {
                int x = pieceX + i * xStep;
                int y = pieceY + i * yStep;
                if (board[x, y] != null)
                {
                    return false;
                }
            }

            // Call the abstract IsValidMove method in the derived classes
            return IsValidMove(newX, newY, board);
        }


    }

    public class Pawn : Piece
    {
        public bool HasMoved { get; set; }

        public Pawn(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
        {
            HasMoved = false;
        }

        public override bool IsValidMove(int newX, int newY, ChessBoard board)
        {
            // Check if the move is one square forward, or two squares forward if the pawn has not moved yet
            int dy = newY - pieceY;
            int dx = newX - pieceX;
            if (dy == -1 && newX == pieceX && board[newX,newY] == null) // white pawn moving one square forward
            {
                HasMoved = true;
                return true;
            }
            else if (dy == -2 && HasMoved == false && newX == pieceX && board[newX, newY] == null) // white pawn moving two squares forward from starting position
            {
                HasMoved = true;
                return true;
            }
            else if (dy == 1 && newX == pieceX && board[newX, newY] == null) // black pawn moving one square forward
            {
                HasMoved = true;
                return true;
            }
            else if (dy == 2 && HasMoved == false && newX == pieceX && board[newX, newY] == null) // black pawn moving two squares forward from starting position
            {
                HasMoved = true;
                return true;
            }
            else if (dx == 1 && dy == -1 && board[newX,newY].isWhite != isWhite) // white pawn capturing to the right
            {
                return true;
            }
            else if (dx == -1 && dy == -1 && board[newX, newY].isWhite != isWhite) // white pawn capturing to the left
            {
                return true;
            }
            else if (dx == 1 && dy == 1 && board[newX, newY].isWhite != isWhite) // black pawn capturing to the right
            {
                return true;
            }
            else if (dx == -1 && dy == 1 && board[newX, newY].isWhite != isWhite) // black pawn capturing to the left
            {
                return true;
            }
            return false;
        
        }
    }
    public class King : Piece
    {
        public King(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
        {
        }

        public override bool IsValidMove(int newX, int newY, ChessBoard board)
        {
            // Check if the move is within one square in any direction
            return Math.Abs(newX - pieceX) <= 1 && Math.Abs(newY - pieceY) <= 1;
        }
    }

    public class Queen : Piece
    {
        public Queen(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
        {
        }

        public override bool IsValidMove(int newX, int newY, ChessBoard board)
        {
            // Check if the move is along a straight line in any direction
            return newX == pieceX || newY == pieceY || Math.Abs(newX - pieceX) == Math.Abs(newY - pieceY);
        }
    }

    public class Rook : Piece
    {
        public Rook(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
        {
        }

        public override bool IsValidMove(int newX, int newY, ChessBoard board)
        {
            // Check if the move is along a straight line
            return newX == pieceX || newY == pieceY;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
        {
        }

        public override bool IsValidMove(int newX, int newY, ChessBoard board)
        {
            // Check if the move is along a diagonal line
            return Math.Abs(newX - pieceX) == Math.Abs(newY - pieceY);
        }
    }

    public class Knight : Piece
    {
        public Knight(bool isWhite, int x, int y, ChessBoard board) : base(isWhite, x, y, board)
        {
        }

        public override bool IsValidMove(int newX, int newY, ChessBoard board)
        {
            // Check if the move is in an L-shape
            int dx = Math.Abs(newX - pieceX);
            int dy = Math.Abs(newY - pieceY);
            return (dx == 1 && dy == 2) || (dx == 2 && dy == 1);
        }
    }

}
