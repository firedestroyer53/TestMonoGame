using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonoGame
{
    internal class Piece
    {
        //template for a chess piece class
        public bool pieceColor;
        public int pieceValue;
        public int pieceX;
        public int pieceY;

        //Class constructor
        public Piece(bool isWhite, int pieceValue, int x, int y)
        {
            this.pieceColor = isWhite;
            this.pieceValue = pieceValue;
            this.pieceX = x;
            this.pieceY = y;
        }

    }
}