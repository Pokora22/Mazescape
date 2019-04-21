using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scr_PortalKeyRecepticle : MonoBehaviour
{
    public string putKeyText, takeKeyText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !transform.parent.GetComponent<scr_PortGate>().active)
            gameObject.GetComponentInChildren<TextMeshPro>().SetText(putKeyText);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            gameObject.GetComponentInChildren<TextMeshPro>().SetText("");
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) //TODO: Change to bindings 
        {
            if (!transform.parent.GetComponent<scr_PortGate>().active) //put key on portal recepticle
            {
                scr_KeyData keyUsed = other.GetComponent<scr_Inventory>().useKey();
                
                transform.parent.GetComponent<scr_PortGate>().activatePortal(keyUsed);
                Instantiate(keyUsed.keyStaticPrefab, transform);
            }
            else //take key from portal recepticle
            {
                
            }
        }
    }
}
