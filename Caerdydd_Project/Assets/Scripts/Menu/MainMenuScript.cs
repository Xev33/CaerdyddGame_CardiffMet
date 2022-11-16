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

    public void Play()
    {
        SceneManager.LoadScene(1);
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
