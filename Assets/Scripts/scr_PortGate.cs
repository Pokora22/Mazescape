using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class scr_PortGate : MonoBehaviour
{
    public bool active;
    
    public scr_KeyData keyData;
    private Transform destination;
    private Transform spawn;
    private Camera camera;
    private RenderTexture gateOutlook;

    void Start()
    {
        List<GameObject> allSpawns = GameObject.FindGameObjectWithTag("GameManager").GetComponent<scr_GameManager>().keySpawns;
        List<GameObject> validSpawns = new List<GameObject>();
        
        keyData = Instantiate(keyData);
        keyData.destinationGateObject = gameObject;
        
        Debug.Log(keyData.keyPickUpPrefab);
        Debug.Log(keyData.keyStaticPrefab);
        Debug.Log(keyData.destinationGateObject);
        
        foreach (GameObject spawn in allSpawns)
            if (!spawn.transform.parent.CompareTag(transform.tag))
                validSpawns.Add(spawn);
        
        if (validSpawns.Count > 0)
        {
            GameObject keySpawnLocation = validSpawns[Random.Range(0, validSpawns.Count)]; 
            Instantiate(keyData.keyPickUpPrefab, keySpawnLocation.transform);
            allSpawns.Remove(keySpawnLocation);
        }

        active = false;
        spawn = transform.GetChild(0); //this gates spawn pt (used for calculating rotation)
        camera = transform.GetChild(1).GetComponent<Camera>(); //this gates camera (?)
        gateOutlook = new RenderTexture(512, 512, 0);
        camera.targetTexture = gateOutlook;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(gameObject);
        GameObject collider = other.gameObject;
        if (active && collider.CompareTag("Player"))
        {
            Transform pcContainer = collider.transform.GetChild(0);
            collider.transform.position = new Vector3(destination.position.x, collider.transform.position.y, destination.position.z);
            pcContainer.Rotate(0, (spawn.rotation.eulerAngles.y - destination.rotation.eulerAngles.y)%180, 0);
        }
    }

    public void activatePortal(scr_KeyData keyData)
    {
        scr_PortGate destGateScript = keyData.destinationGateObject.GetComponent<scr_PortGate>();
        
        //Change this gate's stuff
        setDestination(destGateScript); 
        
        //Change destination gate stuff
        destGateScript.setDestination(this);
    }

    public void setDestination(scr_PortGate gateScript)
    {
        destination = gateScript.spawn; //set this gates destination transform
        GetComponent<Renderer>().material.mainTexture = gateScript.gateOutlook; //set this gates texture
        active = true;
    }
}
