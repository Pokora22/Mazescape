using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Inventory : MonoBehaviour
{
    public scr_KeyData activeKey;
    public Transform itemDisplayPos;
    public bool debug = false;

    private GameObject itemDisplayed;
    public List<scr_KeyData> keys;
    // Start is called before the first frame update
    void Start()
    {
        keys = new List<scr_KeyData>();
    }

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0) changeSelection((int)(scrollWheel * 10));
        
    }

    private void changeSelection(int direction)
    {
        if (keys.Count == 0)
        {
            activeKey = null;
            StartCoroutine(changeItemDisplayed());
            return;
        }
        
        int newIndex = (keys.FindIndex(data => data.Equals(activeKey)) - 1) % keys.Count;
        if (newIndex < 0) newIndex = keys.Count - 1;
        activeKey = keys[newIndex];
        
        StartCoroutine(changeItemDisplayed());
    }

    public scr_KeyData addKey(scr_KeyData key)
    {
        if(debug) Debug.Log("Added to inventory: " + key);
        if (keys.Count == 0)
        {
            activeKey = key;
            StartCoroutine(changeItemDisplayed());
        }
        keys.Add(key);
        
        return key;
    }

    public scr_KeyData removeKey(scr_KeyData key)
    {
        if(debug) Debug.Log("Removed from inventory: " + key.ToString());
        
        keys.Remove(key);
        changeSelection(-1);
        return key;
    }

    public scr_KeyData useKey()
    {
        if (activeKey == null)
        {
            Debug.Log("Active key is null!");
            return null;
        }
        
        return removeKey(activeKey);
    }

    private IEnumerator changeItemDisplayed()
    {
        Debug.Log("Changing item displayed...");
        if(itemDisplayed) Destroy(itemDisplayed);
        Debug.Log("Current active key is: " + activeKey);
        if (!activeKey)
        {
            Debug.Log("Active key: " + activeKey + "(should be null)");
            
        }
        else
        {
            itemDisplayed = activeKey.spawn(itemDisplayPos);
            itemDisplayed.layer = 9;
            itemDisplayed.transform.GetChild(0).gameObject.layer = 9;
        }

        yield return new WaitForSeconds(.5f);
    }
}
