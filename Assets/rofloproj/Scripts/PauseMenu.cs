using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public PlayerMovement playerDeath;
    public Timeshift timeshift;

    public bool gamePause = false;
    public GameObject pausePanel;
    public GameObject joystick;

    void Awake()
    {
        playerDeath.alive = true;
        pausePanel.SetActive(false);
       
    }
    private void Start()
    {
        Invoke("Pause",0.01f);
    }
    public void ShowHighScore()
    {
        Social.ShowLeaderboardUI();
    }
    public void Resume()
    {
        joystick.SetActive(true);
        pausePanel.SetActive(false);
        gamePause = false;
        Time.timeScale = 1;
        if (playerDeath.alive == false)
        {
            joystick.SetActive(false);
        }

        /*
        if (timeshift.timeShift == true)
        {
            timeshift.TimeShiftOn();
        }
        else
        {
            Time.timeScale = 1;
        }
        */
    }
    public void Pause()
    {
        joystick.SetActive(false);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        gamePause = true;
    }
    void QuitGame()
    {
        Application.Quit();
    }
}