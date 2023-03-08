using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestMonoGame
{
    internal class Piece
    {
        // Template for a chess piece class
        public bool pieceColor;
        public int pieceValue;
        public int pieceX;
        public int pieceY;

        // Class constructor
        public Piece(bool isWhite, int pieceValue, int x, int y)
        {
            this.pieceColor = isWhite;
            this.pieceValue = pieceValue;
            this.pieceX = x;
            this.pieceY = y;
        }

        // Pawn moving function
        public void movePawn(int x, int y)
        {
            // Move the pawn to the new position
            ChessBoard.movePiece(this, x, y);
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
