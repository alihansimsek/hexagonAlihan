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
    GameObject[] selectedHexagons = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        CameraController.OnSwipe += SwipeDetector_OnSwipe;
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
                tmp.a = 1f;     //setting the alpha of the color to opaque. it comes as default 0 from the array.
                hexagon.GetComponent<SpriteRenderer>().color = tmp;
                GameObject hexa = Instantiate(hexagon);

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

    private void SwipeDetector_OnSwipe(CameraController.SwipeData data) //Swiping event triggered by CameraController.
    {
        Debug.Log("Swipe in Direction: " + data.Direction);
        TurnSelectedHexagons(data);
        //System.Threading.Thread.Sleep(1000);
        GetComponentInParent<GameManager>().playerSwiped();//letting GameManager know that player have made their move so it will initiate Turn event which will make hexagons check for matches.
        //GameObject.Find("Main Camera").GetComponent<CameraController>().setAllowedToSwipe(false);   //STOP PLAYER SWIPING UNTIL HEXA CHECKS AND DESTROYING ENDS.
            
    }

    internal static void respawnDestroyed(int gridX, int gridY)
    {
       // throw new NotImplementedException();
    }

    private void TurnSelectedHexagons(CameraController.SwipeData data) //Moves selected hexagons between each other and rearrenges hexagonsGrid[,] array.
    {
            Debug.Log("TURN HEXA");
            if (selectedHexagons[0] != null)
                Debug.Log(selectedHexagons[0]);
            {
                Vector3 temp;
                temp = selectedHexagons[0].transform.position;
                GameObject tempObj = selectedHexagons[0];

                if (data.Direction == CameraController.SwipeDirection.Left)
                {
                    switchGridPos(selectedHexagons[0], selectedHexagons[1], selectedHexagons[2]);
                    selectedHexagons[0].transform.position = selectedHexagons[1].transform.position;
                    selectedHexagons[1].transform.position = selectedHexagons[2].transform.position;
                    selectedHexagons[2].transform.position = temp;

                    selectedHexagons[0] = selectedHexagons[1];
                    selectedHexagons[1] = selectedHexagons[2];
                    selectedHexagons[2] = tempObj;
                }
                if (data.Direction == CameraController.SwipeDirection.Right)
                {
                    switchGridPos(selectedHexagons[2], selectedHexagons[1], selectedHexagons[0]);
                    selectedHexagons[0].transform.position = selectedHexagons[2].transform.position;
                    selectedHexagons[2].transform.position = selectedHexagons[1].transform.position;
                    selectedHexagons[1].transform.position = temp;

                    selectedHexagons[0] = selectedHexagons[2];
                    selectedHexagons[2] = selectedHexagons[1];
                    selectedHexagons[1] = tempObj;
                }
            }
            //System.Threading.Thread.Sleep(1000);
        
    }

    private void switchGridPos(GameObject third, GameObject second, GameObject first)   //switches objects' position on hexagonsGrid[,] on given order and updates HexagonManager values.
    {
        Debug.Log(third + " " + second + " " + first);
        int firstX, firstY, secondX, secondY, thirdX, thirdY;
        firstX = first.GetComponent<HexagonManager>().getGridPosX();
        firstY = first.GetComponent<HexagonManager>().getGridPosY();
        secondX = second.GetComponent<HexagonManager>().getGridPosX();
        secondY = second.GetComponent<HexagonManager>().getGridPosY();
        thirdX = third.GetComponent<HexagonManager>().getGridPosX();
        thirdY = third.GetComponent<HexagonManager>().getGridPosY();

        hexagonsGrid[firstX, firstY] = second;
        second.GetComponent<HexagonManager>().setGridPosX(firstX);
        second.GetComponent<HexagonManager>().setGridPosY(firstY);

        hexagonsGrid[secondX, secondY] = third;
        third.GetComponent<HexagonManager>().setGridPosX(secondX);
        third.GetComponent<HexagonManager>().setGridPosY(secondY);

        hexagonsGrid[thirdX, thirdY] = first;
        first.GetComponent<HexagonManager>().setGridPosX(thirdX);
        first.GetComponent<HexagonManager>().setGridPosY(thirdY);
    }

    

    public void highlightNeigbors(int gridPosX, int gridPosY)     //Finds and highlights hexagons selected by CameraController.
    {
        
        Debug.Log("COORDINATES:"+" "+gridPosX+", "+gridPosY);
        selectedHexagons[0] = hexagonsGrid[gridPosX, gridPosY];
        if(gridPosY == 0 || gridPosY % 2 == 0){     //If on an even row, highlight the tile directly underneath.
            
            if((gridPosY + 1) < gridY){   // out of array check for corners

                hexagonsGrid[gridPosX, gridPosY + 1].GetComponent<HexagonManager>().highlight();
                selectedHexagons[1] = hexagonsGrid[gridPosX, gridPosY + 1];
            }
            else{
                
                hexagonsGrid[gridPosX, gridPosY - 1].GetComponent<HexagonManager>().highlight();    //if it is out of bounds, highlight the tile above
                selectedHexagons[1] = hexagonsGrid[gridPosX, gridPosY - 1];
            }
            
            
        }
        else {  //If on an odd row, highlight the tile at x+1,y+1 so every tile choices will be adjecent.
            
            if((gridPosY + 1) < gridY && (gridPosX + 1) < gridX){  // out of array check for corners
            hexagonsGrid[gridPosX + 1, gridPosY + 1].GetComponent<HexagonManager>().highlight();
            selectedHexagons[1] = hexagonsGrid[gridPosX + 1, gridPosY + 1];
            }
            else{
                if((gridPosX + 1) < gridX){   // out of array check for corners
                    hexagonsGrid[gridPosX + 1, gridPosY - 1].GetComponent<HexagonManager>().highlight();
                    selectedHexagons[1] = hexagonsGrid[gridPosX + 1, gridPosY - 1];
                }
                else{
                    hexagonsGrid[gridPosX, gridPosY - 1].GetComponent<HexagonManager>().highlight();    //if it is out of bounds, highlight the adjecent tile above
                    selectedHexagons[1] = hexagonsGrid[gridPosX, gridPosY - 1];
                }
            }
        
        }

        if((gridPosX + 1) < gridX){   // out of array check for corners
        hexagonsGrid[gridPosX + 1, gridPosY].GetComponent<HexagonManager>().highlight();
        selectedHexagons[2] = hexagonsGrid[gridPosX + 1, gridPosY];
        }
        else{
            if(gridPosY == 0 || gridPosY % 2 == 0){                                                     //if even row, choose adjecent tile
            hexagonsGrid[gridPosX - 1, gridPosY + 1].GetComponent<HexagonManager>().highlight();
            selectedHexagons[2] = hexagonsGrid[gridPosX - 1, gridPosY + 1];
            }
            else {
            hexagonsGrid[gridPosX - 1, gridPosY].GetComponent<HexagonManager>().highlight();
            selectedHexagons[2] = hexagonsGrid[gridPosX - 1, gridPosY];
            }
        }
    }

    private void setBoard(int row, int col, float posX, float posY, Vector2[,] hexagonPositions, GameObject hexa)
    {
        hexa.transform.position = new Vector2(posX, posY);
        hexagonPositions[row, col] = new Vector2(posX, posY);
        hexagonsGrid[row, col] = hexa;
        hexa.GetComponent<HexagonManager>().setGridPosX(row);
        hexa.GetComponent<HexagonManager>().setGridPosY(col);
    }

    private void setHexagonsGrid(int gridX, int gridY)
    {
        this.hexagonsGrid = new GameObject[gridX, gridY];
    }
    public GameObject[,] getHexagonsGrid()
    {
        return this.hexagonsGrid;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
