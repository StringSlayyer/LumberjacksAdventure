using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using System.Net;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;


    public float rotationSpeed;

    private Animator animator;



    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    public float hitRange;

    float horizontalInput;
    float verticalInput;
    public int punchDamage = 20;
    Vector3 moveDirection;

    public int treeCount;
    public int money;
    public int DarkTrees;
    Rigidbody rb;
    bool isHitting = false;

    public GameObject pauseWindow;

    AudioManager audioManager;

    Shop shop;
    Storage storage;
    public GameData gameData;
    public AudioSource cutSound;
    
    public TextMeshProUGUI trees;

    Scene currScene;

    Flashlight flashlight;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Time.timeScale = 1f;

        GameObject shopObject = GameObject.FindGameObjectWithTag("shop");
        if (shopObject != null )
        {
            shop = shopObject.GetComponent<Shop>();
        }else
        {
            Debug.Log("obchod nenalezen");
        }
        

        gameData = FindObjectOfType<GameData>();

        // If GameData is found, copy relevant data
        if (gameData != null)
        {
            treeCount = gameData.treeCount;
            money = gameData.moneyCount;
            if(moveSpeed < gameData.playerSpeed)
            {
                moveSpeed = gameData.playerSpeed;
            }
            else
            {
                gameData.playerSpeed = moveSpeed;
            }
            if(punchDamage < gameData.playerDamage)
            {
                punchDamage = gameData.playerDamage;
            }
            else
            {
                gameData.playerDamage = punchDamage;
            }
        }
        else
        {
            Debug.LogError("GameData not found.");
        }


        GameObject shopObj = GameObject.FindGameObjectWithTag("shop");
        if (shopObject != null)
        {
            shop = shopObj.GetComponent<Shop>();
        }

        GameObject storageObj = GameObject.FindGameObjectWithTag("storage");
        if (storageObj != null)
        {
            storage = storageObj.GetComponent<Storage>();
        }

        currScene = SceneManager.GetActiveScene();
        gameData.currScene = currScene.name;
        Debug.Log("momentalni scena " +  gameData.currScene);

        audioManager = FindObjectOfType<AudioManager>();

    }
    int i = 0;
    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        rb.drag = groundDrag;

        trees.text = treeCount.ToString();

        gameData.moneyCount = money;
        gameData.treeCount = treeCount;

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            if (flashlight != null && flashlight.isOn == true)
            {
                animator.SetBool("light", false);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shop!=null || storage!=null)
            {
               
                if (shop.panel.activeSelf || storage.storage.activeSelf)
                {
                    Debug.Log("obchod je otevreny");
                }
                else
                {
                    pauseWindow.SetActive(true);
                }
            }
            else
            {
                pauseWindow.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(0) && isHitting == false) // Assuming left mouse button
        {
            animator.SetBool("isHitting", true);
            audioManager.Play("axeSwing");
            CheckTrees();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("isHitting", false);
        }

        if(Input.GetKeyDown(KeyCode.F) && flashlight != null)
        {
            flashlight.ToggleFlashlight();
            if(flashlight.isOn)
            {
                animator.SetBool("torch", true);
            }
            else
            {
                animator.SetBool("torch", false);
            }
        }

        
    }

    private void MovePlayer()
    {
        Debug.Log("move 1");
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // rotate the player towards the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            // Use Time.deltaTime instead of Time.fixedDeltaTime
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        Debug.Log("move 2");
        if(flashlight != null && flashlight.isOn)
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("light", true);
        }
        else if(flashlight == null || flashlight.isOn == false)
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("light", false);
        }
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        Debug.Log("move 3");
    }

    private void CheckTrees()
    {
        isHitting = true;
        Collider[] objects = Physics.OverlapSphere(transform.position, hitRange);
        foreach (Collider collider in objects)
        {
            if (collider.gameObject.CompareTag("tree"))
            {
                Debug.Log("strom");
                Tree tree = collider.gameObject.GetComponent<Tree>();
                cutSound = tree.GetComponent<AudioSource>();
                audioManager.Play("axeChop");
                Debug.Log("sekam");
                tree.TakeDamage(punchDamage);
                Debug.Log("dosekal jsem");
            }
        }
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        
        yield return new WaitForSeconds(0.8f);

        isHitting = false;
    }


    private void SpeedControl()
{
    Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    // Limit velocity if needed
    if (flatVel.magnitude > moveSpeed)
    {
        Vector3 limitedVel = flatVel.normalized * moveSpeed;
        rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
    }
    
}


    public void FindFlashlight()
    {
        GameObject flashlightObj = GameObject.FindGameObjectWithTag("flashlight");
        flashlight = flashlightObj.GetComponent<Flashlight>();
    }

}