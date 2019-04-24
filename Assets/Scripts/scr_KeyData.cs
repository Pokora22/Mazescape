using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class scr_KeyData : MonoBehaviour
{
    public bool debug = false;
    public String symbol;
    public Color color;
    public Transform spawnLocation;
    public GameObject keyPickUpPrefab, destinationGateObject;

    private void Start()
    {
        destinationGateObject = transform.parent.gameObject;
        
        initializeKeyInWorld();
    }

    private void initializeKeyInWorld()
    {
        scr_GameManager GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<scr_GameManager>();
        
        if(debug) Debug.Log("On start: " + destinationGateObject);

        if (GameManager.randomKeyPlacement)
        {
            List<GameObject> allSpawns = GameManager.keySpawns;
            List<GameObject> validSpawns = new List<GameObject>();

            if (debug) Debug.Log("All spawns count: " + allSpawns.Count);
            if (debug) Debug.Log("Valid spawns count: " + validSpawns.Count);

            foreach (GameObject spawn in allSpawns)
                if (!spawn.transform.parent.CompareTag(transform.parent.tag))
                    validSpawns.Add(spawn);

            if (validSpawns.Count > 0)
            {
                GameObject keySpawnLocation = validSpawns[Random.Range(0, validSpawns.Count)];
                GameObject keyPickup = Instantiate(keyPickUpPrefab, keySpawnLocation.transform.position,
                    keySpawnLocation.transform.rotation, keySpawnLocation.transform);
                keyPickup.GetComponent<scr_PortalKeyPickUp>().setSymbolAndColor(this); //set link back to this data
                allSpawns.Remove(keySpawnLocation);
            }
        }

        else
        {
            keyPickUpPrefab.GetComponent<scr_PortalKeyPickUp>().setSymbolAndColor(this);
            GameObject keyPickup = Instantiate(keyPickUpPrefab, spawnLocation.transform.position,
                spawnLocation.transform.rotation, spawnLocation.transform);
//            keyPickup.GetComponent<scr_PortalKeyPickUp>().setSymbolAndColor(this); //set link back to this data
        }
    }

    public override string ToString()
    {
        return "Key to: " + destinationGateObject;
    }
}
