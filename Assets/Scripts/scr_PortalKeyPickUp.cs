using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scr_PortalKeyPickUp : MonoBehaviour
{
    public scr_KeyData keyData;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<scr_Inventory>().addKey(keyData); //Add key data to inventory
            Destroy(gameObject); //destroy pickable key instance
        }
    }

    public void setSymbolAndColor(scr_KeyData keyData)
    {
        this.keyData = keyData;

        TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();
        tmp.text = keyData.symbol;
        tmp.color = keyData.color;
    }
}
