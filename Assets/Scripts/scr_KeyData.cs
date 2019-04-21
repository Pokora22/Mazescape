using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_KeyData : MonoBehaviour
{
    public GameObject keyPickUpPrefab, keyStaticPrefab, destinationGateObject;
    public Material keyMaterial; //change the new key's material (not on prefab - on separate instance) //TODO: how?
    
    // Start is called before the first frame update
    void Start()
    {
        destinationGateObject = transform.parent.gameObject;
        Debug.Log("On start: " + destinationGateObject);
        List<GameObject> allSpawns = GameObject.FindGameObjectWithTag("GameManager").GetComponent<scr_GameManager>().keySpawns;
        List<GameObject> validSpawns = new List<GameObject>();
        
        Debug.Log("All spawns count: " + allSpawns.Count);
        Debug.Log("Valid spawns count: " + validSpawns.Count);
        
        foreach (GameObject spawn in allSpawns)
            if (!spawn.transform.parent.CompareTag(transform.parent.tag))
                validSpawns.Add(spawn);
        
        if (validSpawns.Count > 0)
        {
            GameObject keySpawnLocation = validSpawns[Random.Range(0, validSpawns.Count)]; 
            GameObject keyPickup = Instantiate(keyPickUpPrefab, keySpawnLocation.transform, true);
            keyPickup.GetComponent<scr_PortalKeyPickUp>().setData(this); //set link back to this data
            allSpawns.Remove(keySpawnLocation);
        }
    }

    public override string ToString()
    {
        return "Key to: " + destinationGateObject;
    }
}
