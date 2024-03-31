using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{

    private PlayerMovement player;

    int currTask = 0;

    public TextMeshProUGUI objective;
    public TextMeshProUGUI state;

    public GameObject stats;

    public GameObject nextLevel;
    Scene currentScene;

    string currObjective;
    string currState;


    public GameData gameData;






    void Start()
    {
        nextLevel.SetActive(false);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerMovement>();
           
        }


        gameData = FindObjectOfType<GameData>();

        if (gameData != null)
        {
            currTask = gameData.currentTask;
            player.treeCount = gameData.treeCount;

            nextLevel.SetActive(false);
            Tasks();
        }
    }

    void Update()
    {
        objective.text = currObjective;
        state.text = currState;
        
        CheckTaskState();

        gameData.currentTask = currTask;

    }

    void Tasks()
    {
        switch (currTask)
        {
            case 0:
                currObjective = "Pok�cej 10 strom�";
                currState = player.treeCount.ToString() + " / 10";
                break;

            case 1:
                currObjective = "Najdi budovu a dosta� se do dal��ho levelu";
                currState = "";
                break;
            case 2:
                currObjective = "Nat� stromy a prodej je v obchod�";
                break;
            case 3:
                currObjective = "Z�skej 500 minc� a kup si pomocn�ka";
                currState = "Po�et minc�: 0 / 500";
                break;
            case 4:
                currObjective = "Kup si vylep�en� na pomocn�ka";
                currState = "";
                break;
            case 5:
                currObjective = "A� bude� cht�t, dojdi k budov� a dosta� se do dal��ho levelu";
                break;
            case 6:
                currObjective = "Kup si v obchod� sv�tilnu";
                break;
            case 7:
                currObjective = "Vylep�i si v obchod� sv�tilnu";
                break;
            case 8:
                currObjective = "Najdi a nat� 10 strom�";
                break;
            case 9:
                currObjective = "Gratuluji, dokon�il jsi hru! A� bude� cht�t, m��e� odej�t p�es �edou budovu.";
                currState = "";
                break;

        }
    }

    void CheckTaskState()
    {
        switch (currTask)
        {
            case 0:
                if (player != null)
                {
                    currState = player.treeCount.ToString() + " / 10";
                    if (player.treeCount >= 10)
                    {
                        currTask += 1;
                        nextLevel.SetActive(true);
                        Tasks();
                    }
                }
                break;
            case 1:
                currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Level 2")
                {
                    currTask += 1;
                    Tasks();
                }
                break;
            case 2:
                if (player != null && player.money > 10)
                {
                    currTask += 1;
                    Tasks();
                }
                break;
            case 3:
                if (player != null)
                {
                    currState = "Po�et minc�: " + player.money + " / 250";
                    if (gameData.isAssistant)
                    {
                        currTask += 1;
                        Tasks();
                    }
                }
                break;
            case 4:
                if (gameData.wasUpgraded == true)
                {
                    currTask += 1;
                    nextLevel.SetActive(true);
                    Tasks();
                }
                break;
            case 5:
                currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Level 3")
                {
                    currTask += 1;
                    Tasks();
                }
                break;
            case 6:
                if(gameData.flashlight == true)
                {
                    currTask += 1;
                    Tasks();
                }
                break;
            case 7:
                if(gameData.flashUpgrade == true)
                {
                    currTask += 1;
                    Tasks();
                }
                break;
            case 8:
                currState = player.DarkTrees.ToString() + " / 10";
                if(player.DarkTrees >= 10)
                {
                    currTask += 1;
                    nextLevel.SetActive(true);
                    Tasks();
                }
                break;
        }
    }


}
