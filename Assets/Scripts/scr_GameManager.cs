using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr_GameManager : MonoBehaviour
{
    public List<GameObject> keySpawns;
    public bool randomKeyPlacement;

    private GameObject[] redPortals, bluePortals, greenPortals, whitePortals;
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
        
        keySpawns = GameObject.FindGameObjectsWithTag("KeySpawnPoint").ToList();
    }

    private void Start()
    {
        for (int i = 0; i < maxPortals; i++)
        {
            if(i < redPortals.Length)
                redPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns);
            if(i < bluePortals.Length)
                bluePortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns);
            if(i < greenPortals.Length)
                greenPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns);
            if(i < whitePortals.Length)
                whitePortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
