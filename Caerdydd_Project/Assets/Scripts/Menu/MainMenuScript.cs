using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelsMenu;
    public GameObject levelNotUnlocked;
    public GameObject lockObj;

    public void Update()
    {
        if (mainMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("joystick button 3"))
            {
                SceneManager.LoadScene(1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick button 1"))
            {
                Quit();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick button 2"))
            {
                OpenLevelsMenu();
            }
        }
        else if (levelsMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("joystick button 2"))
            {
                Level1();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("joystick button 1"))
            {
                if (GetUnlockedLevels())
                {
                    Level2();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("joystick button 0"))
            {
                BackToMain();
            }
        }
    }

    public void Play()
    {
        if (GetUnlockedLevels())
        {
            Level2();
        }
        else
        {
            Level1();
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit button pressed");
    }

    public void OpenLevelsMenu()
    {
        mainMenu.SetActive(false);

        if (GetUnlockedLevels())
        {
            lockObj.SetActive(false);
            levelsMenu.SetActive(true);
            levelNotUnlocked.GetComponent<Button>().enabled = true;
        }
        else
        {
            levelsMenu.SetActive(true);
            levelNotUnlocked.GetComponent<Button>().enabled = false;
            lockObj.SetActive(true);
        }
    }

    public void BackToMain()
    {
        mainMenu.SetActive(true);
        levelsMenu.SetActive(false);
    }

    public bool GetUnlockedLevels()
    {
        int isLevelUnlock;

        isLevelUnlock = PlayerPrefs.GetInt("IsLevel3Unlock", 0);

        if (isLevelUnlock == 1)
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Level2()
    {
        SceneManager.LoadScene(2);
    }
}
