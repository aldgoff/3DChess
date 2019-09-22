using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Coupling: just Vector3Int.
using System;

// Advancement Square class hierarchy for 3D Chess.
// Assumes knowledge of the planar move rule for 3D Chess.
// Code independent of Unity axis definitions.

// Logic: compute advancement squares perimeter by perimeter:
//	 Quad: 3, 5, 7, 9, ...
//	 Rect: 5, 9, 13, 17 ...
// Uses Strategy pattern to abstract differences between the 3 base pieces: Rook, Bishop, & Duke, and quadrant versus linear moves.

// Base class.
public class AdvancementSquare
{
	public int Perims { get; set; } // Is perimeters.Count, but avoids coupling API to List implementation.

	public List<Vector3Int[]> perimeters; // Populated by daughter ctors. TODO: add First/Next/Last/Prev methods to complete decoupling.

	protected Vector3Int nullSquare = new Vector3Int(-1, -1, -1); // TODO: remove dup nullSquare, move up.
}

// Seampoint - potential; may add intermediate classes for quadrant and rectangle advancement squares.

// Code tradeoff; since no effective design pattern for arg reordering, allowing some duplication to maintain clarity.
public class RookAdvSqQuad : AdvancementSquare // Complete.
{
	private int X, Y, Z; // Equal 1 or dstSq.<axis> - srcSq.<axis>.

	// Create the set of perimeters which define the advancement square.
	public RookAdvSqQuad(string plane, Vector3Int srcSq, Vector3Int dstSq)
	{
		perimeters = new List<Vector3Int[]>();

		if (plane == "Horizontal") {
			Z = 1;
			X = dstSq.x - srcSq.x;
			Y = dstSq.y - srcSq.y;
			Perims = Math.Max(Math.Abs(X), Math.Abs(Y));

			int xSign = (X > 0) ? +1 : -1;
			int ySign = (Y > 0) ? +1 : -1;

			// Perimeters are built from both edges toward the corner tip.
			for (int l = 1; l <= Perims; l++) {
				Vector3Int[] perim = new Vector3Int[2 * l + 1];
				for (int r = 0; r < l; r++) {	// Right edge.
					int o = 2 * l - r;			// Opposite edge.
					perim[r] = new Vector3Int(srcSq.x + xSign * l, srcSq.y + ySign * r, srcSq.z);   // Think 0,1,2,3.
					perim[o] = new Vector3Int(srcSq.x + xSign * r, srcSq.y + ySign * l, srcSq.z);   // Think 8,7,6,5.
					if (r == l - 1) {			// Corner tip.
						perim[l] = new Vector3Int(srcSq.x + xSign * l, srcSq.y + ySign * l, srcSq.z);    // Think 4 (l) total is 9.
					}
					if (perim[r] == dstSq) perim[r] = nullSquare; // Sentry so display code doesn't override dstSquare.
					if (perim[o] == dstSq) perim[o] = nullSquare;
					if (perim[l] == dstSq) perim[l] = nullSquare;
				}
				perimeters.Add(perim);
			}
		}
		else if (plane == "RightVertical") {
			X = 1;
			Y = dstSq.y - srcSq.y;
			Z = dstSq.z - srcSq.z;
			Perims = Math.Max(Math.Abs(Y), Math.Abs(Z));

			int ySign = (Y > 0) ? +1 : -1;
			int zSign = (Z > 0) ? +1 : -1;

			// Perimeters are built from both edges toward the corner tip.
			for (int l = 1; l <= Perims; l++) {
				Vector3Int[] perim = new Vector3Int[2 * l + 1];
				for (int r = 0; r < l; r++) {   // Right edge.
					int o = 2 * l - r;          // Opposite edge.
					perim[r] = new Vector3Int(srcSq.x, srcSq.y + ySign * l, srcSq.z + zSign * r);   // Think 0,1,2,3.
					perim[o] = new Vector3Int(srcSq.x, srcSq.y + ySign * r, srcSq.z + zSign * l);   // Think 8,7,6,5.
					if (r == l - 1) {           // Corner tip.
						perim[l] = new Vector3Int(srcSq.x, srcSq.y + ySign * l, srcSq.z + zSign * l);    // Think 4 (l) total is 9.
					}
					if (perim[r] == dstSq) perim[r] = nullSquare; // Sentry so display code doesn't override dstSquare.
					if (perim[o] == dstSq) perim[o] = nullSquare;
					if (perim[l] == dstSq) perim[l] = nullSquare;
				}
				perimeters.Add(perim);
			}
		}
		else if (plane == "LeftVertical") {
			Y = 1;
			X = dstSq.x - srcSq.x;
			Z = dstSq.z - srcSq.z;
			Perims = Math.Max(Math.Abs(X), Math.Abs(Z));

			int xSign = (X > 0) ? +1 : -1;
			int zSign = (Z > 0) ? +1 : -1;

			// Perimeters are built from both edges toward the corner tip.
			for (int l = 1; l <= Perims; l++) {
				Vector3Int[] perim = new Vector3Int[2 * l + 1];
				for (int r = 0; r < l; r++) {   // Right edge.
					int o = 2 * l - r;          // Opposite edge.
					perim[r] = new Vector3Int(srcSq.x + xSign * l, srcSq.y, srcSq.z + zSign * r);		// Think 0,1,2,3.
					perim[o] = new Vector3Int(srcSq.x + xSign * r, srcSq.y, srcSq.z + zSign * l);		// Think 8,7,6,5.
					if (r == l - 1) {			// Corner tip.
						perim[l] = new Vector3Int(srcSq.x + xSign * l, srcSq.y, srcSq.z + zSign * l);   // Think 4 (l) total is 9.
					}
					if (perim[r] == dstSq) perim[r] = nullSquare; // Sentry so display code doesn't override dstSquare.
					if (perim[o] == dstSq) perim[o] = nullSquare;
					if (perim[l] == dstSq) perim[l] = nullSquare;
				}
				perimeters.Add(perim);
			}
		}
		else {
			Debug.Log("*** Error: Unknown rook plane, should never happen.");
			// TODO: How do you do asserts in C#?
		}

		Perims = perimeters.Count;
	}
}

public class RookAdvSqRect : AdvancementSquare
{
	// TODO: class RookAdvSqRect.
}

// Seampoint - add Bishop & Duke.

// TODO: Probably should add Factory class so user code can avoid testing the drop down menu for piece.
// TODO: Need API for Factory to construct quad versus linear advancement squares.
