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
    private Texture inactiveTexture;

    void Start()
    {
        inactiveTexture = GetComponent<Renderer>().material.mainTexture;
        active = false;
        spawn = transform.GetChild(0); //this gates spawn pt (used for calculating rotation)
        camera = transform.GetChild(1).GetComponent<Camera>(); //this gates camera (?)
        gateOutlook = new RenderTexture(512, 512, 16);
        camera.targetTexture = gateOutlook;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        if (active && collider.CompareTag("Player"))
        {
            Debug.Log("Teleporting to: " + destination);
            Transform pcContainer = collider.transform.GetChild(0);
            collider.transform.position = new Vector3(destination.position.x, collider.transform.position.y, destination.position.z);
            pcContainer.Rotate(0, (spawn.rotation.eulerAngles.y - destination.rotation.eulerAngles.y), 0); //TODO: Test for same rotation - might not rotate at all
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

    public void deactivatePortal(scr_KeyData keyData)
    {
        scr_PortGate destGateScript = keyData.destinationGateObject.GetComponent<scr_PortGate>();
        
        //Change this gate's stuff
        unsetDestination(destGateScript); 
        
        //Change destination gate stuff
        destGateScript.unsetDestination(this);
    }

    public void setDestination(scr_PortGate gateScript)
    {
        destination = gateScript.spawn; //set this gates destination transform
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = gateScript.gateOutlook;//set this gates texture
        material.SetTexture("_BumpMap", null);
        active = true;
    }
    
    public void unsetDestination(scr_PortGate gateScript)
    {
        destination = spawn;
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = inactiveTexture;//set this gates texture
        active = false;
    }
}
