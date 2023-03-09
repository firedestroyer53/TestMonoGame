using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestMonoGame
{
    internal class Piece
    {
        // Template for a chess piece class
        public bool isWhite;

        public int pieceX;
        public int pieceY;
        //Position vector of X and Y
        public Vector2 position;
        // Class constructor
        public Piece(bool iswhite, int x, int y)
        {
            this.isWhite = iswhite;

            this.pieceX = x;
            this.pieceY = y;
            this.position = new Vector2(x, y);
        }
        //get bounds method
        public Rectangle getBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }
        //check if move is valid for a pawn given a piece, and a new position
        public bool IsValidMove(int newX, int newY)
        {
            // Check if the new coordinates are within the bounds of the board
            if (newX < 0 || newX > 7 || newY < 0 || newY > 7)
            {
                return false;
            }

            // Check if the pawn is moving forward
            int direction = isWhite ? -1 : 1;
            if (newY != pieceY + direction)
            {
                return false;
            }

            // Check if the pawn is moving straight ahead
            if (newX == pieceX && !isCapture(newX, newY))
            {
                return true;
            }

            // Check if the pawn is capturing diagonally
            if (isCapture(newX, newY))
            {
                return true;
            }

            // Otherwise, the move is not valid
            return false;
        }

        private bool isCapture(int newX, int newY)
        {
            int direction = isWhite ? -1 : 1;
            if (newY == pieceY + direction && (newX == pieceX - 1 || newX == pieceX + 1))
            {
                return true;
            }
            else
            {
                return false;
            }
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
