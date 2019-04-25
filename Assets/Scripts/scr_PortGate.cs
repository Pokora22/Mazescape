using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class scr_PortGate : MonoBehaviour
{
    public bool active;
    
    private Transform destination;
    private Transform spawn;
    private GameObject destinationGateObject;
    private Camera camera;
    private RenderTexture gateOutlook;
    private Texture inactiveTexture;

    void Start()
    {
        inactiveTexture = GetComponent<Renderer>().material.mainTexture;
        active = false;
        spawn = transform.GetChild(0); //this gates spawn pt (used for calculating rotation)
        camera = transform.GetChild(1).GetComponent<Camera>(); //this gates camera (?)
        gateOutlook = new RenderTexture(512, 512, 16);
        camera.targetTexture = gateOutlook;
        camera.enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        if (active && collider.CompareTag("Player"))
        {
            Transform pcContainer = collider.transform.GetChild(0);
            
            collider.transform.position = new Vector3(destination.position.x, collider.transform.position.y, destination.position.z);

            float angleBetweenPortals = spawn.rotation.eulerAngles.y - destination.rotation.eulerAngles.y;
            float angleToRotate = angleBetweenPortals == 0 ? 180 : angleBetweenPortals % 180; 
            pcContainer.Rotate(0, angleToRotate, 0);
        }
    }


    private void OnBecameVisible()
    {
        if (active)
            destinationGateObject.GetComponentInChildren<Camera>().enabled = true;
        
    }

    private void OnBecameInvisible()
    {
        if(active)
            destinationGateObject.GetComponentInChildren<Camera>().enabled = false;
    }

    public void activatePortal(scr_KeyData keyData)
    {
        Debug.Log("Running activate...");
        scr_PortGate destGateScript = keyData.destinationGateObject.GetComponent<scr_PortGate>();
        
        //Change this gate's stuff
        setDestination(destGateScript); 
        
        //Change destination gate stuff
        destGateScript.setDestination(this);
        
        destinationGateObject.GetComponentInChildren<Camera>().enabled = true; //Enable camera on destination gate when activated
    }

    public void deactivatePortal(scr_KeyData keyData)
    {
        Debug.Log("Running deactivate...");
        scr_PortGate destGateScript = destinationGateObject.GetComponent<scr_PortGate>();
        
        //Change this gate's stuff
        unsetDestination(destGateScript); 
        
        //Change destination gate stuff
        destGateScript.unsetDestination(this);
        
//        destinationGateObject.GetComponentInChildren<Camera>().enabled = false; //Disable camera on destination gate when deactivated //TODO: If gate linked to itself ?
        destinationGateObject = null;
    }

    public void setDestination(scr_PortGate gateScript)
    {
        destination = gateScript.spawn; //set this gates destination transform
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = gateScript.gateOutlook;//set this gates texture
        material.SetTexture("_BumpMap", null);
        active = true;
        
        destinationGateObject = gateScript.transform.gameObject; //store dest gate for visible/invisible camera switching
    }
    
    public void unsetDestination(scr_PortGate gateScript)
    {
        destination = spawn;
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = inactiveTexture;//set this gates texture
        active = false;

        destinationGateObject = null; //set dest gate for visible/invisible camera switching
        camera.enabled = false; //just for safety in case there are two gates visible at once when disabling
    }

    public override string ToString()
    {
        return gameObject + " : " + transform.parent.parent;
    }
}
