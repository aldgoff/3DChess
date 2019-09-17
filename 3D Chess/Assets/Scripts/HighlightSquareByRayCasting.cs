using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSquareByRayCasting : MonoBehaviour
{
    // Properties of squares. TODO: Rename script and deprecated self highligting.
    public Color baseColor;
	public bool enable;

    // Debug info.
    Vector3 target;
    bool debug = false;

    // Material list & square color.
    Material[] mat;
    Color originalColor;

    // Boundary of the square.
    float zNegEdge, xNegEdge, yNegEdge;
    float zPosEdge, xPosEdge, yPosEdge;

    // Minimize performance hit.
    bool isHighlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        MeshRenderer myMesh = GetComponent<MeshRenderer>();
        mat = myMesh.materials;

        if(mat.Length > 1)
        {
            print("Assert failed, array of materials is longer than 1.");
        }

        baseColor = originalColor = mat[0].GetColor("_Color");

        // Boundary of the square.
        zNegEdge = transform.position.z - transform.localScale.z / 2;
        zPosEdge = transform.position.z + transform.localScale.z / 2;

        xNegEdge = transform.position.x - transform.localScale.x / 2;
        xPosEdge = transform.position.x + transform.localScale.x / 2;

        yNegEdge = transform.position.y - transform.localScale.y;
        yPosEdge = transform.position.y + transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // From camera to mouse.

		if(!enable) {
			return;
		}

		RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // Ray hit a GameObject (square, piece, etc.).
        {
            Vector3 point = hit.point;

            bool withinZ = zNegEdge < point.z && point.z < zPosEdge;
            bool withinX = xNegEdge < point.x && point.x < xPosEdge;
            bool withinY = yNegEdge < point.y && point.y < yPosEdge;

            if (withinZ && withinX && withinY) // Hit was this square.
            {
                if (!isHighlighted) // If not yet highlighted, highlight it.
                {
                    mat[0].SetColor("_Color", Color.yellow); // Highlight it.
                    isHighlighted = true;
                    if (debug) print("Highlight square at " + target);
                }
            }
            else if (isHighlighted) // Mouse off this square, unhighlight.
            {
                mat[0].SetColor("_Color", originalColor);
                isHighlighted = false;
                if (debug) print("Mouse off square.");
            }
        }
        else if(isHighlighted) // Mouse off board, unhighlight.
        {
            mat[0].SetColor("_Color", originalColor);
            isHighlighted = false;
            if (debug) print("Mouse off board.");
        }
    }
}
