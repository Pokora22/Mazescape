﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Utility;

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

                if (keyUsed != null)
                {
                    transform.parent.GetComponent<scr_PortGate>().activatePortal(keyUsed);
                    placeStaticKey(keyUsed);
                }
            }
            else //take key from portal recepticle
            {
                
            }
        }
    }

    private void placeStaticKey(scr_KeyData keyUsed)
    {
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        GameObject keyPlaced = Instantiate(keyUsed.keyPickUpPrefab, position, rotation,
            transform); 
                    
        keyPlaced.GetComponent<Collider>().isTrigger = false;
        keyPlaced.GetComponent<AutoMoveAndRotate>().enabled = false;

        Transform symbolObject = keyPlaced.transform.GetChild(0);
        keyPlaced.GetComponent<scr_PortalKeyPickUp>().setSymbolAndColor(keyUsed);
        symbolObject.localPosition = new Vector3(.1f, 0, 0); 
    }
}
