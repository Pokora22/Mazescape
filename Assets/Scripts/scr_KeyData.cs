﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_KeyData : MonoBehaviour
{
    public bool debug = false;
    public GameObject keyPickUpPrefab, keyStaticPrefab, destinationGateObject;
    public Material keyMaterial; //change the new key's material (not on prefab - on separate instance) //TODO: how?
    
    // Start is called before the first frame update
    void Start()
    {
        scr_GameManager GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<scr_GameManager>();
        destinationGateObject = transform.parent.gameObject;
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
                keyPickup.GetComponent<scr_PortalKeyPickUp>().setData(this); //set link back to this data
                allSpawns.Remove(keySpawnLocation);
            }
        }

        else
        {
            Transform keySpawnLocation = transform.parent.GetComponent<scr_PortGate>().keyLocation;
            GameObject keyPickup = Instantiate(keyPickUpPrefab, keySpawnLocation.transform.position,
                keySpawnLocation.transform.rotation, keySpawnLocation.transform);
            keyPickup.GetComponent<scr_PortalKeyPickUp>().setData(this); //set link back to this data
        }
    }

    public override string ToString()
    {
        return "Key to: " + destinationGateObject;
    }
}
