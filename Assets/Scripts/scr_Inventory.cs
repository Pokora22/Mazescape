using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Inventory : MonoBehaviour
{
    public scr_KeyData activeKey;
    public Transform itemDisplayPos;

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
        keys.Remove(key);
        changeSelection(-1);
        return key;
    }

    private IEnumerator changeItemDisplayed()
    {
        if(itemDisplayed) Destroy(itemDisplayed);
        
        if (activeKey)
        {
            itemDisplayed = activeKey.spawn(itemDisplayPos, false);
            itemDisplayed.layer = 9;
            itemDisplayed.transform.GetChild(0).gameObject.layer = 9;
        }

        yield return new WaitForSeconds(.5f);
    }
}
