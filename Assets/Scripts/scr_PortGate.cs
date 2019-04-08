using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class scr_PortGate : MonoBehaviour
{
    public GameObject destinationGate;
    public RenderTexture renderTexture;
    private Transform destination;
    private Transform spawn;
    private Camera camera;
    private RenderTexture viewTexture;

    // Start is called before the first frame update
    void Start()
    {
        destination =  destinationGate.transform.GetChild(0);
        spawn = transform.GetChild(0);
        camera = transform.GetChild(1).GetComponent<Camera>();
        viewTexture = new RenderTexture(renderTexture);
        camera.targetTexture = viewTexture;
        destinationGate.GetComponent<Renderer>().material.mainTexture = viewTexture;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        if (destinationGate != null && collider.CompareTag("Player"))
        {
            Debug.Log(collider.transform.localPosition);
            collider.transform.position = new Vector3(destination.position.x, collider.transform.position.y, destination.position.z);
            collider.GetComponent<FirstPersonAIO>().rotate(spawn.rotation.eulerAngles.y - destination.rotation.eulerAngles.y);
        }
    }
}
