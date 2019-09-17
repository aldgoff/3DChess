using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Implement UI dropdown for planes selection.
public class PlanesSelection : MonoBehaviour
{
    public Dropdown basePlanes;
	public Text selectedPlane;

	// Each list is a seampoint, can add more items w/o having to change code.
	public List<string> rookPlanes = new List<string>() { "Rook Planes", "  Horizontal", "  LeftVertical", "  RightVertical" };
	public List<string> bishopPlanes = new List<string>() { "Bishop Planes", "  ForwardBackward", "  LeftRight",
															"  ForwardSlant", "  BackwardSlant", "  LeftSlant", "  RightSlant" };
	public List<string> dukePlanes = new List<string>() { "Duke Planes", "  Z Pair", "  X Pair", "  Y Pair" };

	public List<string> queenPlanes = new List<string>() { "Queen Planes", "  RookBishop", "  RookDuke", "  BishopDuke" };
	public List<string> stackPlanes = new List<string>() { "Stack Planes" };

	List<string> knightSpots = new List<string>() { "Knight Spots" };
	List<string> kingSpots   = new List<string>() { "King Spots" };
	List<string> pawnSquares = new List<string>() { "Pawn Squares" };

	public void PlanesDropdownIndexChanged(int index)
	{
		int ind = index - 1;

		int basePlaneCount   = rookPlanes.Count + bishopPlanes.Count + dukePlanes.Count;
		int powerPiecesCount = basePlaneCount + queenPlanes.Count + stackPlanes.Count;

		if (ind < rookPlanes.Count) {
			selectedPlane.text = rookPlanes[ind];
		}
		else if (ind < rookPlanes.Count + bishopPlanes.Count) {
			selectedPlane.text = bishopPlanes[ind - rookPlanes.Count];
		}
		else if (ind < rookPlanes.Count + bishopPlanes.Count + dukePlanes.Count) {
			selectedPlane.text = dukePlanes[ind - rookPlanes.Count - bishopPlanes.Count];
		}
		else if (ind < basePlaneCount + queenPlanes.Count) {
			selectedPlane.text = queenPlanes[ind - basePlaneCount];
		}
		else if (ind < basePlaneCount + queenPlanes.Count + stackPlanes.Count) {
			selectedPlane.text = stackPlanes[ind - basePlaneCount - queenPlanes.Count];
		}
		else if (ind < powerPiecesCount + knightSpots.Count) {
			selectedPlane.text = knightSpots[ind - powerPiecesCount];
		}
		else if (ind < powerPiecesCount + knightSpots.Count + kingSpots.Count) {
			selectedPlane.text = kingSpots[ind - powerPiecesCount - knightSpots.Count];
		}
		else if (ind < powerPiecesCount + knightSpots.Count + kingSpots.Count + pawnSquares.Count) {
			selectedPlane.text = pawnSquares[ind - powerPiecesCount - knightSpots.Count - kingSpots.Count ];
		}

		if(ind == 0) { // RookPlanes.
			selectedPlane.color = Color.red;
		}

		print("PlanesSelection.PlanesDropdownIndexChanged(" + index + ")");
	}

	void Start()
    {
		print("----- PlanesSelection.Start() -----");
		print("*** Rook plane count   = " + rookPlanes.Count);
		print("*** Bishop plane count = " + bishopPlanes.Count);
		print("*** Duke plane count   = " + dukePlanes.Count);
		PopulateList();
    }

    void PopulateList()
    {
		basePlanes.AddOptions(rookPlanes);
        basePlanes.AddOptions(bishopPlanes);
        basePlanes.AddOptions(dukePlanes);

		basePlanes.AddOptions(queenPlanes);
		basePlanes.AddOptions(stackPlanes);

		basePlanes.AddOptions(knightSpots);
		basePlanes.AddOptions(kingSpots);
		basePlanes.AddOptions(pawnSquares);
	}

	void Update() {}
}
