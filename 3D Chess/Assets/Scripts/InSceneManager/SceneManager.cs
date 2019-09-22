using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace project_status
{
	namespace chess_board
	{
		class GetRidOfEmptyNameSpaceWarning { /* Sheeze, inside so comments WON'T line up! */ };
		/* Alternating black/gray squares.
		 * Detect raycast hits on squares.
		 * Planes support base colors tinted on point, line, and quad.
		 *   Rook in red.
		 *   Bishop in green
		 * Refactored to improve base plane cohesion.
		 * Added forward/backward and left/right options for bishop planes.
		 * Double click idiom to freeze highlighted planes so cursor can change levels.
		 * TODO: fix double click idiom to deal with UI.
		 * Refactored, eliminated obsolete code.
		*/
		namespace highlight_advancement_squares
		{
			/* Complete src & dst square logic.
			 * All 12 rook quadrants.
			 * Full tinting; quad & linear.
			 * Do not highlight over the dstSquare.
			 * Advancement square class hierarchy.
			 * Compute perimeters.
			 * Perimeters presented on later frames.
			 * Ripple effect working.
			 */
			class GetRidOfEmptyNameSpaceWarning { /* Sheeze, inside so comments WON'T line up! */ };
		}
	}
	namespace user_interface
	{
		/* Have draft of UI.
		 * Can create standard size boards:
		 *   8x8x8
		 *   8x8x19
		 *   10x10x10
		 *   5x5x5
		 * Button to scan board in array order.
		 * Drop down for piece/planes to highlight
		 * TODO triage doubling of plane options in drop down menu.
		 * Sub folders "Buttons" and "Dropdowns" in the UI Panel object.
		 */
		namespace board_size
		{
			/* Create button and dropdown UI elements.
			 * Add a CreateBoard button and a BoardSize dropdown GameObject in the UI.
			 * Add a CreateBoard script with a CreateSizedBoard pubic method.
			 * Add a Dropdown object and a ChessBoard3D object to the CreateBoard script.
			 * Populate those fields in the "Create Board (Script)" component in the Inspector.
			 * Confirm that the button can get the selected option in the dropdown menu.
			 * Start calling and growing the properties based board creation.
			 */
			class GetRidOfEmptyNameSpaceWarning { };
		}
	}
}

// SceneManager GameObject includes these scripts as components:
//   SceneManager
//   PlanesBehavior
//   PlanesSelection
//   moreToCome

public class SceneManager : MonoBehaviour
{
	public ChessBoard3D chessBoard;
    public HighLightSquareByGrid highLight;
	// Seampoint: add another object; => another script with another new class.

    // Start is called before the first frame update
    void Start()
    {
		print("----- SceneManager.Start() -----");
		Debug.Log("Testing object reference chain - " + chessBoard.properties.boardVerticalSep);
	}

	// Update is called once per frame
	void Update()
    {

    }
}
