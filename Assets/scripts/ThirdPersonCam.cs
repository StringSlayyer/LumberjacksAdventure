using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Transform player;
    public Rigidbody rb;

    public float rotationSpeed;

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 

    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            player.rotation = Quaternion.Lerp(player.rotation, Quaternion.LookRotation(inputDir.normalized, Vector3.up), Time.deltaTime * rotationSpeed);
        }
    }

}
