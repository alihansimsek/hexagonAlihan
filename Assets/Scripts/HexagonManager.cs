using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexagonManager : MonoBehaviour
{
    private static int HEXAGON_SIDES = 6;
    private int gridPosX, gridPosY; //will be set when the hexagon placed in a grid array by GridManager
    private GameObject[] neighbors = new GameObject[HEXAGON_SIDES];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

    }
    

    private void OnEnable()     //obje yaratıldığında event'e subscribe oluyor
    {
        GameManager.playerPlayed += Turn;
    }

    private void OnDisable()    //obje yok edildiğinde event'e unsubscribe oluyor
    {
        GameManager.playerPlayed -= Turn;
    }
    void Turn() //listener event will be invoked by GameManager after each move of the player. this method will fill neighbors array and check if there is a match and call back gameManager if there is a match.
    {
        Debug.Log("Turn initiated. Hexagon: " + this.gameObject + " " + gridPosX + " " + gridPosY);
        GameObject[,] hexaGrid = GameObject.Find("GameManager/Grid").GetComponent<GridManager>().getHexagonsGrid();
        Boolean isMatch = false;
        Boolean isEvenRow = false;
        if (gridPosY == 0 || gridPosY % 2 == 0)
        {
            isEvenRow = true;
        }

        if (isEvenRow) {  //spagetti code block. couldnt figure out a formula to fill neighbors in a loop
            if (gridPosX + 1 < hexaGrid.GetLength(0)) neighbors[0] = hexaGrid[gridPosX + 1, gridPosY];
            if (gridPosY + 1 < hexaGrid.GetLength(1)) neighbors[1] = hexaGrid[gridPosX, gridPosY + 1];
            if (gridPosX - 1 >= 0 && gridPosY + 1 < hexaGrid.GetLength(1)) neighbors[2] = hexaGrid[gridPosX - 1, gridPosY + 1];
            if (gridPosX - 1 >= 0) neighbors[3] = hexaGrid[gridPosX - 1, gridPosY];
            if (gridPosX - 1 >= 0 && gridPosY - 1 >= 0) neighbors[4] = hexaGrid[gridPosX - 1, gridPosY - 1];
            if (gridPosY - 1 >= 0) neighbors[5] = hexaGrid[gridPosX, gridPosY - 1]; 
        }

        else             // neighbors' x coordinate values change on odd rows, because of the grid's unusual nature
        {
            if (gridPosX + 1 < hexaGrid.GetLength(0)) neighbors[0] = hexaGrid[gridPosX + 1, gridPosY];
            if (gridPosX + 1 < hexaGrid.GetLength(0) && gridPosY + 1 < hexaGrid.GetLength(1)) neighbors[1] = hexaGrid[gridPosX + 1, gridPosY + 1];
            if (gridPosY + 1 < hexaGrid.GetLength(1)) neighbors[2] = hexaGrid[gridPosX, gridPosY + 1];
            if (gridPosX - 1 >= 0) neighbors[3] = hexaGrid[gridPosX - 1, gridPosY];
            if (gridPosY - 1 >= 0) neighbors[4] = hexaGrid[gridPosX, gridPosY - 1];
            if (gridPosY - 1 >= 0 && gridPosX + 1 < hexaGrid.GetLength(0)) neighbors[5] = hexaGrid[gridPosX + 1, gridPosY - 1];
        }

        Color hexaColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < neighbors.Length; i++)
        {
           if(neighbors[i] != null)
            {
                Color neighborColor = neighbors[i].GetComponent<SpriteRenderer>().color;
                if(neighborColor == hexaColor)                                              //If it hits two matches back to back, hexagon decides to destroy itself.
                {
                    if (isMatch)
                    {
                        GameManager.TurnCheck(gridPosX, gridPosY, hexaGrid.Length, true, this.gameObject);
                        break;
                    }
                    isMatch = true;
                }
                else
                {
                    isMatch = false;
                }
            }
        }
        GameManager.TurnCheck(gridPosX, gridPosY, hexaGrid.Length, false, this.gameObject);
    }

    public void selectHexa()
    {
        Debug.Log("SELECT HEXA");
        clearHighlights();
        highlight();
        GameObject gm = GameObject.Find("GameManager/Grid");
        gm.GetComponent<GridManager>().highlightNeigbors(gridPosX, gridPosY);
    }

    private void clearHighlights()
    {
        GameObject[] hl = GameObject.FindGameObjectsWithTag("Highlight");
        for(int i = 0; i < hl.Length; i++)
        {
            GameObject.Destroy(hl[i]);
        }
        
    }

    public void setGridPosX(int x) {
        gridPosX = x;
    }
    public void setGridPosY(int y) {
        gridPosY = y;
    }

    public int getGridPosX()
    {
        return gridPosX;
    }
    public int getGridPosY()
    {
        return gridPosY;
    }

    public void highlight()
    {
        GameObject highlight = new GameObject();
        highlight.tag = "Highlight";
        highlight.AddComponent<SpriteRenderer>();
        highlight.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        highlight.GetComponent<Transform>().position = this.transform.position + new Vector3(0, 0, 5); ;
        highlight.GetComponent<Transform>().localScale = this.transform.localScale + new Vector3(0.1f, 0.1f, 0);
    }
}
