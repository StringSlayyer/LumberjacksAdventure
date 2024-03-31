using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Storage : MonoBehaviour
{

    int capacity;
    private PlayerAssistant assistant;
    private PlayerMovement player;

    public GameObject storage;
    public GameObject text;
    public GameObject objectives;
    private bool isPlayer;
    private bool wasPlayer;

    public TextMeshProUGUI trees;

    AudioManager audioManager;

    private void Start()
    {
        text.SetActive(false);
        Collider[] objects = Physics.OverlapSphere(transform.position, 50f);
        int trees = 0;
        foreach (Collider collider in objects) {
            if (collider.gameObject.CompareTag("tree"))
            {
                trees++;
                Destroy(collider.gameObject);
            }
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<PlayerMovement>();
        }

        audioManager = FindObjectOfType<AudioManager>();

    }
    void Update()
    {

        CheckPlayer();

        if (Input.GetKeyDown(KeyCode.E) && isPlayer)
        {
            text.SetActive(false);
            storage.SetActive(true);
            objectives.SetActive(false);
            audioManager.Play("Open");
            PauseGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }


        if (Input.GetKeyDown(KeyCode.Escape) && storage.activeSelf)
        {
            
            storage.SetActive(false);
            objectives.SetActive(true);
            audioManager.Play("Open");
            ResumeGame();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }

        trees.text = capacity.ToString();

        Collider[] objects = Physics.OverlapSphere(transform.position, 18f);
        foreach (Collider collider in objects)
        {
            if (collider.gameObject.CompareTag("helper"))
            {
                getTreesFromAssistant(collider);
            }
        }
    }

    private void getTreesFromAssistant(Collider collider)
    {
        PlayerAssistant assistant = collider.gameObject.GetComponent<PlayerAssistant>();

        if (assistant != null)
        {
            capacity += assistant.inventory;
            assistant.inventory = 0;
            assistant.MoveAssistant();
        }
    }

    public void getTreesFromPlayer()
    {
        capacity += player.treeCount;
        player.treeCount = 0;
    }

    public void giveTreesToPlayer()
    {
        player.treeCount += capacity;
        capacity = 0;
    }

    private void CheckPlayer()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, 15f);

        isPlayer = false;

        foreach (Collider collider in objects)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                isPlayer = true;
                break;
            }
        }

        if (isPlayer != wasPlayer)
        {
            if (isPlayer)
            {
                text.SetActive(!storage.activeSelf);
            }
            else
            {
                text.SetActive(false);
            }
        }
        wasPlayer = isPlayer;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
