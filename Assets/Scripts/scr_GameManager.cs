using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_GameManager : MonoBehaviour
{
    public enum SCENE{MAIN_MENU, MAZE, DEATH, GAME_OVER, EXIT}
    
    
    public List<GameObject> keySpawns;
    public bool randomKeyPlacement, debug;

    private GameObject[] redPortals, bluePortals, greenPortals, whitePortals, masterPortals;
    private int maxPortals = 0;
    
    void Awake(){
            DontDestroyOnLoad(this);
        }

    public void loadScene(SCENE scene)
    {
        switch (scene)
        {
            case SCENE.MAIN_MENU:
                SceneManager.LoadScene("MainMenu");
                break;
            case SCENE.MAZE:
                SceneManager.LoadScene("Maze");
                StartCoroutine(InitializeMaze());
                break;
            case SCENE.DEATH:
                SceneManager.LoadScene("You Died");
                break;
            case SCENE.GAME_OVER:
                SceneManager.LoadScene("GameOver");
                break;
            case SCENE.EXIT:
                Application.Quit();
                break;
        }
    }
    

    public void startGame()
    {
        SceneManager.LoadScene("Maze");
    }

    public void exit()
    {
        Application.Quit();
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void deathScreen()
    {
        SceneManager.LoadScene("You Died");
    }
    
    private IEnumerator InitializeMaze()
    {
        Debug.Log("Game initializing...");
        while (!SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Maze")))
        {
            Debug.Log("Waiting...");
            yield return null;
        }

        whitePortals = GameObject.FindGameObjectsWithTag("WhitePortal"); //Sky - Teal
        if (whitePortals.Length > maxPortals) maxPortals = whitePortals.Length;
        redPortals = GameObject.FindGameObjectsWithTag("RedPortal");
        if (redPortals.Length > maxPortals) maxPortals = redPortals.Length;
        bluePortals = GameObject.FindGameObjectsWithTag("BluePortal");
        if (bluePortals.Length > maxPortals) maxPortals = bluePortals.Length;
        greenPortals = GameObject.FindGameObjectsWithTag("GreenPortal");
        if (greenPortals.Length > maxPortals) maxPortals = greenPortals.Length;
        masterPortals = GameObject.FindGameObjectsWithTag("MasterPortal");
        if (masterPortals.Length > maxPortals) maxPortals = masterPortals.Length;
        
        keySpawns = GameObject.FindGameObjectsWithTag("KeySpawnPoint").ToList();
        Debug.Log(keySpawns.Count);
        
        for (int i = 0; i < maxPortals; i++)
        {
            if (i < whitePortals.Length)
                whitePortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement); //always one in green zone
            if(i < redPortals.Length)
                redPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement); //always one in white zone
            if(i < bluePortals.Length)
                bluePortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement); //always one in red zone
            if(i < greenPortals.Length)
                greenPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, randomKeyPlacement);
            
            if(i < masterPortals.Length)
                masterPortals[i].gameObject.GetComponent<scr_KeyData>().initializeKeyInWorld(keySpawns, false);
        }
    }

    private void Start()
    {
        
    }
}
