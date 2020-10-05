using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void playerTurn();
    public static event playerTurn playerPlayed;
    private static ArrayList destroyedList = new ArrayList();
    private static int hexaCount = 0;
    internal static void selectHexagon(GameObject gameObject)
    {

    }

    //TURN MANTIGI: OYUNCU SEÇTİĞİ 3 HEXAGONU ÇEVİRDİĞİNDE GAME MANAGER TURN EVENTİNİ İNVOKE EDECEK.
    //MATCH VARSA HANGİ HEXAGONDAN GELDİYSE ONU PATLATACAK
    //MATCHDEN SONRA YENİ HEXAGON SPAWN EDECEK VE BİR MİNİ-TURN İNVOKE EDECEK Kİ HEXAGONLAR KONTROLLERİNİ YAPSIN
    //MATCH GELİRSE BU LOOP DEVAM EDECEK, GELMEZSE OYUNCU İNPUTUNA İZİN VERİLECEK. MİNİ-TURN BOMBA GERİSAYIMINI ETKİLEMEYECEK.

    // Start is called before the first frame update
    void Start()
    {

    }

    internal static void TurnCheck(int gridX, int gridY, int totalHexa, Boolean isDestroy, GameObject hexa)   //destroy the incoming hexagon object on the grid and on the scene
    {
        hexaCount++;
        if (isDestroy)
        {
            Destroy(hexa);
            int[] coordinates = new int[4];
            coordinates[0] = gridX;
            coordinates[1] = gridY;
           /* coordinates[2] = hexa.transform.position.x;
            coordinates[3] = hexa.transform.position.y;*/

            destroyedList.AddRange(coordinates);
        }
        if(hexaCount >= totalHexa)
        {
            spawnHexa(destroyedList);
            hexaCount = 0;
            destroyedList = new ArrayList();
            clearHighlights();
        }
    }

    private static void spawnHexa(ArrayList destroyedList)
    {
        for (int i = 0; i < destroyedList.Count; i = i+4)
        {
            GridManager.respawnDestroyed((int)destroyedList[i], (int)destroyedList[i+1]);
        }
        GameObject.Find("Main Camera").GetComponent<CameraController>().setAllowedToSwipe(true);   //ALLOW PLAYER SWIPING

    }

    private static void clearHighlights()
    {
        GameObject[] hl = GameObject.FindGameObjectsWithTag("Highlight");
        for (int i = 0; i < hl.Length; i++)
        {
            GameObject.Destroy(hl[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playerSwiped()
    {
        Debug.Log("PLAYER SWIPED");
        if (playerPlayed != null)
            playerPlayed();
    }

}
