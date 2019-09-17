using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Create one of the standard size boards (8x8x8, 8x8x10, 10x10x10, 5x5x5).
 * Board coordinates are a permutation of Unity coordinates.
 *  Board Unity
 *    x     z
 *    y     x
 *    z     y
 * Squares are spaced 1 unit apart horizontally.
 * Currently spaced 2 units vertically. TODO: make variable under user control.
 * Squares are 0.96 x 0.96 x 0.1.
 * Center of board is placed at Unity origin (0,0,0).
 * TODO: Code architecture.
 */

// Associated with the CreateBoard button.
public class CreateBoard : MonoBehaviour
{
	public Dropdown standardBoardSize;
	public ChessBoard3D chessBoard;

	// TODO: Possibly not needed.
	// Called by EventSystem when selection changes.
	public void BoardSizeDropdownIndexChanged(int index)
	{
		print("Board size dropdown index = " + index);
		print("  standardBoardSize dropdown = " + standardBoardSize.captionText.text);
	}

	// Called on button "OnClick()".
	public void CreateSizedBoard()
	{
		print("=== CreateSizedBoard");

		string boardSizeOption = standardBoardSize.captionText.text;
		print("  standardBoardSize dropdown selected option = " + boardSizeOption);

		Vector3Int size;
		Vector3Int locWhiteK11sq;
		if (boardSizeOption == "8x8x8")
		{
			size = new Vector3Int(8, 8, 8);
			locWhiteK11sq = new Vector3Int(0, 0, 4);
		}
		else if (boardSizeOption == "8x8x10")
		{
			size = new Vector3Int(8, 8, 10);
			locWhiteK11sq = new Vector3Int(0, 0, 5);
		}
		else if (boardSizeOption == "10x10x10")
		{
			size = new Vector3Int(10, 10, 10);
			locWhiteK11sq = new Vector3Int(0, 0, 5);
		}
		else if (boardSizeOption == "5x5x5")
		{
			size = new Vector3Int(5, 5, 5);
			locWhiteK11sq = new Vector3Int(0, 0, 2);
		}
		else {
			print("Error, unknown standard board size " + boardSizeOption);
			size = new Vector3Int(0, 0, 0);
			locWhiteK11sq = new Vector3Int(0, 0, 0);
		}
		print("  Selected chess board size = " + size);

		// New method; breaks out properties, better conformance to SRP.
		chessBoard.CreateStandardBoard(size, locWhiteK11sq);
	}

	void Start()
    {
        print("----- CreateBoard.Start() -----");
		print("  standardBoardSize dropdown name    = " + standardBoardSize.name);
		print("  standardBoardSize dropdown caption = " + standardBoardSize.captionText.text);
		print("  standardBoardSize dropdown options = " + standardBoardSize.options.Count);
	}

	void Update() {}
}
