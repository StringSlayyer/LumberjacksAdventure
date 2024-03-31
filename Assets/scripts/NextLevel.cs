using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour

{

    private PlayerMovement player;
    public GameObject text;
    Scene currScene;


    private bool isPlayer;
    private bool wasPlayer = false; 
    void Start()
    {

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerMovement>();
        }


        currScene = SceneManager.GetActiveScene();

    }

    private void Update()
    {
        CheckPlayer();
        if(Input.GetKeyDown(KeyCode.E) && isPlayer == true) {
            text.SetActive(false);
            if(currScene.name == "Level 1")
            {
                SceneManager.LoadScene("Level 2");
            }
            else if(currScene.name == "Level 2")
            {
                SceneManager.LoadScene("Level 3");
            }
            else if (currScene.name == "Level 3")
            {
                SceneManager.LoadScene("Stats");
            }
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 25f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("tree"))
            {
                Destroy(collider.gameObject);
            }
        }
    }

    private void CheckPlayer()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, 25f);

        
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
                text.SetActive(true);
            }
            else
            {
                text.SetActive(false);
            }
        }

        wasPlayer = isPlayer;
    }
}
