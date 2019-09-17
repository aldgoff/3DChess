using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBishopPlanes : MonoBehaviour
{
	public ChessBoard3D chessBoard;

	private int maxDist = 0;

	void Start()
	{
		print("----- HighlightBishopPlanes.Start() -----");
	}

	void Update() { }


	private Material[] theMat;
	private HighlightSquareByRayCasting sqScriptClass;
	private Color bishopColor = Color.green;

	static private Color BishopQuadWhite = Color.Lerp(Color.green, Color.white, 0.3f);		// Quad.
	static private Color BishopQuadBlack = Color.Lerp(Color.green, Color.black, 0.1f);

	static private Color BishopLineWhite = Color.Lerp(BishopQuadWhite, Color.black, 0.4f);	// Line.
	static private Color BishopLineBlack = Color.Lerp(BishopQuadBlack, Color.black, 0.4f);

	static private Color BishopPointWhite = Color.Lerp(BishopLineWhite, Color.black, 0.4f);	// Point.
	static private Color BishopPointBlack = Color.Lerp(BishopLineBlack, Color.black, 0.4f);

	public void HighlightPlanes(string plane, Vector3Int square, Color color)
	{
		print("***** " + plane);

		if (plane == "Bishop Planes") {
			ForwardSlant(square, color);
			BackwardSlant(square, color);
			LeftSlant(square, color);
			RightSlant(square, color);
		} else if (plane == "  ForwardBackward") {
			ForwardSlant(square, color);
			BackwardSlant(square, color);
		} else if (plane == "  LeftRight") {
			LeftSlant(square, color);
			RightSlant(square, color);
		} else if (plane == "  ForwardSlant") {
			ForwardSlant(square, color);
		} else if (plane == "  BackwardSlant") {
			BackwardSlant(square, color);
		} else if (plane == "  LeftSlant") {
			LeftSlant(square, color);
		} else if (plane == "  RightSlant") {
			RightSlant(square, color);
		} else {
			print("Error: unknown Bishop plane " + plane);
		}
	}

	private void MaxDist() // Hack until for loop limits computed more efficiently.
	{
		maxDist = (chessBoard.size.x > maxDist) ? chessBoard.size.x : maxDist;
		maxDist = (chessBoard.size.y > maxDist) ? chessBoard.size.y : maxDist;
		maxDist = (chessBoard.size.z > maxDist) ? chessBoard.size.z : maxDist;
	}

	private bool OffTheBoard(int x, int y, int z) // Hack until for loop limits computed more efficiently.
	{
		if ((x < 0) || chessBoard.size.x - 1 < x) return true;
		if ((y < 0) || chessBoard.size.y - 1 < y) return true;
		if ((z < 0) || chessBoard.size.z - 1 < z) return true;

		return false;
	}

	// TODO: make the nested loop limits more efficient; most squares on board is 48, current scan is 256. Do for all 4 planes.

	public void ForwardSlant(Vector3Int square, Color color)
	{
		MaxDist();

		for (int i = -maxDist; i < maxDist; i++) {
			for (int j = -maxDist; j < maxDist; j++) {
				int x = square.x + i;
				int y = square.y + j;
				int z = square.z - (i + j);

				if (OffTheBoard(x, y, z)) continue;

				theMat = chessBoard.squares[x, y, z].GetComponent<MeshRenderer>().materials;
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.x == x && square.y == y && square.z == z) {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopPointWhite : BishopPointBlack; // Point.
					} else if ((z == square.z && (x - square.x) == -(y - square.y))		// Horizontal.
							|| (x == square.x && (z - square.z) == -(y - square.y))		// RightVertical.
							|| (y == square.y && (x - square.x) == -(z - square.z))) {	// LeftVertical.
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopLineWhite : BishopLineBlack;   // Line.
					} else {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopQuadWhite : BishopQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", bishopColor);
				}
			}
		}
	}

	public void BackwardSlant(Vector3Int square, Color color)
	{
		MaxDist();

		for (int i = -maxDist; i < maxDist; i++) {
			for (int j = -maxDist; j < maxDist; j++) {
				int x = square.x + i;
				int y = square.y + j;
				int z = square.z + (i + j);

				if (OffTheBoard(x, y, z)) continue;

				theMat = chessBoard.squares[x, y, z].GetComponent<MeshRenderer>().materials;
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.x == x && square.y == y && square.z == z) {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopPointWhite : BishopPointBlack; // Point.
					} else if ((z == square.z && (x - square.x) == -(y - square.y))     // Horizontal.
							|| (x == square.x && (z - square.z) ==  (y - square.y))     // RightVertical.
							|| (y == square.y && (x - square.x) ==  (z - square.z))) {  // LeftVertical.
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopLineWhite : BishopLineBlack;   // Line.
					} else {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopQuadWhite : BishopQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", bishopColor);
				}
			}
		}
	}

	public void LeftSlant(Vector3Int square, Color color)
	{
		MaxDist();
		int offTheBoard = 0;

		for (int i = -maxDist; i < maxDist; i++) {
			for (int j = -maxDist; j < maxDist; j++) {
				int x = square.x + (i + j); // works.
				int y = square.y + i;
				int z = square.z + j;

				if (OffTheBoard(x, y, z)) { // TODO: pull offTheBoard once for loop limits more efficient.
					offTheBoard++;
					continue;
				}

				theMat = chessBoard.squares[x, y, z].GetComponent<MeshRenderer>().materials;
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.x == x && square.y == y && square.z == z) {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopPointWhite : BishopPointBlack; // Point.
					} else if ((z == square.z && (x - square.x) ==  (y - square.y))		// Horizontal.
						    || (x == square.x && (z - square.z) == -(y - square.y))		// RightVertical.
						    || (y == square.y && (x - square.x) ==  (z - square.z))) {	// LeftVertical.
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopLineWhite : BishopLineBlack;   // Line.
					} else {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopQuadWhite : BishopQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", bishopColor);
				}
			}
		}

		print("LeftSlant Bishop squares off board = " + offTheBoard + "/" + (2*maxDist*2*maxDist)); // 208-255/256, sigh.
	}

	public void RightSlant(Vector3Int square, Color color)
	{
		MaxDist();

		for (int i = -maxDist; i < maxDist; i++) {
			for (int j = -maxDist; j < maxDist; j++) {
				int x = square.x + i - j; // works.
				int y = square.y + i;
				int z = square.z + j;

				if (OffTheBoard(x, y, z)) continue;

				theMat = chessBoard.squares[x, y, z].GetComponent<MeshRenderer>().materials;
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.x == x && square.y == y && square.z == z) {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopPointWhite : BishopPointBlack; // Point.
					} else if ((z == square.z && (x - square.x) ==  (y - square.y))		// Horizontal.
							|| (x == square.x && (z - square.z) ==  (y - square.y))		// RightVertical.
							|| (y == square.y && (x - square.x) == -(z - square.z))) {	// LeftVertical.
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopLineWhite : BishopLineBlack;   // Line.
					} else {
						bishopColor = (sqScriptClass.baseColor == Color.white) ? BishopQuadWhite : BishopQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", bishopColor);
				}
			}
		}
	}
}

/* Duke plane.
  				int x = square.x + i;
				int y = square.y + i;
				int z = square.z - i;
*/