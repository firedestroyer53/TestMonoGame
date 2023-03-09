using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestMonoGame
{
    internal class Piece
    {
        // Template for a chess piece class
        public bool isWhite;
        public int pieceValue;
        public int pieceX;
        public int pieceY;
        //Position vector of X and Y
        public Vector2 position;
        // Class constructor
        public Piece(bool iswhite, int pieceValue, int x, int y)
        {
            this.isWhite = iswhite;
            this.pieceValue = pieceValue;
            this.pieceX = x;
            this.pieceY = y;
            this.position = new Vector2(x,y);
        }
        //get bounds method
        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }
        // Pawn moving function
        
        public bool pawnValidMove(int currentX, int currentY, int newX, int newY, bool isWhite, ChessBoard board)
        {
            // Check if the move is within the bounds of the board
            if (newX < 0 || newX > 7 || newY < 0 || newY > 7)
            {
                return false;
            }

            // Check if the new position is occupied by a friendly piece
            if (board[newX, newY] != null && board[newX, newY].isWhite == isWhite)
            {
                return false;
            }

            // Check if the pawn is moving forward
            int direction = isWhite ? -1 : 1; // Direction should be reversed as y starts from the top
            if (newY != currentY + direction || newX != currentX)
            {
                // Check if it's the pawn's first move and can move two squares
                if (newY == currentY + 2 * direction && currentY == (isWhite ? 6 : 1) && newX == currentX)
                {
                    // Check if there are any pieces in the way of the pawn's move
                    if (board[currentX, currentY + direction] != null || board[newX, newY] != null)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }

            // Check if the new position is empty or occupied by an opponent's piece
            if (board[newX, newY] == null || board[newX, newY].isWhite != isWhite)
            {
                return true;
            }

            return false;
        }

        public bool isMouseOver(MouseState mouseState)
        {
            if (mouseState.X > (pieceX - 1) * 64 && mouseState.X < pieceX * 64 && mouseState.Y > (pieceY - 1) * 64 && mouseState.Y < pieceY * 64)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
