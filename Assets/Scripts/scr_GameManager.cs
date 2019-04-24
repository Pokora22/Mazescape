using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class scr_GameManager : MonoBehaviour
{
    public List<GameObject> keySpawns;

    public bool randomKeyPlacement;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] findbytag = GameObject.FindGameObjectsWithTag("KeySpawnPoint");
        keySpawns = findbytag.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
