using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To make folding icons persistent, place this line in the settings file.
// "editor.showFoldingControls": "always"

/* Design */

/* Couplings:
*   Squares
* State:
*   OnDisplay:
*     MouseOverSquare
*     HighlightedSquares:
*       Clicked
*       Source - such as a piece
*       Destination - such as the perimeter of an advancement square
*       Presentation - such as Rook planes
* Responsibilities:
*   Contains the set of squares
*   Support queries on Properties & state
*   Support commands to change state
*/

/* Graphical:
 * Square separation grid setting (1)
 * Vertical separation between levels (should be multiple of squares horizontal separation)
 */

public class ChessBoard3D : MonoBehaviour
{
	public ChessBoardProperties properties;

	public Vector3Int size = new Vector3Int(0, 0, 0);

    // Assumes square size is 1.0 or less; typically 0.90.
    public GameObject whiteSquare;
	public GameObject blackSquare;

	public Cell[,,] cells;

	// Set from ChessBoardProperties.
	private int boardVerticalSep;
	private float squareSep;
	void Start()
    {
		print("----- ChessBoard3D.Start() -----");
		properties.squareSize = whiteSquare.transform.localScale.x;
		properties.squareThickness = whiteSquare.transform.localScale.y;

		boardVerticalSep = properties.boardVerticalSep;
		squareSep = properties.squareSep;

		print("  Initial board.size = " + properties.size);
		print("  squareSize = " + properties.squareSize);
		print("  squareThickness = " + properties.squareThickness);
		print("  boardVerticalSep = " + boardVerticalSep);
		print("  squareSep = " + squareSep);
	}

	void Update() {}

	// New method more compliant with SRP.
	public void CreateStandardBoard(Vector3Int size, Vector3Int locWhiteK11sq)
	{
		print("ChessBoard3D.CreateStandardBoard() of " + size.x + "x" + size.y + "x" + size.z + " with level sep " + boardVerticalSep);

		cells = new Cell[size.x, size.y, size.z];

		this.size = properties.size = size;
		properties.locWhiteK11sq = locWhiteK11sq;
		bool firstSqIsWhite = locWhiteK11sq.z % 2 == 1; // Convention is false, first square is black.

		properties.ComputeBoardEdges();

		Vector2 boardXedges = properties.boardXedges;
		Vector2 boardYedges = properties.boardYedges;
		Vector2 boardZedges = properties.boardZedges;

		Color DarkBlack = Color.Lerp(Color.gray, Color.black, 0.5f);

		for (int k = 0; k < size.z; k++) // Build each level.
		{
			for (int j = 0; j < size.y; j++) // Build each column.
			{
				for (int i = 0; i < size.x; i++) // Build each row.
				{
					if ((i + j + k) % 2 == (firstSqIsWhite ? 0 : 1)) {
						cells[i, j, k] = new Cell(Instantiate(whiteSquare));
						cells[i, j, k].SetBaseColor(Color.white);
					} else {
						cells[i, j, k] = new Cell(Instantiate(blackSquare));
						cells[i, j, k].SetBaseColor(DarkBlack);
					}
					cells[i, j, k].square.transform.position = new Vector3(boardYedges[0] + squareSep / 2 + j, boardZedges[0] + k * boardVerticalSep, boardXedges[0] + squareSep / 2 + i);

					// Alternate levels for black/grey cells.
					if ((i + j) % 2 == 1) {
						if (k % 2 == (firstSqIsWhite ? 0 : 1)) {
							cells[i, j, k].SetBaseColor(Color.gray);
						}
					}
				}
			}
		}

		// Mark White K11 square in blue.
		Vector3Int loc = properties.locWhiteK11sq;
		cells[loc.x, loc.y, loc.z].SetBaseColor(Color.cyan);
	}
}
