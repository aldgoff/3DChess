using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanBoardDemo : MonoBehaviour
{
    public ChessBoard3D chessBoard;

    private bool scanInProgress = false;

    // Start is called before the first frame update
    void Start()
    {
		print("----- ScanBoardDemo.Start() -----");
	}

	// Update is called once per frame
	void Update()
    {
        Alternate();
    }

    // Runtime demo of marking squares - a simple scan through the entire board.
    public int[] scan = new int[3];

    public void RunScan()
    {
        print("ScanBoardDemo.RunScan()");
        InitScan();
        // Let Update call Alternate().
    }

    private void EndScan()
    {
		chessBoard.cells[0, 0, 0].UnhighlightCell();
	}

	private void InitScan()
    {
        print("ScanBoardDemo.InitScan()");

        scan[0] = scan[1] = scan[2] = 0;
		chessBoard.cells[scan[0], scan[1], scan[2]].HighlightCell(Color.red);

        scanInProgress = true;
        alternate = true;
    }

    private bool alternate = true;
    private void Alternate()
    {
        if (scanInProgress == false)
        {
            return;
        }

        // On alternate frames...
        if (alternate) // Unhighlight previous square.
        {
            alternate = false;
			chessBoard.cells[scan[0], scan[1], scan[2]].UnhighlightCell();
		}
		else // Highlight next square.
        {
            NextSquare();
            if(scanInProgress == false)
            {
                EndScan();
                return;
            }

			chessBoard.cells[scan[0], scan[1], scan[2]].HighlightCell(Color.red);

            alternate = true;
        }
    }

    private void NextSquare() // TODO: replace magic numbers.
    {
        scan[0]++;
        if (scan[0] >= chessBoard.size.x)
        {
            scan[0] = 0;
            scan[1]++;
            if (scan[1] >= chessBoard.size.y)
            {
                scan[1] = 0;
                scan[2]++;
                if (scan[2] >= chessBoard.size.z)
                {
                    scan[2] = 0;
                    scanInProgress = false;
                }
            }
        }
    }
}
