using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public int treeCount;
    public int currentTask;
    public int moneyCount;
    public bool isAssistant;
    public bool wasUpgraded = false;
    public bool flashlight = false;
    public int assistantMInventory = 10;
    public float assistantSpeed = 0;
    public int assistantInventory = 0;
    public int playerDamage = 0;
    public float playerSpeed = 0;
    public float upgrPDmgPrice;
    public float upgrASpeedPrice;
    public float upgrAInvPrice;
    public float upgrPSpeedPrice;
    public bool flashUpgrade = false;
    public int TotalTrees;
    public int TotalPlayerTrees;
    public int TotalAssistantTrees;
    public int TotalEarnedMoney;
    public int TotalSpentMoney;
    public string currScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetGameData()
    {
        treeCount = 0;
        currentTask = 0;
        moneyCount = 0;
        isAssistant = false;
        wasUpgraded = false;
        flashlight = false;
        assistantMInventory = 10;
        assistantSpeed = 0;
        assistantInventory = 0;
        playerDamage = 20;
        playerSpeed = 0;
        upgrPDmgPrice = 0; 
        upgrASpeedPrice = 0; 
        upgrAInvPrice = 0; 
        upgrPSpeedPrice = 0; 
        flashUpgrade = false;
        TotalTrees = 0;
        TotalPlayerTrees = 0;
        TotalAssistantTrees = 0;
        TotalEarnedMoney = 0;
        TotalSpentMoney = 0;
        currScene = null; 
    }
}