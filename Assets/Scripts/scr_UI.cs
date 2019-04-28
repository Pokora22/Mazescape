using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scr_UI : MonoBehaviour
{
    public float msgTimeout = 5;
    
    private TextMeshPro splash;
    
    private void Awake()
    {
        splash = GameObject.FindGameObjectWithTag("SplashText").GetComponent<TextMeshPro>();
    }

    public void displayMsg(string msg)
    {
        splash.text = msg;
    }

    private IEnumerator clearSplash()
    {
        yield return new WaitForSeconds(msgTimeout);
        splash.text = "";
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
