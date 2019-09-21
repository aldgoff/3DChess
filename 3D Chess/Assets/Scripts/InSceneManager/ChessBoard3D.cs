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
	ChessBoard3D() { print("ChessBoard3D.ctor()"); }
	~ChessBoard3D() { print("ChessBoard3D.dtor()"); }

	public ChessBoardProperties properties;

	public Vector3Int size = new Vector3Int(0, 0, 0);

    // Assumes square size is 1.0 or less; typically 0.90.
    public GameObject whiteSquare;
    public GameObject blackSquare;

    public GameObject[,,] squares;

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

		squares = new GameObject[size.x, size.y, size.z];

		this.size = properties.size = size;
		properties.locWhiteK11sq = locWhiteK11sq;
		bool firstSqIsWhite = locWhiteK11sq.z % 2 == 1; // Convention is false, first square is black.

		properties.ComputeBoardEdges();

		Vector2 boardXedges = properties.boardXedges;
		Vector2 boardYedges = properties.boardYedges;
		Vector2 boardZedges = properties.boardZedges;

		for (int k = 0; k < size.z; k++) // Build each level.
		{
			for (int j = 0; j < size.y; j++) // Build each column.
			{
				for (int i = 0; i < size.x; i++) // build each row.
				{
					if ((i + j + k) % 2 == (firstSqIsWhite ? 0 : 1)) {
						squares[i, j, k] = Instantiate(whiteSquare);
					} else {
						squares[i, j, k] = Instantiate(blackSquare);
					}
					squares[i, j, k].transform.position = new Vector3(boardYedges[0] + squareSep / 2 + j, boardZedges[0] + k * boardVerticalSep, boardXedges[0] + squareSep / 2 + i);

					// Alternate levels for black/grey squares.
					if (k % 2 == (firstSqIsWhite ? 0 : 1) && (i + j) % 2 == 1) {
						MeshRenderer aMesh = squares[i, j, k].GetComponent<MeshRenderer>();
						aMesh.materials[0].SetColor("_Color", Color.gray);
					}
				}
			}
		}

		// Mark White K11 square in blue.
		Vector3Int loc = properties.locWhiteK11sq;
		MeshRenderer myMesh = squares[loc.x, loc.y, loc.z].GetComponent<MeshRenderer>();
		myMesh.materials[0].SetColor("_Color", Color.cyan);
	}
}
