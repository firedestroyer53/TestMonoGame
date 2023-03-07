using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMonoGame
{
    internal class ChessBoard
    {
        //class for a chessboard that you can put pieces on
        public int[,] board = new int[8, 8];
        public int cellSize = 64;
        public void placePiece(Piece piece)
        {
            //method to place a piece on the board
            board[piece.pieceX, piece.pieceY] = piece.pieceValue;
        }
        public void removePiece(Piece piece)
        {
            //method to remove a piece from the board
            board[piece.pieceX, piece.pieceY] = 0;
        }
        public void movePiece(Piece piece, int x, int y)
        {
            //method to move a piece on the board
            removePiece(piece);
            piece.pieceX = x;
            piece.pieceY = y;
            placePiece(piece);
        }
        //get length of x function here
        public int getLengthX()
        {
            return board.GetLength(0);
        }
        //get length of y function here
        public int getLengthY()
        {
            return board.GetLength(1);
        }
        //array2D[x, y]
        public int this[int x, int y]
        {
            get { return board[x, y]; }
            set { board[x, y] = value; }
        }
        //class constructor here
        public ChessBoard()
        {
            //initialize the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = 0;
                }
            }
        }
    }   
}
