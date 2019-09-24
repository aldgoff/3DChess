using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Public member in SceneManager.

public class HighLightSquareByGrid : MonoBehaviour
{
	public Dropdown mouseBehavior;
	public int mouseBehaviorIndex;

	public ChessBoard3D chessBoard;
	public PlanesSelection planesSelection; // Dropdown script.
	public HighlightRookPlanes highlightRookPlanes;
	public HighlightBishopPlanes highlightBishopPlanes;

	// Used to highlight planes.
	Vector3Int nullSquare = new Vector3Int(-1, -1, -1); // TODO: make const.
	Vector3Int prevSquare;

	// Used to highlight advancement squares.
	Vector3Int srcSquare;
	Vector3Int dstSquare;
	bool haveRookAdvSq;

	// Deprecated.
	DemoRookAdvSq demoRAS;

	private bool debug;

	public void MouseBehaviorDropdownIndexChanged(int index)
	{
		mouseBehaviorIndex = index; // TODO: So we can see in Inspector at runtime - not working.
		print("Mouse behavior dropdown index = " + mouseBehaviorIndex);
		print("  MouseBehavior dropdown = " + mouseBehavior.captionText.text);
	}

	// Start is called before the first frame update
	void Start()
	{
		print("----- HighLightSquareByGrid.Start() -----");
		print("  Initial chessBoard.size = " + chessBoard.size);
		print("  Initial chessBoard.chessBoardProperties.size = " + chessBoard.properties.size);
		prevSquare = nullSquare;

		srcSquare = nullSquare;
		dstSquare = nullSquare;

		// Deprecated.
		demoRAS = new DemoRookAdvSq();
		demoRAS.InitPerimeters();
	}

	Vector3Int FindRaycastSquare(RaycastHit hit)
	{
		Vector3 point = hit.point;

		// Remember that chessboard coordiantes are permutation of Unity coordinates.
		ChessBoardProperties props = chessBoard.properties; // New, SRP compliant.

		Vector2 boardXedges = props.boardXedges;
		Vector2 boardYedges = props.boardYedges;
		Vector2 boardZedges = props.boardZedges;
		Vector3Int size = props.size;
		float squareThickness = props.squareThickness;
		int boardVerticalSep = props.boardVerticalSep;

		if(debug) {
			print("boardXedges = " + boardXedges);
			print("boardYedges = " + boardYedges);
			print("boardZedges = " + boardZedges);
			print("size = " + size);
			print("squareThickness = " + squareThickness);
			print("boardVerticalSep = " + boardVerticalSep);
			print("----------");
		}

		bool withinX = boardXedges[0] < point.z && point.z < boardXedges[1];
		bool withinY = boardYedges[0] < point.x && point.x < boardYedges[1];
		bool withinZ = boardZedges[0] < point.y && point.y < boardZedges[1] + squareThickness;

		if (!(withinX && withinY && withinZ)) {
			print("*** nullSquare ***");
			return nullSquare;
		}

		int zCoord = 0;
		int xCoord = 0;
		int yCoord = 0;

		// Find level (horizontal).
		for (int k = 0; k < size.z; k++) {
			float loZ = boardZedges[0] + k * boardVerticalSep;
			float hiZ = boardZedges[0] + (k + 1 - squareThickness) * boardVerticalSep;
			withinZ = loZ < point.y && point.y < hiZ;
			if (withinZ) {
				zCoord = k;
				break;
			}
		}
		if (!withinZ) {
			return nullSquare;
		}

		// Find left vertical.
		for (int i = 0; i < size.x; i++) {
			withinX = boardXedges[0] + i < point.z && point.z < boardXedges[0] + i + 1;
			if (withinX) {
				xCoord = i;
				break;
			}
		}

		// Find right vertical.
		for (int j = 0; j < size.y; j++) {
			withinY = boardYedges[0] + j < point.x && point.x < boardYedges[0] + j + 1;
			if (withinY) {
				yCoord = j;
				break;
			}
		}

		return new Vector3Int(xCoord, yCoord, zCoord);
	}

	bool frozen = true;

	// Update is called once per frame
	void Update()
	{
		// TODO: Inefficient, should only query when it changes.
		switch (mouseBehavior.captionText.text) {
		case "Select Mouse Behavior":
			break;
		case "Source/Destination":
			demoRAS.RookPlaneAdvancementSquare(chessBoard.squares);
			break;

		case "Highlight Planes on Mouseover":
			HighlightPlanes();
			break;
		case "Animate Advancement Squares":
			SetSrcDstSquares();
			break;

		default:
			print("Unknown Mouse Behavior dropdown option - " + mouseBehavior.captionText.text);
			break;
		}
	}

	void SetSrcDstSquares()
	{
		Color srcColor = Color.yellow;
		Color dstColor = Color.magenta;
		string plane = planesSelection.selectedPlane.text;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // From camera to mouse.
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) { // Ray hit a GameObject (square, piece, etc.).
			//print("Running set source/destination squares()");

			// Find raycast square. The nullSquare (-1,-1,-1) means hit is not on any square.
			Vector3Int raycastSquare = FindRaycastSquare(hit);
			//print("Square hit = " + raycastSquare);

			// Below is working, but verbose. TODO refactor src/dst square selection logic.
			// Click marks source square.
			if (Input.GetMouseButtonUp(1)) {
				print("MouseButton 1 up; raycastSquare = " + raycastSquare + " srcSq was " + srcSquare);

				if (srcSquare == nullSquare && dstSquare == nullSquare /* Fresh */) {	// Fresh srcSq.
					srcSquare = raycastSquare;
					print("Highligh fresh srcSq  - " + srcSquare);
					Highlight(srcSquare, srcColor);
				} else if (srcSquare != nullSquare && dstSquare == nullSquare) {
					if (srcSquare == raycastSquare) {							// Toggle srcSq off.
						print("Clear current srcSq - " + srcSquare);
						if (planesSelection.rookPlanes.Contains(plane)) {
							highlightRookPlanes.HighlightSquare(srcSquare, false, "Point");
						}
						//Unhighlight(srcSquare);
						srcSquare = nullSquare;
					}
					else {														// Move srcSq.
						print("Clear previous srcSq - " + srcSquare);
						Unhighlight(srcSquare);
						srcSquare = raycastSquare;
						print("Highlight new srcSq - " + srcSquare);
						Highlight(srcSquare, srcColor);
					}
				}
				else if(srcSquare == nullSquare && dstSquare != nullSquare) {   // Fresh srcSq with dstSq.
					if (raycastSquare == dstSquare) {                           // Src & dst squares must be different.
						// Do nothing.
					}
					else {
						srcSquare = raycastSquare;
						print("Highligh fresh srcSq with dstSq  - " + srcSquare);
						Highlight(srcSquare, srcColor);
					}
				}
				else {
					if(raycastSquare == dstSquare) {							// Src & dst squares must be different.
						// Do nothing.
					}
					else if (srcSquare == raycastSquare) {                      // Toggle srcSq off.
						print("Clear current srcSq with dstSq - " + srcSquare);
						Unhighlight(srcSquare);
						srcSquare = nullSquare;
					}
					else {														// Move srcSq.
						print("Clear previous srcSq with dstSq - " + srcSquare);
						Unhighlight(srcSquare);
						srcSquare = raycastSquare;
						print("Highlight new srcSq with dstSq - " + srcSquare);
						Highlight(srcSquare, srcColor);
					}
				}
			}

			// Click marks destination square.
			if (Input.GetMouseButtonUp(0)) {
				print("MouseButton 0 up; raycastSquare = " + raycastSquare + " dstSq was " + dstSquare);

				// No srcSquare, manage dstSquare's highlighting, but no advancement square.
				if (srcSquare == nullSquare) {
					if (dstSquare == nullSquare) {							// Fresh.
						dstSquare = Highlight(raycastSquare, dstColor);
					}
					else {
						if (dstSquare == raycastSquare) {					// Toggle.
							dstSquare = Unhighlight(dstSquare); 
						}
						else {												// Move.
							Unhighlight(dstSquare);
							dstSquare = Highlight(raycastSquare, dstColor);
						}
					}
				}
				// SrcSquare is highlighted, manage advancement square(s).
				else {
					if (dstSquare == nullSquare) {
						if (raycastSquare != srcSquare) {                   // Fresh.
							print("Fresh dstSquare");
							dstSquare = Highlight(raycastSquare, dstColor);
							if (planesSelection.rookPlanes.Contains(plane)) {
								if(highlightRookPlanes.AdvSq(srcSquare, dstSquare)) {
									StartCoroutine(highlightRookPlanes.ShowAdvSqByPerimeter());
									haveRookAdvSq = true;
								}
							}
						}
					}
					else {
						if (raycastSquare != srcSquare) {
							if (dstSquare == raycastSquare) {               // Toggle.
								print("Toggle dstSquare");
								if (planesSelection.rookPlanes.Contains(plane)) {
									if (haveRookAdvSq) {
										highlightRookPlanes.HighlightSquare(dstSquare, false);
									}
									else {
										dstSquare = Unhighlight(dstSquare);
									}
								}
								dstSquare = nullSquare;
							} else {                                        // Move.
								print("Move dstSquare");
								Unhighlight(dstSquare);
								if (planesSelection.rookPlanes.Contains(plane)) {
									highlightRookPlanes.EraseAdvSq();
									haveRookAdvSq = false;
								}
								dstSquare = Highlight(raycastSquare, dstColor);
								//if (planesSelection.rookPlanes.Contains(plane)) {
								//	if (highlightRookPlanes.AdvSq(srcSquare, dstSquare)) {
								//		haveRookAdvSq = true;
								//	}
								//}
							}
						}
					}
				}
			}
		}
		else { // Clear src & dst squares off the board.
			if (Input.GetMouseButtonUp(1)) {
				if (srcSquare != nullSquare) {
					print("Clear board of srcSquare - " + srcSquare);
					srcSquare = Unhighlight(srcSquare);
				}
			}
			if (Input.GetMouseButtonUp(0)) {
				if (dstSquare != nullSquare) {
					print("Clear board of dstSquare - " + dstSquare);
					dstSquare = Unhighlight(dstSquare);
				}
				if (haveRookAdvSq) {
					StartCoroutine(highlightRookPlanes.ClearAdvSqByPerimeter());
				}
			}
		}
	}

	private Vector3Int Highlight(Vector3Int sq, Color color)
	{
		HighlightSquareByRayCasting sqScriptClass;

		sqScriptClass = chessBoard.squares[sq.x, sq.y, sq.z].GetComponent<HighlightSquareByRayCasting>();
		Material[] theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
		theMat[0].SetColor("_Color", color);

		return sq;
	}

	private Vector3Int Unhighlight(Vector3Int sq)
	{
		HighlightSquareByRayCasting sqScriptClass;

		sqScriptClass = chessBoard.squares[sq.x, sq.y, sq.z].GetComponent<HighlightSquareByRayCasting>();
		Material[] theMat = sqScriptClass.GetComponent<MeshRenderer>().materials;
		theMat[0].SetColor("_Color", sqScriptClass.baseColor);

		return nullSquare;
	}

	void HighlightPlanes()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // From camera to mouse.

		string plane = planesSelection.selectedPlane.text;

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) { // Ray hit a GameObject (square, piece, etc.).

			print("Plane selected = " + plane);

			// Find raycast square. The nullSquare (-1,-1,-1) means hit is not on any square.
			Vector3Int raycastSquare = FindRaycastSquare(hit);
			print("  Square hit = " + raycastSquare);

			// TODO: Add if's based on plane selection dropdown.
			if (raycastSquare == nullSquare) {
				if (!frozen) {
					if (prevSquare != nullSquare) { // Unhighlite prevSquare.
						if (planesSelection.rookPlanes.Contains(plane)) {
							highlightRookPlanes.HighlightPlanes(plane, prevSquare, Color.clear);
						}
						if (planesSelection.bishopPlanes.Contains(plane)) {
							highlightBishopPlanes.HighlightPlanes(plane, prevSquare, Color.clear);
						}
						// Seampoint - add other bases pieces.

						prevSquare = nullSquare;
					}
				}
			} else if (raycastSquare != prevSquare) {
				if (!frozen) {
					if (prevSquare != nullSquare) { // Unhighlite prevSquare.
						if (planesSelection.rookPlanes.Contains(plane)) {
							highlightRookPlanes.HighlightPlanes(plane, prevSquare, Color.clear);
						}
						if (planesSelection.bishopPlanes.Contains(plane)) {
							highlightBishopPlanes.HighlightPlanes(plane, prevSquare, Color.clear);
						}
						// Seampoint - add other bases pieces.
					}
					// Highlite raycastSquare.
					if (planesSelection.rookPlanes.Contains(plane)) {
						highlightRookPlanes.HighlightPlanes(plane, raycastSquare, Color.red);
					}
					if (planesSelection.bishopPlanes.Contains(plane)) {
						highlightBishopPlanes.HighlightPlanes(plane, raycastSquare, Color.green);
					}
					// Seampoint - add other bases pieces.

					prevSquare = raycastSquare;
				}
			}

		} else {
			if (!frozen) {
				if (prevSquare != nullSquare) { // Unhighlite prevSquare.
					if (planesSelection.rookPlanes.Contains(plane)) {
						highlightRookPlanes.HighlightPlanes(plane, prevSquare, Color.clear);
					}
					if (planesSelection.bishopPlanes.Contains(plane)) {
						highlightBishopPlanes.HighlightPlanes(plane, prevSquare, Color.clear);
					}
					// Seampoint - add other bases pieces.

					prevSquare = nullSquare;
				}
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			frozen = !frozen;
		}
	}
}

// This is a demo only - very hard coded, mono color on both black & white squares.
// One horizontal rook advancement square on level 2.
// Deprecated.
public class DemoRookAdvSq
{
	Vector2Int[] perimeter0 = new Vector2Int[1];
	Vector2Int[] perimeter1 = new Vector2Int[3];
	Vector2Int[] perimeter2 = new Vector2Int[5];
	Vector2Int[] perimeter3 = new Vector2Int[7];
	Vector2Int[] perimeter4 = new Vector2Int[9];
	Vector2Int[] perimeter5 = new Vector2Int[11];
	Vector2Int[] perimeter6 = new Vector2Int[13];
	Vector2Int[] perimeter7 = new Vector2Int[15];

	public void InitPerimeters()
	{
		perimeter0[0] = new Vector2Int(0, 0);

		for (int i = 0; i < 1; i++) {
			perimeter1[i] = new Vector2Int(1, i);
			perimeter1[1 * 2 - i] = new Vector2Int(i, 1);
		}
		perimeter1[1] = new Vector2Int(1, 1);

		for (int i = 0; i < 2; i++) {
			perimeter2[i] = new Vector2Int(2, i);
			perimeter2[2 * 2 - i] = new Vector2Int(i, 2);
		}
		perimeter2[2] = new Vector2Int(2, 2);

		for (int i = 0; i < 3; i++) {
			perimeter3[i] = new Vector2Int(3, i);
			perimeter3[2 * 3 - i] = new Vector2Int(i, 3);
		}
		perimeter3[3] = new Vector2Int(3, 3);

		for (int i = 0; i < 4; i++) {
			perimeter4[i] = new Vector2Int(4, i);
			perimeter4[2 * 4 - i] = new Vector2Int(i, 4);
		}
		perimeter4[4] = new Vector2Int(4, 4);

		for (int i = 0; i < 5; i++) {
			perimeter5[i] = new Vector2Int(5, i);
			perimeter5[2 * 5 - i] = new Vector2Int(i, 5);
		}
		perimeter5[5] = new Vector2Int(5, 5);

		for (int i = 0; i < 6; i++) {
			perimeter6[i] = new Vector2Int(6, i);
			perimeter6[2 * 6 - i] = new Vector2Int(i, 6);
		}
		perimeter6[6] = new Vector2Int(6, 6);

		for (int i = 0; i < 7; i++) {
			perimeter7[i] = new Vector2Int(7, i);
			perimeter7[2 * 7 - i] = new Vector2Int(i, 7);
		}
		perimeter7[7] = new Vector2Int(7, 7);
	}

	int frameDelay = 0;
	int perimeter = 0;

	Color color = Color.red;

	public void RookPlaneAdvancementSquare(GameObject[,,] squares)
	{
		MeshRenderer theMesh;
		Material[] theMat;

		if (frameDelay == 0) {
			frameDelay = 20;
			if (perimeter == 0) {
				theMesh = squares[perimeter0[0][0], perimeter0[0][1], 2].GetComponent<MeshRenderer>();
				theMat = theMesh.materials;
				theMat[0].SetColor("_Color", color);
			} else if (perimeter == 1) {
				for (int i = 0; i < perimeter1.Length; i++) {
					theMesh = squares[perimeter1[i][0], perimeter1[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 2) {
				for (int i = 0; i < perimeter2.Length; i++) {
					theMesh = squares[perimeter2[i][0], perimeter2[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 3) {
				for (int i = 0; i < perimeter3.Length; i++) {
					theMesh = squares[perimeter3[i][0], perimeter3[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 4) {
				for (int i = 0; i < perimeter4.Length; i++) {
					theMesh = squares[perimeter4[i][0], perimeter4[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 5) {
				for (int i = 0; i < perimeter5.Length; i++) {
					theMesh = squares[perimeter5[i][0], perimeter5[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 6) {
				for (int i = 0; i < perimeter6.Length; i++) {
					theMesh = squares[perimeter6[i][0], perimeter6[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 7) {
				for (int i = 0; i < perimeter7.Length; i++) {
					theMesh = squares[perimeter7[i][0], perimeter7[i][1], 2].GetComponent<MeshRenderer>();
					theMesh.materials[0].SetColor("_Color", color);
				}
			} else if (perimeter == 8) {
				color = Color.clear;
				perimeter = -1;
			}
			perimeter++;
		} else {
			frameDelay--;
		}
	}
}

