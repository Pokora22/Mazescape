using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Inventory : MonoBehaviour
{
    public scr_KeyData activeKey;
    public bool debug = false;

    private List<scr_KeyData> keys;
    // Start is called before the first frame update
    void Start()
    {
        keys = new List<scr_KeyData>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(debug) Debug.Log("Active key: " + activeKey);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            activeKey = keys[0];
            if(debug) Debug.Log("Changed active key to: " + activeKey);
        }
    }

    public scr_KeyData addKey(scr_KeyData key)
    {
        if(debug) Debug.Log("Added to inventory: " + key);
        keys.Add(key);
        return key;
    }

    public scr_KeyData removeKey(scr_KeyData key)
    {
        if(debug) Debug.Log("Removed from inventory: " + key.ToString());
        keys.Remove(key);
        return key;
    }

    public scr_KeyData useKey()
    {
        if(activeKey == null) Debug.Log("Active key is null!");
        
        return removeKey(activeKey);
    }
}
