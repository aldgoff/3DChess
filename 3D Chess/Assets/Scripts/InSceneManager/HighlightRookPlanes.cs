using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HighlightRookPlanes : MonoBehaviour
{
	public ChessBoard3D chessBoard;

	private Vector3Int nullSquare = new Vector3Int(-1, -1, -1);

	private Vector3Int srcSquare, dstSquare;

	void Start() {
		print("----- HighlightRookPlanes.Start() -----");
		srcSquare = nullSquare;
		dstSquare = nullSquare;
	}
	void Update() {
		if (doneErasing) {
			ShowAdvSqs(); // One perimeter per frame group.
		}
		EraseAdvSqs(); // One perimeter per smaller frame group.
	}

	private Material[] theMat;
	private HighlightSquareByRayCasting sqScriptClass;

	// Colors:
	private Color rookColor = Color.red;

	static private Color RookQuadWhite = Color.Lerp(Color.red, Color.white, 0.15f);     // Quad.
	static private Color RookQuadBlack = Color.Lerp(Color.red, Color.black, 0.15f);

	static private Color RookLineWhite = Color.Lerp(RookQuadWhite, Color.black, 0.3f);  // Line.
	static private Color RookLineBlack = Color.Lerp(RookQuadBlack, Color.black, 0.3f);

	static private Color RookPointWhite = Color.Lerp(RookLineWhite, Color.black, 0.4f); // Point.
	static private Color RookPointBlack = Color.Lerp(RookLineBlack, Color.black, 0.4f);

	// Planes:
	public void HighlightPlanes(string plane, Vector3Int square, Color color)
	{
		if (plane == "Rook Planes") {
			Horizontal(square, color);
			LeftVertical(square, color);
			RightVertical(square, color);
		} else if (plane == "  Horizontal") { Horizontal(square, color);
		} else if (plane == "  LeftVertical") { LeftVertical(square, color);
		} else if (plane == "  RightVertical") { RightVertical(square, color);
		} else {
			print("Error: unknown Rook plane " + plane);
		}
	}

	public void Horizontal(Vector3Int square, Color color)
	{
		int z = square.z;
		for (int x = 0; x < chessBoard.size.x; x++) {
			for (int y = 0; y < chessBoard.size.y; y++) {
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.x == x && square.y == y) {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookPointWhite : RookPointBlack; // Point.
					} else if (square.x == x || square.y == y) {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookLineWhite : RookLineBlack;   // Line.
					} else {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookQuadWhite : RookQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", rookColor);
				}
			}
		}
	}

	public void LeftVertical(Vector3Int square, Color color)
	{
		int y = square.y;
		for (int z = 0; z < chessBoard.size.z; z++) {
			for (int x = 0; x < chessBoard.size.x; x++) {
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.x == x && square.z == z) {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookPointWhite : RookPointBlack; // Point.
					} else if (square.x == x || square.z == z) {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookLineWhite : RookLineBlack;   // Line.
					} else {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookQuadWhite : RookQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", rookColor);
				}
			}
		}
	}

	public void RightVertical(Vector3Int square, Color color)
	{
		int x = square.x;
		for (int z = 0; z < chessBoard.size.z; z++) {
			for (int y = 0; y < chessBoard.size.y; y++) {
				sqScriptClass = chessBoard.squares[x, y, z].GetComponent<HighlightSquareByRayCasting>();
				theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
				if (color == Color.clear) {
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				} else {
					if (square.y == y && square.z == z) {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookPointWhite : RookPointBlack; // Point.
					} else if (square.z == z || square.y == y) {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookLineWhite : RookLineBlack;   // Line.
					} else {
						rookColor = (sqScriptClass.baseColor == Color.white) ? RookQuadWhite : RookQuadBlack;   // Quadrant.
					}
					theMat[0].SetColor("_Color", rookColor);
				}
			}
		}
	}

	// Advancement squares:
	bool doneShowing = true;
	bool doneErasing = true;
	int frameDelay = 0;
	int perimeter = 0;

	private AdvancementSquare advSq;

	private bool IsOffBoard(Vector3Int sq, Vector3Int size)
	{
		// Skip squares off the board.
		if ((sq.x < 0 || size.x <= sq.x)
		 || (sq.y < 0 || size.y <= sq.y)
		 || (sq.z < 0 || size.z <= sq.z)) {
			return true;
		}
		// Skip dstSquare.
		if (sq == dstSquare) {
			return true;
		}

		// Highlight square.
		return false;
	}

	public void HighlightSquare(Vector3Int sq, bool line, string point="")
	{
		sqScriptClass = chessBoard.squares[sq.x, sq.y, sq.z].GetComponent<HighlightSquareByRayCasting>();
		theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
		if (point == "Point") {
			rookColor = (sqScriptClass.baseColor == Color.white) ? RookPointWhite : RookPointBlack; // Point.
		} else if (line) {
			rookColor = (sqScriptClass.baseColor == Color.white) ? RookLineWhite : RookLineBlack;   // Line.
		} else {
			rookColor = (sqScriptClass.baseColor == Color.white) ? RookQuadWhite : RookQuadBlack;   // Quadrant.
		}
		theMat[0].SetColor("_Color", rookColor);
	}

	private void EraseAdvSqs() // Called by Update().
	{
		if (doneErasing) {
			return;
		}

		dstSquare = nullSquare;

		if (frameDelay == 0) {
			frameDelay = 20;
			print("Erasing Rook perimeter = " + perimeter);

			// Unhighlight squares.
			float xPos = (dstSquare.x > srcSquare.x) ? 0.1f : -0.1f;
			float yPos = (dstSquare.y > srcSquare.y) ? 0.1f : -0.1f;

			if(perimeter >= 0) {
				for (int i = 0; i < advSq.perimeters[perimeter].Length; i++) {
					Vector3Int sq = advSq.perimeters[perimeter][i];

					// Skip squares off the board.
					if (IsOffBoard(sq, chessBoard.size)) {
						continue;
					}

					// Unhighlight next square.
					sqScriptClass = chessBoard.squares[sq.x, sq.y, sq.z].GetComponent<HighlightSquareByRayCasting>();
					theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
					theMat[0].SetColor("_Color", sqScriptClass.baseColor);
				}

				if (--perimeter < 0) {
					print("Last perimeter");
					doneErasing = true;
				}
			}
		} else {
			frameDelay--;
		}
	}

	private void ShowAdvSqs() // Called by Update().
	{
		if (doneShowing) {
			return;
		} else {
			if (frameDelay == 0) {
				frameDelay = 30;
				//print("Rook perimeter = " + perimeter);

				// Restore previous perimeter's tilt to flat.
				if (perimeter > 0) {
					for (int i = 0; i < advSq.perimeters[perimeter - 1].Length; i++) {
						if (i == 0) print("Restore perimeter " + (perimeter - 1));
						Vector3Int sq = advSq.perimeters[perimeter - 1][i];
						if (IsOffBoard(sq, chessBoard.size)) {
							continue;
						}
						chessBoard.squares[sq.x, sq.y, sq.z].transform.rotation = new Quaternion(0, 0, 0, 0); // Restore.
					}
				}

				// Highlight squares and ripple tilt.
				float xPos = (dstSquare.x > srcSquare.x) ? 0.1f : -0.1f;
				float yPos = (dstSquare.y > srcSquare.y) ? 0.1f : -0.1f;

				if (perimeter < advSq.perimeters.Count) {
					for (int i = 0; i < advSq.perimeters[perimeter].Length; i++) {
						if (i == 0) print("Rotate perimeter " + perimeter);
						Vector3Int sq = advSq.perimeters[perimeter][i];

						// Skip squares off the board.
						if (IsOffBoard(sq, chessBoard.size)) {
							continue;
						}
						chessBoard.squares[sq.x, sq.y, sq.z].transform.rotation = new Quaternion(4.0f, yPos, 0.0f, xPos); // Rotate.

						// Highlight next square.
						bool line = i == 0 || i == advSq.perimeters[perimeter].Length - 1;
						HighlightSquare(sq, line);
					}
				}

				if (++perimeter > advSq.perimeters.Count) {
					print("Last perimeter");
					doneShowing = true;
				}
			} else {
				frameDelay--;
			}
		}
	}

	public void EraseAdvSq()
	{
		perimeter = advSq.perimeters.Count - 1;
		doneErasing = false;
	}

	// Called by HighLightSquareByGrid.SetSrcDstSquares().
	public bool AdvSq(Vector3Int srcSquare, Vector3Int dstSquare)
	{
		bool validAdvSq = true;

		this.srcSquare = srcSquare;
		this.dstSquare = dstSquare;
		print("AdvSq: " + srcSquare + " / " + dstSquare);
		perimeter = 0;

		bool sameX = srcSquare.x == dstSquare.x;
		bool sameY = srcSquare.y == dstSquare.y;
		bool sameZ = srcSquare.z == dstSquare.z;

		if ((sameX && sameY) || (sameX && sameZ) || (sameY && sameZ)) {
			print("Linear rook move.");
			if (sameX && sameY) {
				print("Rook vertical linear move, Z varies.");
			}
			else if (sameX && sameZ) {
				print("Rook horizontal linear move, Y varies.");
			}
			else if(sameY && sameZ) {
				print("Rook horizontal linear move, X varies.");
			}
			//print("Number of perimeters = " + advSq.Perims);
		}
		else if (sameX || sameY || sameZ) {
			print("Quadrant rook move.");
			if (sameZ) {
				advSq = new RookAdvSqQuad("Horizontal", srcSquare, dstSquare);
				print("Rook Horizontal plane, advancement square number of perimeters = " + advSq.Perims);
			}
			else if (sameX) {
				advSq = new RookAdvSqQuad("RightVertical", srcSquare, dstSquare);
				print("Rook RightVertical plane, advancement square number of perimeters = " + advSq.Perims);
			}
			else if (sameY) {
				advSq = new RookAdvSqQuad("LeftVertical", srcSquare, dstSquare);
				print("Rook LeftVertical plane, advancement square number of perimeters = " + advSq.Perims);
			}
			doneShowing = false;
		} else {
			print("Not in a rook plane.");
			validAdvSq = false;
		}

		return validAdvSq;
	}
}
