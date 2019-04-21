using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr_GameManager : MonoBehaviour
{
    public List<GameObject> keySpawns; 
    // Start is called before the first frame update
    void Awake()
    {
        keySpawns = GameObject.FindGameObjectsWithTag("KeySpawnPoint").ToList();
        Debug.Log("Game manager: key spawns count: " + keySpawns.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
