using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [SerializeField]
    private float mouseSensitivityX = 500f;
    [SerializeField]
    private float mouseSensitivityY = 300f;

    [Header("General Player Movement")]
    [SerializeField]
    [Min(0.1f)][Tooltip("How fast the player can move.")]
    private float movementSpeed = 12f;

    [SerializeField]
    [Min(0.1f)][Tooltip("How high the player can jump.")]
    private float jumpHeight = 4f;

    [SerializeField]
    [Min(-0.1f)]
    private float gravity = -9.81f;
    
    [SerializeField]
    [Tooltip("Length of the ray, which goes down to check for any collision.")]
    private float groundCheckRay = 2;

    [Header("Debug")]
    [SerializeField]
    [Tooltip("Makes, that you can see the ground-collision-ray.")]
    private bool showGroundRay = false;
    
    private Transform _playerBody;
    private CharacterController _controller;
    private Camera _mainCam;
    private float _xRotation = 0f;
    private Vector3 _velocity;
    private bool _isGrounded = false;

    void Start()
    {
        _playerBody = this.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        _mainCam = Camera.main;
        _controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        //- Only Camera Movement
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 60f);
        
        _mainCam.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        _playerBody.Rotate(Vector3.up * mouseX);
        //- Only Camera Movement

        
        //- Only Player Movement
        var movementX = Input.GetAxis("Horizontal");
        var movementY = Input.GetAxis("Vertical");

        var move = transform.right * movementX + transform.forward * movementY;

        _controller.Move(move * (movementSpeed * Time.deltaTime));
        //- Only Player Movement

        
        //- Only Gravity including Jump
        if (Physics.Raycast(transform.position,Vector3.down,groundCheckRay))
        {
            _isGrounded = true;
        }
        else { _isGrounded = false; }
        
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _isGrounded = false;
        }
        
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        if (_isGrounded)
        {
            _velocity.y = 0;
        }
        //- Only Gravity including Jump
    }

    private void OnDrawGizmos()
    {
        if (showGroundRay)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckRay);
        }
    }
}
