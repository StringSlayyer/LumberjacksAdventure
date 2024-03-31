using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void NewGame()
    {
        if(GameData.Instance != null) 
        { 
            GameData.Instance.ResetGameData();
        }
        SceneManager.LoadScene("Level 1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
