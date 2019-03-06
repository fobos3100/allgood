using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public GameObject PauseUI;

    private bool paused = false;

    private void Start()
    {
        PauseUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
            //Player.canMove = false;
        }

        if (!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
            //Player.canMove = true;
        }
    }

    public void Resume()
    {
        paused = false;
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Options()
    {

    }
    
    public void Quit()
    {
        //Application.LoadLevel(1); //go to main menu
        Application.Quit();
    }
}
