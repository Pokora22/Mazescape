using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr_GameManager : MonoBehaviour
{
    public List<GameObject> keySpawns;
    public bool randomKeyPlacement;

    private GameObject[] redPortals, bluePortals, greenPortals, whitePortals, masterPortals;
    private int maxPortals = 0;
    
    void Awake()
    {
        redPortals = GameObject.FindGameObjectsWithTag("RedPortal");
        if (redPortals.Length > maxPortals) maxPortals = redPortals.Length;
        bluePortals = GameObject.FindGameObjectsWithTag("BluePortal");
        if (bluePortals.Length > maxPortals) maxPortals = bluePortals.Length;
        greenPortals = GameObject.FindGameObjectsWithTag("GreenPortal");
        if (greenPortals.Length > maxPortals) maxPortals = greenPortals.Length;
        whitePortals = GameObject.FindGameObjectsWithTag("WhitePortal");
        if (whitePortals.Length > maxPortals) maxPortals = whitePortals.Length;
        masterPortals = GameObject.FindGameObjectsWithTag("MasterPortal");
        if (masterPortals.Length > maxPortals) maxPortals = masterPortals.Length;
        
        keySpawns = GameObject.FindGameObjectsWithTag("KeySpawnPoint").ToList();
        
        for (int i = 0; i < maxPortals; i++)
        {
            if(i < redPortals.Length)
                redPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement);
            if(i < bluePortals.Length)
                bluePortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement);
            if(i < greenPortals.Length)
                greenPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement);
            if(i < whitePortals.Length)
                whitePortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement);
            
            if(i < masterPortals.Length)
                masterPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, false);
        }
    }

    private void Start()
    {
        
    }
}
