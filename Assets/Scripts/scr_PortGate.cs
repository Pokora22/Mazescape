using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class scr_PortGate : MonoBehaviour
{
    public bool active;

    private scr_KeyData keyData;
    private Transform destination;
    private Transform spawn;
    private Camera camera;
    private RenderTexture gateOutlook;

    void Start()
    {
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
        Material material = GetComponent<Renderer>().material; 
        material.mainTexture = gateScript.gateOutlook; //set this gates texture
        material.mainTextureOffset.Set(1, 0); //flip the texture along x axis (mirrored otherwise)
        material.mainTextureScale.Set(-1, 1);
        active = true;
    }
}
