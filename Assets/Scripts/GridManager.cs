using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridX = 8;
    public int gridY = 9;
    public GameObject hexagon;
    public Color[] colors = new Color[8];
    GameObject[,] hexagonsGrid;
    // Start is called before the first frame update
    void Start()
    {
        Vector2[,] hexagonPositions = new Vector2[gridX,gridY];
        setHexagonsGrid(gridX, gridY);
        
        float posY = 0;
        float posX = 0;
        int row = 0;
        int col = 0;
        for(row = 0; row < gridX; row++)
        {
            
            for(col = 0; col < gridY; col++)
            {
                Color tmp = colors[UnityEngine.Random.Range(0, colors.Length)];
                tmp.a = 1f;     //setting the alpha of the color to opaque. it comes default 0 from the array.
                hexagon.GetComponent<SpriteRenderer>().color = tmp;
                GameObject hexa = Instantiate(hexagon); //HEXAGON OBJESİNE PARAMETRE SET EDILECEK (ROW VE COL). HEXAGON PATLADIĞINDA BU DEĞERLER İLE GRİD'E HABER VERECEK. GRID DE ONA GÖRE ARRAYDE O OBJEYİ PATLATIP YENİ OLUŞTURACAK.

                posX = row;

                if (col != 0)
                {
                    posY = -col * 0.80f;    //squeezes the tiles together. except the first row.
                }
                else
                {
                    posY = -col;    //standard positioning for first row
                }
                
                if (col != 0 && col % 2 != 0)       //each even row
                {
                    posX += 0.5f;
                    
                }
                setBoard(row,col,posX,posY,hexagonPositions,hexa);
                
            }
        }
    }

    private void setHexagonsGrid(int gridX, int gridY)
    {
        this.hexagonsGrid = new GameObject[gridX, gridY];
    }
    private GameObject[,] getHexagonsGrid()
    {
        return this.hexagonsGrid;
    }

    public void highlightNeigbors(int gridPosX, int gridPosY)     //Finds and highlights hexagons selected by CameraController. Needs more controls for tiles at the corners.
    {
        hexagonsGrid[gridPosX + 1, gridPosY].GetComponent<HexagonManager>().highlight();
        Debug.Log("called highlight of: " + hexagonsGrid[gridPosX + 1, gridPosY]);
        hexagonsGrid[gridPosX, gridPosY + 1].GetComponent<HexagonManager>().highlight();
        Debug.Log("called highlight of: " + hexagonsGrid[gridPosX, gridPosY+1]);
    }

    private void setBoard(int row, int col, float posX, float posY, Vector2[,] hexagonPositions, GameObject hexa)
    {
        hexa.transform.position = new Vector2(posX, posY);
        hexagonPositions[row, col] = new Vector2(posX, posY);
        hexagonsGrid[row, col] = hexa;
        hexa.GetComponent<HexagonManager>().setGridPosX(row);
        hexa.GetComponent<HexagonManager>().setGridPosY(col);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
