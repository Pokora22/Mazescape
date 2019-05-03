using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ButtonHandler : MonoBehaviour
{
    private scr_GameManager GameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<scr_GameManager>();
    }

    public void start()
    {
        GameManager.loadScene(scr_GameManager.SCENE.MAZE);
    }

    public void exit()
    {
        Application.Quit();
    }

    public void menu()
    {
        GameManager.loadScene(scr_GameManager.SCENE.MAIN_MENU);
    }
}
