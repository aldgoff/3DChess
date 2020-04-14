using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	public GameObject square; // Can display a mesh material color, respond to rayhits, and rotate.

	private Material mat; // Material that covers the cube's (square) mesh.
	public Color baseColor; // Typcially white or a gray, and maybe cyan for White K11 square.

	private List<AdvancementSquare> advSqs = new List<AdvancementSquare>(); // TODO: Make ref to save memory.

	public Cell(GameObject square)
	{
		this.square = square;
		MeshRenderer aMesh = square.GetComponent<MeshRenderer>();
		mat = aMesh.material;
	}

	public void SetBaseColor(Color color)
	{
		baseColor = color;
		mat.SetColor("_Color", baseColor);
	}

	public void HighlightCell(Color color)
	{
		mat.SetColor("_Color", color);
	}

	public void UnhighlightCell()
	{
		mat.SetColor("_Color", baseColor);
	}

	public void Aquire(AdvancementSquare advSq) // Advancement square now covers cell - display proper color & tint.
    {
		Debug.Log("Cell.Aquire() - add to list and display proper color & tint.");
		advSqs.Add(advSq);
		ColorAndTint();
		Debug.Log("Number of AdvSqs = " + advSqs.Count);
    }

    public void Divest(AdvancementSquare advSq) // Advancement square no longer covers cell - display last color & tint.
    {
		Debug.Log("Cell.Divest() - extract from list and display last color & tint.");
		advSqs.Remove(advSq); // TODO: Trap for error - advSq not in list.
		ColorAndTint();
		Debug.Log("Number of AdvSqs = " + advSqs.Count);
	}

	private void ColorAndTint()
	{
		if (advSqs.Count > 0) {
			int lastIndex = advSqs.Count - 1;
			AdvancementSquare advSq = advSqs[lastIndex];

			// TODO: Determine plane type (Rook, Bishop, Duke).
			// TODO: Determine square type (black, white).
			// TODO: Find cell in perimeter, determine whether point, line, or quad.
			// TODO: Color and tint appropriately.
		}
		else {
			// TODO: Restore color to originalColor (white/black).
		}
	}

	// TODO: Add Piece.
}
