﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Inventory : MonoBehaviour
{
    public scr_KeyData activeKey;
    public Transform itemDisplayPos;

    private GameObject itemDisplayed;
    private float itemSwappedRecently;
    public List<scr_KeyData> keys;
    // Start is called before the first frame update
    void Start()
    {
        keys = new List<scr_KeyData>();
    }

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0) 
            changeSelection(scrollWheel);
    }

    private void changeSelection(float direction)
    {
        if (keys.Count == 0)
        {
            activeKey = null;
            changeItemDisplayed();
            return;
        }

        int newIndex;
        if (direction < 0)
        {
            newIndex = (keys.FindIndex(data => data.Equals(activeKey)) - 1) % keys.Count;
            if (newIndex < 0) newIndex = keys.Count - 1;
        }
        else
        {
            newIndex = (keys.FindIndex(data => data.Equals(activeKey)) + 1) % keys.Count;
        }
         
        activeKey = keys[newIndex];
        changeItemDisplayed();
    }

    public scr_KeyData addKey(scr_KeyData key)
    {
        if (keys.Count == 0)
        {
            activeKey = key;
            changeItemDisplayed();
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

    private void changeItemDisplayed()
    {
        if(itemDisplayed) Destroy(itemDisplayed);
        
        if (activeKey)
        {
            itemDisplayed = activeKey.spawn(itemDisplayPos, false);
            itemDisplayed.layer = 9;
            itemDisplayed.transform.GetChild(0).gameObject.layer = 9;
        }
    }
}
