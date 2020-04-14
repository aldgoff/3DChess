using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Poor class name, this class supports highlighting both rook planes and advancement squares.
 */

public class HighlightRookPlanes : MonoBehaviour
{
	#region Class Variables
	public ChessBoard3D chessBoard;

	private Vector3Int nullSquare = new Vector3Int(-1, -1, -1);

	private Vector3Int srcSquare, dstSquare;

	private bool debug;
	#endregion

	void Start() {
		print("----- HighlightRookPlanes.Start() -----");
		srcSquare = nullSquare;
		dstSquare = nullSquare;
	}

	void Update() {
	}

	// Public interface for highlighting rook planes:
	// TODO: Add interface to unlight planes.
	public void HighlightPlanes(string plane, Vector3Int srcSq, Color color)
	{
		// Plane comes from the UI dropdown.
		// Square is the cell the mouse is hovering over, determined by raycasting.
		// Color if clear implies unhighlight.

		if (plane == "Rook Planes") {
			Horizontal(srcSq, color);
			LeftVertical(srcSq, color);
			RightVertical(srcSq, color);
		} else if (plane == "  Horizontal") {
			Horizontal(srcSq, color);
		} else if (plane == "  LeftVertical") {
			LeftVertical(srcSq, color);
		} else if (plane == "  RightVertical") {
			RightVertical(srcSq, color);
		} else {
			print("Error: unknown Rook plane " + plane);
		}
	}

	// Partial public interface for highlighting rook advancement squares:
	public IEnumerator ClearAdvSqByPerimeter(AdvancementSquare advSq)
	{
		UnHighlightSquare(advSq.dstSquare);
		for (int perimeter = advSq.perimeters.Count-1; perimeter >= 0; perimeter--) {
			if (debug) print("^^^ Coroutine: clear perimeter " + (perimeter + 1));

			for (int i = 0; i < advSq.perimeters[perimeter].Length; i++) {
				Vector3Int sq = advSq.perimeters[perimeter][i];

				// Skip squares off the board.
				if (IsOffBoard(sq, chessBoard.size)) {
					continue;
				}

				UnHighlightSquare(sq);
			}

			yield return new WaitForSeconds(0.1f); // TODO: put advSqByPerimeter speed under player control.
		}
	}

	public IEnumerator ShowAdvSqByPerimeter(AdvancementSquare advSq)
	{
		for (int perimeter = 0; perimeter <= advSq.perimeters.Count; perimeter++) {
			if(debug) print("^^^ Coroutine: show perimeter " + (perimeter + 1));

			int prevPerimeter = perimeter - 1;
			int currPerimeter = perimeter;

			// Flatten ripple tilt of previous perimeter.
			if(perimeter > 0) {
				for (int i = 0; i < advSq.perimeters[prevPerimeter].Length; i++) {
					Vector3Int sq = advSq.perimeters[prevPerimeter][i];

					// Skip squares off the board.
					if (IsOffBoard(sq, chessBoard.size)) {
						continue;
					}
					chessBoard.cells[sq.x, sq.y, sq.z].square.transform.rotation = new Quaternion(0f, 0.0f, 0.0f, 0.0f); // Flatten.
				}
			}

			// Highlight and ripple tilt current perimeter.
			if (perimeter < advSq.perimeters.Count) {
				float xPos = (dstSquare.x > srcSquare.x) ? 0.1f : -0.1f;
				float yPos = (dstSquare.y > srcSquare.y) ? 0.1f : -0.1f;

				for (int i = 0; i < advSq.perimeters[currPerimeter].Length; i++) {
					Vector3Int sq = advSq.perimeters[currPerimeter][i];

					// Skip squares off the board.
					if (IsOffBoard(sq, chessBoard.size)) {
						continue;
					}
					chessBoard.cells[sq.x, sq.y, sq.z].square.transform.rotation = new Quaternion(4.0f, yPos, 0.0f, xPos); // Rotate.

					HighlightSquare(sq.x, sq.y, sq.z, DetCellToSrc(srcSquare, sq, rookColor));
				}
			}

			yield return new WaitForSeconds(0.2f); // TODO: put advSqByPerimeter speed under player control.
		}
	}

	// TODO: Lots of duplication, need helper methods.
	public IEnumerator ClearShowAdvSqsByPerimeter(AdvancementSquare advSq1, AdvancementSquare advSq2)
	{
		// Clear first advSq1.
		for (int perimeter = advSq1.perimeters.Count - 1; perimeter >= 0; perimeter--) {
			if (true) print("^^^ Coroutine: clear perimeter " + (perimeter + 1));

			for (int i = 0; i < advSq1.perimeters[perimeter].Length; i++) {
				Vector3Int sq = advSq1.perimeters[perimeter][i];

				// Skip squares off the board.
				if (IsOffBoard(sq, chessBoard.size)) {
					continue;
				}

				UnHighlightSquare(sq);
			}

			yield return new WaitForSeconds(0.1f); // TODO: put advSqByPerimeter speed under player control.
		}

		// Show 2nd advSq1.
		for (int perimeter = 0; perimeter <= advSq2.perimeters.Count; perimeter++) {
			if (debug) print("^^^ Coroutine: show perimeter " + (perimeter + 1));

			int prevPerimeter = perimeter - 1;
			int currPerimeter = perimeter;

			// Flatten ripple tilt of previous perimeter.
			if (perimeter > 0) {
				for (int i = 0; i < advSq2.perimeters[prevPerimeter].Length; i++) {
					Vector3Int sq = advSq2.perimeters[prevPerimeter][i];

					// Skip squares off the board.
					if (IsOffBoard(sq, chessBoard.size)) {
						continue;
					}
					chessBoard.cells[sq.x, sq.y, sq.z].square.transform.rotation = new Quaternion(0f, 0.0f, 0.0f, 0.0f); // Flatten.
				}
			}

			// Highlight and ripple tilt current perimeter.
			if (perimeter < advSq2.perimeters.Count) {
				float xPos = (dstSquare.x > srcSquare.x) ? 0.1f : -0.1f;
				float yPos = (dstSquare.y > srcSquare.y) ? 0.1f : -0.1f;

				for (int i = 0; i < advSq2.perimeters[currPerimeter].Length; i++) {
					Vector3Int sq = advSq2.perimeters[currPerimeter][i];

					// Skip squares off the board.
					if (IsOffBoard(sq, chessBoard.size)) {
						continue;
					}
					chessBoard.cells[sq.x, sq.y, sq.z].square.transform.rotation = new Quaternion(4.0f, yPos, 0.0f, xPos); // Rotate.

					HighlightSquare(sq.x, sq.y, sq.z, DetCellToSrc(srcSquare, sq, rookColor));
				}
			}

			yield return new WaitForSeconds(0.2f); // TODO: put advSqByPerimeter speed under player control.
		}
	}



	// Private members and methods:
	private HighlightSquareByRayCasting sqScriptClass;

	/* Algorithm for highlighting rook planes:
	 * The nexus of the 3 planes is the square the mouse is hovering over (source square), revealed by ray casting.
	 * For each plane that is selected in the UI, all the cells in that plane need to be highlighted.
	 * The color for rook is red, but the tint depends on:
	 *   the cells's relationship with the source square
	 *   the base color of the cell (white or black)
	 * The plane is scanned by a simple double for loop covering every cell in the plane.
	 * For each cell in the plane the relationship with the source square is determined.
	 * Highligting is done depending on the CellToSource and base color of the cell.
	 */

	// Color:
	private Color rookColor = Color.red;

	// Tints:
	static private Color RookQuadWhite = Color.Lerp(Color.red, Color.white, 0.15f);     // Quad.
	static private Color RookQuadBlack = Color.Lerp(Color.red, Color.black, 0.15f);

	static private Color RookLineWhite = Color.Lerp(RookQuadWhite, Color.black, 0.3f);  // Line.
	static private Color RookLineBlack = Color.Lerp(RookQuadBlack, Color.black, 0.3f);

	static private Color RookPointWhite = Color.Lerp(RookLineWhite, Color.black, 0.4f); // Point.
	static private Color RookPointBlack = Color.Lerp(RookLineBlack, Color.black, 0.4f);

	// Tinting:
	public enum CellToSource {
		Clear,	// Overloading enum so single interface for highlight and unhighlight.
		Point,	// Nexus of the planes - darkest tink.
		Line,	// Represents a straight line move - medium tint.
		Quad,	// A pure quandrant move - lightest tint.
	};

	private CellToSource DetCellToSrc(Vector3Int srcSq, Vector3Int dstSq, Color color)
	{
		if (color == Color.clear) {
			return CellToSource.Clear;
		}

		if (srcSq == dstSq) {									return CellToSource.Point;
		}
		else if ((srcSq.x == dstSq.x && srcSq.y == dstSq.y)
			  || (srcSq.y == dstSq.y && srcSq.z == dstSq.z)
			  || (srcSq.z == dstSq.z && srcSq.x == dstSq.x)) {	return CellToSource.Line;
		}
		else if ((srcSq.x != dstSq.x && srcSq.y != dstSq.y)
			  || (srcSq.y != dstSq.y && srcSq.z != dstSq.z)
			  || (srcSq.z != dstSq.z && srcSq.x != dstSq.x)) {	return CellToSource.Quad;
		}
		else {													return CellToSource.Clear;
		}
	}

	public void HighlightSquare(int x, int y, int z, CellToSource cellToSrc)
	{
		Cell cell = chessBoard.cells[x, y, z];

		if (cellToSrc == CellToSource.Clear) {
			rookColor = cell.baseColor;    // Unhighlight.
		} else if (cellToSrc == CellToSource.Point) {
			rookColor = (cell.baseColor == Color.white) ? RookPointWhite : RookPointBlack; // Point.
		} else if (cellToSrc == CellToSource.Line) {
			rookColor = (cell.baseColor == Color.white) ? RookLineWhite : RookLineBlack;   // Line.
		} else if (cellToSrc == CellToSource.Quad) {
			rookColor = (cell.baseColor == Color.white) ? RookQuadWhite : RookQuadBlack;   // Quadrant.
		}
		else {
			Debug.LogError("Error: unknown CellToSource = " + cellToSrc);
		}
		cell.HighlightCell(rookColor);

		return;
	}

	public void RestoreHighlighting(Vector3Int dstSq, Vector3Int srcSq)
	{
		HighlightSquare(dstSq.x, dstSq.y, dstSq.z, DetCellToSrc(srcSq, dstSq, rookColor));
	}

	// Planes:
	private void Horizontal(Vector3Int srcSq, Color color)
	{
		int z = srcSq.z; // Z-axis is constant in the Horizontal plane.
		for (int x = 0; x < chessBoard.size.x; x++) { // Scan x & y.
			for (int y = 0; y < chessBoard.size.y; y++) {
				Vector3Int dstSq = new Vector3Int(x, y, z);
				HighlightSquare(x, y, z, DetCellToSrc(srcSq, dstSq, color));
			}
		}
	}

	private void LeftVertical(Vector3Int srcSq, Color color)
	{
		int y = srcSq.y; // Y-axis is constant in the LeftVertical plane.
		for (int x = 0; x < chessBoard.size.x; x++) { // Scan x & z.
			for (int z = 0; z < chessBoard.size.z; z++) {
				Vector3Int dstSq = new Vector3Int(x, y, z);
				HighlightSquare(x, y, z, DetCellToSrc(srcSq, dstSq, color));
			}
		}
	}

	private void RightVertical(Vector3Int srcSq, Color color)
	{
		int x = srcSq.x; // X-axis is constant in the RightVertical plane.
		for (int y = 0; y < chessBoard.size.y; y++) { // Scan y & z.
			for (int z = 0; z < chessBoard.size.z; z++) {
				Vector3Int dstSq = new Vector3Int(x, y, z);
				HighlightSquare(x, y, z, DetCellToSrc(srcSq, dstSq, color));
			}
		}
	}

	/* Algorithm for highlighting rook advancement squares:
	 * TBD
	 */

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

	public void HighlightSquare(Vector3Int sq, bool line, string point = "")// TODO: get rid of.
	{
		Cell cell = chessBoard.cells[sq.x, sq.y, sq.z];

		if (point == "Point") {
			rookColor = (cell.baseColor == Color.white) ? RookPointWhite : RookPointBlack; // Point.
		} else if (line) {
			rookColor = (cell.baseColor == Color.white) ? RookLineWhite : RookLineBlack;   // Line.
		} else {
			rookColor = (cell.baseColor == Color.white) ? RookQuadWhite : RookQuadBlack;   // Quadrant.
		}
		cell.HighlightCell(rookColor);
	}

	public void UnHighlightSquare(Vector3Int sq)
	{
		chessBoard.cells[sq.x, sq.y, sq.z].UnhighlightCell();
	}

	// Called by HighLightSquareByGrid.SetSrcDstSquares().
	// TODO: Change to return List<>.
	public AdvancementSquare AdvSq(Vector3Int srcSquare, Vector3Int dstSquare)
	{
		List<AdvancementSquare> advSqs = new List<AdvancementSquare>();

		this.srcSquare = srcSquare;
		this.dstSquare = dstSquare;
		print("AdvSq: " + srcSquare + " / " + dstSquare);

		bool sameX = srcSquare.x == dstSquare.x;
		bool sameY = srcSquare.y == dstSquare.y;
		bool sameZ = srcSquare.z == dstSquare.z;

		if ((sameX && sameY) || (sameX && sameZ) || (sameY && sameZ)) {
			print("Linear rook move.");
			AdvancementSquare advSqR1;
			AdvancementSquare advSqR2 = null;
			if (sameX && sameY) {
				advSqR1 = new RookAdvSqRect("RightVertical", srcSquare, dstSquare);
				print("Rook right vertical linear move, Y varies; number of perimeters = " + advSqR1.Perims);
				//advSqR2 = new RookAdvSqRect("LeftVertical", srcSquare, dstSquare);
				//print("Rook left vertical linear move, Z varies; number of perimeters = " + advSqR2.Perims);
			}
			else if (sameX && sameZ) {
				advSqR1 = new RookAdvSqRect("Horizontal", srcSquare, dstSquare);
				print("Rook horizontal linear move, Y varies; number of perimeters = " + advSqR1.Perims);
				//advSqR2 = new RookAdvSqRect("RightVertical", srcSquare, dstSquare);
				//print("Rook right vertical linear move, Y varies; number of perimeters = " + advSqR2.Perims);
			}
			else if(sameY && sameZ) {
				advSqR1 = new RookAdvSqRect("LeftVertical", srcSquare, dstSquare);
				print("Rook left vertical linear move, X varies; number of perimeters = " + advSqR1.Perims);
				//advSqR2 = new RookAdvSqRect("Horizontal", srcSquare, dstSquare);
				//print("Rook horizontal linear move, X varies; number of perimeters = " + advSqR2.Perims);
			}
			else {
				advSqR1 = null;
				advSqR2 = null;
			}
			if (advSqR1 != null) advSqs.Add(advSqR1);
			if (advSqR2 != null) advSqs.Add(advSqR2);
			return advSqR1;
		}
		else if (sameX || sameY || sameZ) {
			print("Quadrant rook move.");
			AdvancementSquare advSq;
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
			else {
				advSq = null;
			}
			if (advSq != null) advSqs.Add(advSq);
			return advSq;
		}
		else {
			print("Not in a rook plane.");
			//validAdvSq = false;
			return null;
		}
	}

	// Called by HighLightSquareByGrid.SetSrcDstSquares().
	public List<AdvancementSquare> AdvSqs(Vector3Int srcSquare, Vector3Int dstSquare)
	{
		List<AdvancementSquare> advSqs = new List<AdvancementSquare>();

		this.srcSquare = srcSquare;
		this.dstSquare = dstSquare;
		print("AdvSq: " + srcSquare + " / " + dstSquare);

		bool sameX = srcSquare.x == dstSquare.x;
		bool sameY = srcSquare.y == dstSquare.y;
		bool sameZ = srcSquare.z == dstSquare.z;

		if ((sameX && sameY) || (sameX && sameZ) || (sameY && sameZ)) {
			print("Linear rook move.");
			AdvancementSquare advSqR1;
			AdvancementSquare advSqR2;
			if (sameX && sameY) {
				advSqR1 = new RookAdvSqRect("RightVertical", srcSquare, dstSquare);
				print("Rook right vertical linear move, Y varies; number of perimeters = " + advSqR1.Perims);
				advSqR2 = new RookAdvSqRect("LeftVertical", srcSquare, dstSquare);
				//print("Rook left vertical linear move, Z varies; number of perimeters = " + advSqR2.Perims);
			} else if (sameX && sameZ) {
				advSqR1 = new RookAdvSqRect("Horizontal", srcSquare, dstSquare);
				print("Rook horizontal linear move, Y varies; number of perimeters = " + advSqR1.Perims);
				advSqR2 = new RookAdvSqRect("RightVertical", srcSquare, dstSquare);
				//print("Rook right vertical linear move, Y varies; number of perimeters = " + advSqR2.Perims);
			} else if (sameY && sameZ) {
				advSqR1 = new RookAdvSqRect("LeftVertical", srcSquare, dstSquare);
				print("Rook left vertical linear move, X varies; number of perimeters = " + advSqR1.Perims);
				advSqR2 = new RookAdvSqRect("Horizontal", srcSquare, dstSquare);
				//print("Rook horizontal linear move, X varies; number of perimeters = " + advSqR2.Perims);
			} else {
				advSqR1 = null;
				advSqR2 = null;
			}
			if (advSqR1 != null) advSqs.Add(advSqR1);
			if (advSqR2 != null) advSqs.Add(advSqR2);
		}
		else if (sameX || sameY || sameZ) {
			print("Quadrant rook move.");
			AdvancementSquare advSq;
			if (sameZ) {
				advSq = new RookAdvSqQuad("Horizontal", srcSquare, dstSquare);
				print("Rook Horizontal plane, advancement square number of perimeters = " + advSq.Perims);
			} else if (sameX) {
				advSq = new RookAdvSqQuad("RightVertical", srcSquare, dstSquare);
				print("Rook RightVertical plane, advancement square number of perimeters = " + advSq.Perims);
			} else if (sameY) {
				advSq = new RookAdvSqQuad("LeftVertical", srcSquare, dstSquare);
				print("Rook LeftVertical plane, advancement square number of perimeters = " + advSq.Perims);
			} else {
				advSq = null;
			}
			if (advSq != null) advSqs.Add(advSq);
		}
		else {
			print("Not in a rook plane.");
		}
		return advSqs;
	}
}
