using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{

    public TextMeshProUGUI totalTrees;
    public TextMeshProUGUI playerTrees;
    public TextMeshProUGUI assistantTrees;
    public TextMeshProUGUI totalMoney;
    public TextMeshProUGUI spentMoney;

    GameData gameData;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameData = FindObjectOfType<GameData>();
        if (gameData != null)
        {
            totalTrees.text = gameData.TotalTrees.ToString();
            playerTrees.text = gameData.TotalPlayerTrees.ToString();
            assistantTrees.text = gameData.TotalAssistantTrees.ToString();
            totalMoney.text = gameData.TotalEarnedMoney.ToString();
            spentMoney.text = gameData.TotalSpentMoney.ToString();
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
