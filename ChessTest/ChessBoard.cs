using System.Collections;
using System.Linq;

namespace ChessTest;

public class ChessBoard : IEnumerable
{
    // class for a chessboard
    private static readonly Piece[,] Board = new Piece[8, 8];

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

    public static void MovePiece(Piece piece, int x, int y)
    {
        // method to move a piece on the board
        Board[piece.PieceX, piece.PieceY] = null;

        piece.PieceX = x;
        piece.PieceY = y;
        Board[piece.PieceX, piece.PieceY] = piece;
    }

    // get length of x function here
    public static int GetLengthX()
    {
        return Board.GetLength(0);
    }

    // get length of y function here
    public static int GetLengthY()
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
            else
            {
                return null;
            }
        }
        set
        {
            if (x >= 0 && x < Board.GetLength(0) && y >= 0 && y < Board.GetLength(1))
            {
                Board[x, y] = value;
            }
        }
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