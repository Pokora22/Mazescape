using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Inventory : MonoBehaviour
{
    public scr_KeyData activeKey;
    public Transform itemDisplayPos;
    public bool debug = false;

    private GameObject itemDisplayed;
    private List<scr_KeyData> keys;
    // Start is called before the first frame update
    void Start()
    {
        keys = new List<scr_KeyData>();
    }

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel < 0 && keys.Count != 0)
        {
            int newIndex = (keys.FindIndex(data => activeKey) - 1) % keys.Count;
            if (newIndex < 0) newIndex = keys.Count - 1;
            
            activeKey = keys[newIndex];
        }
        else if (scrollWheel > 0 && keys.Count != 0)
        {
            int newIndex = (keys.FindIndex(data => activeKey) + 1) % keys.Count;
            activeKey = keys[newIndex];
        }
    }

    public scr_KeyData addKey(scr_KeyData key)
    {
        if(debug) Debug.Log("Added to inventory: " + key);
        if (keys.Count == 0) activeKey = key;
        keys.Add(key);
        
        return key;
    }

    public scr_KeyData removeKey(scr_KeyData key)
    {
        if(debug) Debug.Log("Removed from inventory: " + key.ToString());
        activeKey = null;
        keys.Remove(key);
        return key;
    }

    public scr_KeyData useKey()
    {
        if(activeKey == null) Debug.Log("Active key is null!");
        
        return removeKey(activeKey);
    }

    private void changeItemDisplayed()
    {
        if(itemDisplayed != null) Destroy(itemDisplayed);
        itemDisplayed = Instantiate(activeKey.keyPickUpPrefab);
    }
}
