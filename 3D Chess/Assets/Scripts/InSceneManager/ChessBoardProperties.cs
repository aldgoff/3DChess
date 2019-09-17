using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Is a component in the ChessBoardProperties GameObject.
// Used by ChessBoard3D as a public global variable.
// Properties of a 3D chess board fixed at create board time.
public class ChessBoardProperties : MonoBehaviour
{
	ChessBoardProperties() { print("ChessBoardProperties.ctor()"); }
	~ChessBoardProperties() { print("ChessBoardProperties.dtor()"); }

	public Vector3Int size;
	public Vector3Int locWhiteK11sq;

	// TODO: Un hardcode these.
	public float squareSep = 1.0f;
	public int boardVerticalSep = 2;

	// These all set by ChessBoard3D.Start() based on square transforms.
	public float squareSize;
	public float squareThickness;

	public Vector2 boardXedges;
	public Vector2 boardYedges;
	public Vector2 boardZedges;

	// This gets executed only once, as part of generating a 3D board.
	public void ComputeBoardEdges()
	{
		boardXedges = new Vector2(-size.x / 2.0f, size.x / 2.0f);
		boardYedges = new Vector2(-size.y / 2.0f, size.y / 2.0f);
		int zOffset = (size.z - 1) * boardVerticalSep / 2;
		boardZedges = new Vector2(-zOffset, zOffset);

		print("X | Y | Z Edges = " + boardXedges + " | " + boardYedges + " | " + boardZedges);
	}

	// Might not be needed.
	void Start() {}
    void Update() {}
}
