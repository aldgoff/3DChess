using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/* Create an 8x8x8 board.
 * Board coordinates are a permutation of Unity coordinates.
 *  Board Unity
 *    x     z
 *    y     x
 *    z     y
 * Squares are spaced 1 unit apart horizontally.
 * Currently spaced 2 units vertically. TODO: make variable under user control.
 * Squares are 0.96 x 0.96 x 0.1.
 * Center of board is placed at Unity origin (0,0,0).
 * TODO: Code architecture.
 */

// Deprecated.
public class ButtonHandler : MonoBehaviour
{
    public ChessBoard3D chessBoard;

    public GameObject[,,] squares = new GameObject[8,8,8];
    private bool boardConstructed = false;

    public void SetText(string text)
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        txt.text = text;
    }

    public void CreateBoard()
    {
        chessBoard.size = new Vector3Int(8, 8, 8);
		//chessBoard.CreateBoard();

		boardConstructed = true;

		//chessBoard.properties.CreateBoard(chessBoard.size, new Vector3Int(0, 0, 4));
		//print("chessBoard.properties.GetDimensions() = " + chessBoard.properties.GetDimensions());
		//print("chessBoard.properties.GetLocWhiteK11() = " + chessBoard.properties.GetLocWhiteK11());
		//print("chessBoard.properties.IsBuilt() = " + chessBoard.properties.IsBuilt());

		//InitScan();
	}


	// Start is called before the first frame update
	void Start()
    {
        print("----- ButtonHandler.Start() -----");
        //chessBoard.size = new Vector3Int(6,6,6);
        //print("ButtonHandler.chessBoard.size = " + chessBoard.size);

        InitPerimeters();
    }

    // Update is called once per frame
    void Update()
    {
        //MarkRookPlane();
        RookPlaneAdvancementSquare();
    }

    private MeshRenderer theMesh;
    private Material[] theMat;

    private bool done = false;
    void MarkRookPlane()
    {
        if (done) return;

        int y = 2;
        for(int x=0; x<8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                theMesh = squares[x, y, z].GetComponent<MeshRenderer>();
                theMat = theMesh.materials;
                theMat[0].SetColor("_Color", Color.red);
            }
        }

        done = true;
    }

    int frameDelay = 0;
    int perimeter = 0;

    Vector2Int[] perimeter0 = new Vector2Int[1];
    Vector2Int[] perimeter1 = new Vector2Int[3];
    Vector2Int[] perimeter2 = new Vector2Int[5];
    Vector2Int[] perimeter3 = new Vector2Int[7];
    Vector2Int[] perimeter4 = new Vector2Int[9];
    Vector2Int[] perimeter5 = new Vector2Int[11];
    Vector2Int[] perimeter6 = new Vector2Int[13];
    Vector2Int[] perimeter7 = new Vector2Int[15];

    void InitPerimeters()
    {
		print("======== perimeter = " + perimeter);

		perimeter0[0] = new Vector2Int(0, 0);

        for (int i = 0; i < 1; i++)
        {
            perimeter1[i] = new Vector2Int(1, i);
            perimeter1[1 * 2 - i] = new Vector2Int(i, 1);
        }
        perimeter1[1] = new Vector2Int(1, 1);

        for (int i = 0; i < 2; i++)
        {
            perimeter2[i] = new Vector2Int(2, i);
            perimeter2[2 * 2 - i] = new Vector2Int(i, 2);
        }
        perimeter2[2] = new Vector2Int(2, 2);

        for (int i = 0; i < 3; i++)
        {
            perimeter3[i] = new Vector2Int(3, i);
            perimeter3[2 * 3 - i] = new Vector2Int(i, 3);
        }
        perimeter3[3] = new Vector2Int(3, 3);

        for (int i = 0; i < 4; i++)
        {
            perimeter4[i] = new Vector2Int(4, i);
            perimeter4[2*4 - i] = new Vector2Int(i, 4);
        }
        perimeter4[4] = new Vector2Int(4, 4);

        for(int i=0; i<5; i++)
        {
            perimeter5[i]      = new Vector2Int(5, i);
            perimeter5[2*5 - i] = new Vector2Int(i, 5);
        }
        perimeter5[5] = new Vector2Int(5, 5);

        for (int i = 0; i < 6; i++)
        {
            perimeter6[i]      = new Vector2Int(6, i);
            perimeter6[2*6 - i] = new Vector2Int(i, 6);
        }
        perimeter6[6] = new Vector2Int(6, 6);

        for (int i = 0; i < 7; i++)
        {
            perimeter7[i] = new Vector2Int(7, i);
            perimeter7[2*7 - i] = new Vector2Int(i, 7);
        }
        perimeter7[7] = new Vector2Int(7, 7);

		print("======== perimeter1.Length = " + perimeter1.Length);
		print("======== perimeter2.Length = " + perimeter2.Length);
		print("======== perimeter3.Length = " + perimeter3.Length);
		print("======== perimeter4.Length = " + perimeter4.Length);
		print("======== perimeter5.Length = " + perimeter5.Length);
		print("======== perimeter6.Length = " + perimeter6.Length);
		print("======== perimeter7.Length = " + perimeter7.Length);
	}

	void RookPlaneAdvancementSquare()
    {
		print("RookPlaneAdvancementSquare() - board constructed " + boardConstructed);

        if (boardConstructed == false)
        {
            return;
        }

		if (frameDelay == 0)
        {
            frameDelay = 20;
            if (perimeter == 0)
            {
                theMesh = squares[perimeter0[0][0], perimeter0[0][1], 2].GetComponent<MeshRenderer>();
                theMat = theMesh.materials;
                theMat[0].SetColor("_Color", Color.red);
            }
            else if (perimeter == 1)
            {
                for (int i = 0; i < perimeter1.Length; i++)
                {
					print("Perimeter 1");
                    theMesh = squares[perimeter1[i][0], perimeter1[i][1], 2].GetComponent<MeshRenderer>();
                    theMesh.materials[0].SetColor("_Color", Color.red);
                }
            }
            else if (perimeter == 2)
            {
                for (int i = 0; i < perimeter2.Length; i++)
                {
                    theMesh = squares[perimeter2[i][0], perimeter2[i][1], 2].GetComponent<MeshRenderer>();
                    theMesh.materials[0].SetColor("_Color", Color.red);
                }
            }
            else if (perimeter == 3)
            {
                for (int i = 0; i < perimeter3.Length; i++)
                {
                    theMesh = squares[perimeter3[i][0], perimeter3[i][1], 2].GetComponent<MeshRenderer>();
                    theMesh.materials[0].SetColor("_Color", Color.red);
                }
            }
            else if (perimeter == 4)
            {
                for (int i = 0; i < perimeter4.Length; i++)
                {
                    theMesh = squares[perimeter4[i][0], perimeter4[i][1], 2].GetComponent<MeshRenderer>();
                    theMesh.materials[0].SetColor("_Color", Color.red);
                }
            }
            else if (perimeter == 5)
            {
                for (int i = 0; i < perimeter5.Length; i++)
                {
                    theMesh = squares[perimeter5[i][0], perimeter5[i][1], 2].GetComponent<MeshRenderer>();
                    theMesh.materials[0].SetColor("_Color", Color.red);
                }
            }
            else if (perimeter == 6)
            {
                for (int i = 0; i < perimeter6.Length; i++)
                {
                    theMesh = squares[perimeter6[i][0], perimeter6[i][1], 2].GetComponent<MeshRenderer>();
                    theMesh.materials[0].SetColor("_Color", Color.red);
                }
            }
            //else if (perimeter == 7)
            //{
            //    for (int i = 0; i < perimeter7.Length; i++)
            //    {
            //        theMesh = squares[perimeter7[i][0], perimeter7[i][1], 2].GetComponent<MeshRenderer>();
            //        theMesh.materials[0].SetColor("_Color", Color.red);
            //    }
            //}
            perimeter++;
        }
        else
        {
            frameDelay--;
        }
    }
}
