using System.Collections;

namespace TestMonoGame
{
    public class ChessBoard : IEnumerable
    {
        // class for a chessboard
        public static Piece[,] board = new Piece[8, 8];

        public void placePiece(Piece piece)
        {
            // method to add a piece to the board
            board[piece.pieceX, piece.pieceY] = piece;
        }

        //support to use foreach loop on chessboard
        public IEnumerator GetEnumerator()
        {
            foreach (var piece in board)
            {
                yield return piece;
            }
        }

        public void movePiece(Piece piece, int x, int y)
        {
            // method to move a piece on the board
            board[piece.pieceX, piece.pieceY] = null;

            piece.pieceX = x;
            piece.pieceY = y;
            board[piece.pieceX, piece.pieceY] = piece;
        }

        // get length of x function here
        public int getLengthX()
        {
            return board.GetLength(0);
        }

        // get length of y function here
        public int getLengthY()
        {
            return board.GetLength(1);
        }

        // array2D[x, y]
        public Piece this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1))
                {
                    return board[x, y];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (x >= 0 && x < board.GetLength(0) && y >= 0 && y < board.GetLength(1))
                {
                    board[x, y] = value;
                }
                else
                {

                }
            }
        }


        // class constructor here
        public ChessBoard()
        {
            // initialize the board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = null;
                }
            }
        }
    }
}
