using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class scr_PortGate : MonoBehaviour
{
    public bool active;
    public bool masterGate;
    public Transform destination;
    
    private Transform spawn;
    private OffMeshLink navLink;
    private GameObject destinationGateObject;
    private scr_Portal_AudioControl audioController;
    private Camera camera;
    private RenderTexture gateOutlook;
    private Texture inactiveTexture;

    private void Awake()
    {
        inactiveTexture = GetComponent<Renderer>().material.mainTexture;
        spawn = transform.GetChild(0); //this gates spawn pt (used for calculating rotation)
        camera = transform.GetChild(1).GetComponent<Camera>(); //this gates camera (?)
        gateOutlook = new RenderTexture(512, 512, 16);
        navLink = GetComponentInChildren<OffMeshLink>();
        audioController = GetComponent<scr_Portal_AudioControl>();
    }

    void Start()
    {
        active = false;
        navLink.activated = false;
        navLink.startTransform = navLink.transform;
        camera.targetTexture = gateOutlook;
        camera.enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        if (active)
        {
            if (collider.CompareTag("Player"))
            {
                Transform pcContainer = collider.transform.GetChild(0);
            
                collider.transform.position = new Vector3(destination.position.x, collider.transform.position.y, destination.position.z);

                float angleBetweenPortals = spawn.rotation.eulerAngles.y - destination.rotation.eulerAngles.y;
                float angleToRotate = (angleBetweenPortals < 1 && angleBetweenPortals > -1) ? 180 :
                    (Math.Abs(angleBetweenPortals) > 179 && Math.Abs(angleBetweenPortals) < 181) ? 0 : angleBetweenPortals; 
                pcContainer.Rotate(0, angleToRotate, 0);
            }
            else if (collider.CompareTag("Minotaur"))
            {
                Vector3 minoDestination = new Vector3(destination.position.x, collider.transform.position.y,
                    destination.position.z);
                NavMesh.SamplePosition(minoDestination, out NavMeshHit hitpos, 2, NavMesh.AllAreas);

                collider.transform.GetComponent<scr_AI_Enemy>().teleport(hitpos.position, destination.rotation);
            }

            audioController.teleportSFX();
            destinationGateObject.GetComponent<scr_Portal_AudioControl>().teleportSFX();
        }
        else if (collider.CompareTag("Minotaur"))
            collider.transform.GetComponent<scr_AI_Enemy>().CurrentState = scr_AI_Enemy.ENEMY_STATE.PATROL;
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
        scr_PortGate destGateScript = keyData.transform.GetComponent<scr_PortGate>();
        
        //Change this gate's stuff
        setDestination(destGateScript); 
        
        //Change destination gate stuff
        destGateScript.setDestination(this);
        
        destinationGateObject.GetComponentInChildren<Camera>().enabled = true; //Enable camera on destination gate when activated
    }

    public void deactivatePortal(scr_KeyData keyData)
    {
        scr_PortGate destGateScript = destinationGateObject.GetComponent<scr_PortGate>();
        
        //Change this gate's stuff
        unsetDestination(destGateScript); 
        
        //Change destination gate stuff
        destGateScript.unsetDestination(this);

        destinationGateObject = null;
    }

    public void setDestination(scr_PortGate gateScript)
    {
        destination = gateScript.spawn; //set this gates destination transform
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = gateScript.gateOutlook;//set this gates texture
        material.SetTexture("_BumpMap", null);
        navLink.endTransform = gateScript.navLink.transform;
        navLink.activated = true;
        active = true;
        
        audioController.idleSFX(active);
        
        destinationGateObject = gateScript.transform.gameObject; //store dest gate for visible/invisible camera switching
    }
    
    public void unsetDestination(scr_PortGate gateScript)
    {
        destination = spawn;
        Material material = GetComponent<Renderer>().material;
        material.mainTexture = inactiveTexture;//set this gates texture
        navLink.activated = false;
        active = false;
        
        audioController.idleSFX(active);

        destinationGateObject = null; //set dest gate for visible/invisible camera switching
        camera.enabled = false; //just for safety in case there are two gates visible at once when disabling
    }

    public override string ToString()
    {
        return gameObject + " : " + transform.parent.parent;
    }
}
