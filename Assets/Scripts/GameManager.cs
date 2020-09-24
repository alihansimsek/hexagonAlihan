using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void playerTurn();
    public static event playerTurn playerPlayed;

    internal static void selectHexagon(GameObject gameObject)
    {
        
    }

    //TURN MANTIGI: OYUNCU SEÇTİĞİ 3 HEXAGONU ÇEVİRDİĞİNDE GAME MANAGER TURN EVENTİNİ İNVOKE EDECEK. EĞER HİÇBİR HEXAGONDAN
    //MATCH GELMEZSE OYUNCUNUN HAREKETİNİ GERİ ALACAK. MATCH VARSA HANGİ HEXAGONDAN GELDİYSE ONU PATLATACAK
    //MATCHDEN SONRA YENİ HEXAGON SPAWN EDECEK VE BİR MİNİ-TURN İNVOKE EDECEK Kİ HEXAGONLAR KONTROLLERİNİ YAPSIN
    //MATCH GELİRSE BU LOOP DEVAM EDECEK, GELMEZSE OYUNCU İNPUTUNA İZİN VERİLECEK. MİNİ-TURN BOMBA GERİSAYIMINI ETKİLEMEYECEK.

    // Start is called before the first frame update
    void Start()
    {
        if (playerPlayed != null)
            playerPlayed();
    }

    internal static void Blast(GameObject gameObject)   //destroy the incoming hexagon object on the grid and on the scene
    {
        //throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
