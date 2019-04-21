using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PortalKeyPickUp : MonoBehaviour
{
    public scr_KeyData keyData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Key: " + gameObject);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<scr_Inventory>().addKey(keyData); //Add key data to inventory
            Destroy(gameObject); //destroy pickable key instance
        }
    }

    public void setData(scr_KeyData keyData)
    {
        this.keyData = keyData;
    }
}
