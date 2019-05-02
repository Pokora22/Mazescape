using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityStandardAssets.Utility;

public class scr_ExitPuzzle: MonoBehaviour
{
    public string putKeyText, takeKeyText;

    private GameObject keyPlaced;
    private GameObject lift;
    private scr_ExitLift liftScript;
    [SerializeField] private Transform keyPosition;
    private bool mouseAxisInUse;
    private bool active;


    private void Awake()
    {
        lift = GameObject.FindGameObjectWithTag("ExitLift");
        liftScript = lift.GetComponent<scr_ExitLift>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showUseText();
        }
    }

    private void showUseText()
    {
        if (active)
            gameObject.GetComponentInChildren<TextMeshPro>().SetText(putKeyText);
        else
            gameObject.GetComponentInChildren<TextMeshPro>().SetText(takeKeyText);
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
                scr_KeyData keyUsed = playerInventory.activeKey; //Get active (used) key data from player inventory

                if (!active) //put key on portal recepticle
                {
                    if (keyUsed != null)
                    {
                        if (keyUsed.transform.root != transform.root)
                        {
                            GameObject.FindGameObjectWithTag("UI").GetComponent<scr_UI>()
                                .displayMsg("Some force is pushing the key back...");
                            return;
                        }

                        active = true;
                        keyPlaced = keyUsed.spawn(transform, false);
                        keyPlaced.GetComponent<BoxCollider>().isTrigger = false;
                
                        playerInventory.removeKey(keyUsed);
                        
                        showUseText();
                        
                        updateLift(-1);
                    }
                }

                else if (keyPlaced) //take key from portal recepticle 
                {
                    scr_KeyData keyData = keyPlaced.GetComponent<scr_PortalKeyPickUp>().keyData;
                    playerInventory.addKey(keyData); //Add the data from the pickup back to player inventory
                    active = false;
                    Destroy(keyPlaced);
                    
                    updateLift(1);
                }
            }
        }
    }

    private void updateLift(float direction)
    {
        liftScript.targetHeight = liftScript.targetHeight + (liftScript.maxHeight / 4) * direction;
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
