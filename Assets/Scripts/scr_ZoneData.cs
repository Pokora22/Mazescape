using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ZoneData : MonoBehaviour
{
    private List<GameObject> localDestinations;
    
    // Start is called before the first frame update
    void Start()
    {
//        localDestinations = new List<GameObject>();
//        
//        foreach (GameObject dest in GameObject.FindGameObjectsWithTag("Dest"))
//            if(dest.transform.IsChildOf(transform))
//                localDestinations.Add(dest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
//        if (other.CompareTag("Minotaur"))
//        {
//            other.GetComponent<scr_AI_Enemy>().setDestinations(localDestinations);
//        }
    }
}
