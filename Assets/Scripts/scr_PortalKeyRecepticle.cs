using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Utility;

public class scr_PortalKeyRecepticle : MonoBehaviour
{
    public string putKeyText, takeKeyText;

    private GameObject keyPlaced;
    private bool mouseAxisInUse;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showUseText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            gameObject.GetComponentInChildren<TextMeshPro>().SetText("");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerClick())
            {
                scr_Inventory playerInventory = other.GetComponent<scr_Inventory>();
                if (!transform.parent.GetComponent<scr_PortGate>().active) //put key on portal recepticle
                {
                    scr_KeyData keyUsed = playerInventory.activeKey; //Get active (used) key data from player inventory

                    if (keyUsed != null)
                    {
                        if (keyUsed.transform.GetComponentInParent<scr_PortGate>().active)
                        {
                            GameObject.FindGameObjectWithTag("UI").GetComponent<scr_UI>().displayMsg("Some force is pushing the key back...");
                            return;
                        }

                        transform.parent.GetComponent<scr_PortGate>().activatePortal(keyUsed);
                        keyPlaced = keyUsed.spawn(transform, true);
                        keyPlaced.transform.GetChild(0).localPosition = new Vector3(.1f, 0, 0); //Change symbol position

                        playerInventory.removeKey(keyUsed);
                        
                        showUseText();
                    }
                }
                else if (keyPlaced) //take key from portal recepticle 
                {
                    scr_KeyData keyData = keyPlaced.GetComponent<scr_PortalKeyPickUp>().keyData;
                    playerInventory.addKey(keyData); //Add the data from the pickup back to player inventory
                    transform.parent.GetComponent<scr_PortGate>().deactivatePortal(keyData);
                    Destroy(keyPlaced);
                    showUseText();
                }
            }
        }
    }
    
    private void showUseText()
    {
        if (!keyPlaced)
            gameObject.GetComponentInChildren<TextMeshPro>().SetText(putKeyText);
        else
            gameObject.GetComponentInChildren<TextMeshPro>().SetText(takeKeyText);
    }

    private bool playerClick()
    {
        if (Input.GetAxisRaw("Fire1") != 0)
            if (mouseAxisInUse == false)
                return mouseAxisInUse = true;
            else
                return false;            
        
        return mouseAxisInUse = false;
    }
}
