using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerAssistant : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float hitRange;
    public int damage = 20;
    public int inventory;
    public int maxInventory;
    Tree nearestTree = null;

    Scene currentScene;
    GameData gameData;

    private Animator animator;
    private Rigidbody rb;
    Storage storage;

    public Light light;


    void Start()
    {

        maxInventory = 10;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GameObject storageObj = GameObject.FindGameObjectWithTag("storage");
        storage = storageObj.GetComponent<Storage>();
        

        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level 3")
        {
            light.enabled = true;
        }

        gameData = FindObjectOfType<GameData>();

        
        if (gameData != null)
        {
            inventory = gameData.assistantInventory;
            if(gameData.wasUpgraded)
            {
                maxInventory = gameData.assistantMInventory;
                moveSpeed = gameData.assistantSpeed;
            }
            
        }

        StartCoroutine(AssistantBehavior());
    }

    

    IEnumerator AssistantBehavior()
    {
        while (true)
        {
            if(inventory >= maxInventory)
            {
                GoToStorage();
            }
            else if (inventory < maxInventory)
            {
                gameData.assistantInventory = inventory;
                FindNearestTree();

                if (nearestTree != null)
                {

                    if (inventory < maxInventory)
                    {
                        GoToTree(nearestTree);
                        CutTree(nearestTree);
                    }

                }
                else
                {
                    MoveAssistant();
                }
            }
            

            yield return null;
        }
    }

    public void MoveAssistant()
    {
        animator.SetBool("isMoving", true);

        Vector3 moveDirection = transform.forward;

        moveDirection.Normalize();

        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);

        bool shouldMove = true;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("shop") || collider.CompareTag("storage") || collider.CompareTag("lamp"))
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                shouldMove = false;
                break; 
            }
        }

        if (shouldMove)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }




    private void FindNearestTree()
    {
        float closestDistance = float.MaxValue;
        nearestTree = null;

        Collider[] objects = Physics.OverlapSphere(transform.position, 50f);
        foreach (Collider collider in objects)
        {
            if (collider.gameObject.CompareTag("tree"))
            {
                Tree tree = collider.GetComponent<Tree>();
                if (tree != null)
                {
                    float distance = Vector3.Distance(transform.position, tree.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearestTree = tree;
                    }
                }

            }
        }

        
    }

    private void GoToTree(Tree tree)
    {
        animator.SetBool("isMoving", true);

        float distanceFromTree = 4f;

        Vector3 directionToTree = tree.transform.position - transform.position;
        Vector3 targetPosition = tree.transform.position - directionToTree.normalized * distanceFromTree;
       
        targetPosition.y = transform.position.y;
        
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 5f);
        foreach (var nearbyObject in nearbyObjects)
        {
            if (nearbyObject.CompareTag("shop") || nearbyObject.CompareTag("storage") || nearbyObject.CompareTag("lamp") )
            {
                MoveAssistant();
                break;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (tree.transform.position - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

        
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

    }

    private void CutTree(Tree tree)
    {
        float distance = Vector3.Distance(transform.position, tree.transform.position);
        if(distance <= 5f)
        {
            StartCoroutine(CutTreeCoroutine(tree));
        }
    }

    private IEnumerator CutTreeCoroutine(Tree tree)
    {
        
        while (tree.currentHealth > 0)
        {
            animator.SetBool("isHitting", true);
            yield return new WaitForSeconds(1f);
            tree.TakeDamage(damage);

            animator.SetBool("isHitting", false);

        }

        
    }

    private void GoToStorage()
    {
        Vector3 targetPosition = storage.transform.position;
        targetPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, 5f);
        foreach (var nearbyObject in nearbyObjects)
        {
            if (nearbyObject.CompareTag("shop") || nearbyObject.CompareTag("storage"))
            {
                targetPosition += transform.right * 3f;
                break; 
            }
        }
    }

    

    
}
