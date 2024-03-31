using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tree : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private PlayerMovement player;
    private PlayerAssistant assistant;

    GameData gameData;

    void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerMovement>();
            
        }

        gameData = FindObjectOfType<GameData>();

    }

    void Update()
    {
        

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        if(currentHealth <= 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    CutDownTree();
                }
                else if (collider.CompareTag("helper"))
                {
                    FindAssistant();
                    AssistantCutTree();
                }
            }
        }
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }

    void CutDownTree()
    {
        if (player != null)
        {
            player.treeCount++;
            gameData.TotalPlayerTrees++;
            gameData.TotalTrees++;
            if (gameData.currScene == "Level 3")
            {
                player.DarkTrees++;
            }
        }

        Destroy(gameObject);
    }

    void AssistantCutTree()
    {
        if (assistant != null)
        {
            assistant.inventory++;
            gameData.TotalAssistantTrees++;
            gameData.TotalTrees++;
        }
        Destroy(gameObject);
    }

    public void FindAssistant()
    {
        GameObject assistantObject = GameObject.FindGameObjectWithTag("helper");

        if (assistantObject != null)
        {
            assistant = assistantObject.GetComponent<PlayerAssistant>();  
        }
        
    }
}
