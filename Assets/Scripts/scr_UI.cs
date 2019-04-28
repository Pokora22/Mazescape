using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scr_UI : MonoBehaviour
{
    public float msgTimeout = 5;
    
    private TextMeshProUGUI splash;
    
    private void Awake()
    {
        splash = GameObject.FindGameObjectWithTag("SplashText").GetComponent<TextMeshProUGUI>();
    }

    public void displayMsg(string msg)
    {
        splash.text = msg;

        StartCoroutine(clearSplash());
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
}
