using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexagonManager : MonoBehaviour
{
    private Color[] neighborColors = new Color[8];
    private int gridPosX, gridPosY; //will be set when the hexagon placed in a grid array by GridManager

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
        print("Turn initiated");
        for(int i = 0; i < neighborColors.Length; i++)
        {

        }
        GameManager.Blast(this.gameObject); //will be called when there is a match
    }

    public void selectHexa()
    {
        
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
