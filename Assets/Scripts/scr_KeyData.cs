﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class scr_KeyData : MonoBehaviour
{
    public bool debug = false;
    public String symbol;
    public Color color;
    public Transform spawnLocation;
    public GameObject keyPickUpPrefab;

    private void Start()
    {
//        transform = ((Component) this).transform.parent.gameObject;
        
//        initializeKeyInWorld();
    }

    public void initializeKeyInWorld(List<GameObject> allSpawns, bool randomPlacement)
    {
        scr_GameManager GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<scr_GameManager>();

        if (randomPlacement){
            
            List<GameObject> validSpawns = new List<GameObject>();

            foreach (GameObject spawn in allSpawns)
                if (!spawn.transform.IsChildOf(((Component) this).transform.root))
                    validSpawns.Add(spawn);

            if (validSpawns.Count > 0)
            {
                GameObject keySpawnLocation = validSpawns[Random.Range(0, validSpawns.Count)];
                spawn(keySpawnLocation.transform, false);
                allSpawns.Remove(keySpawnLocation);
            }
        }

        else spawn(spawnLocation, false);
    }

    public GameObject spawn(Transform spawnLocation, bool receptacleStatic)
    {
        GameObject key = Instantiate(keyPickUpPrefab, spawnLocation.transform.position,
            spawnLocation.transform.rotation, spawnLocation.transform);
        
        key.GetComponent<scr_PortalKeyPickUp>().setupKey(this);

        if (receptacleStatic)
        {
            key.GetComponent<Collider>().isTrigger = false;
            key.GetComponent<AutoMoveAndRotate>().enabled = false;
        }

        return key;
    }

    public override string ToString()
    {
        return "Key to: " + transform;
    }
}
