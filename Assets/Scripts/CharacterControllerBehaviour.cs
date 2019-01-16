using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerBehaviour : MonoBehaviour
{
   
    [Header("Locomotion Parameters")]
    [SerializeField]
    private float _mass = 75; // [kg]

    [SerializeField]
    private float _acceleration = 3; // [m/s^2]

    [SerializeField]
    private float _dragOnGround = 1; // []

    [SerializeField]
    private float _maxRunningSpeed = (30.0f * 1000) / (60 * 60); // [m/s], 30 km/h

    private float _beginMaxRunningSpeed;

    [SerializeField]
    private float _sprintingMultiplier;

    [Header("Dependencies")]
    [SerializeField, Tooltip("What should determine the absolute forward when a player presses forward.")]
    private Transform _absoluteForward;

    private CharacterController _characterController;

    private Vector3 _velocity = Vector3.zero;

    private Vector3 _movement;

    public bool _isSprinting;

    public bool _isPickingUpAxe;

    public bool _stopMovement;

    public bool _isHoldingAxe;

    public bool _isHoldingLog;

    public bool _swingAxe;

    public bool _hitTree;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _isSprinting = false;

        _stopMovement = false;

        _isHoldingAxe = false;

        _isHoldingLog = false;

        _hitTree = false;

        _beginMaxRunningSpeed = _maxRunningSpeed;

    }

    void Update()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        ApplyGround();
        ApplyGravity();


        if (_stopMovement == false)
        {
            if(_isHoldingLog == false && _isHoldingAxe == false)
            {
                ApplySprint();
            }

            ApplyMovement();
            ApplyGroundDrag();
            DropItem();
            SwingAxe();
            _characterController.Move(_velocity * Time.deltaTime);
        }

        LimitMaximumRunningSpeed();

    }

    private void DropItem()
    {
        if(Input.GetButtonDown("Fire2") && (_isHoldingAxe == true || _isHoldingLog == true) && _movement == Vector3.zero)
        {
            Debug.Log("[CHAR] Dropped");
            _isHoldingAxe = false;
            _isHoldingLog = false;
        }
    }

    private void SwingAxe()
    {
        if (Input.GetButtonDown("XboxRB") && _isHoldingAxe == true && _swingAxe == false)
        {
            Debug.Log("[CHAR] Swing Axe");
            _swingAxe = true;
            
        }
    }

    private void ApplySprint()
    {
        if (Input.GetButtonDown("Fire3") &&
            ((Input.GetAxis("Horizontal") > 0.9f || (Input.GetAxis("Vertical") > 0.9f)) ||
            ((Input.GetAxis("Horizontal") < -0.9f || (Input.GetAxis("Vertical") < -0.9f)))))
        {
            _maxRunningSpeed += _sprintingMultiplier;
            _isSprinting = true;
        }

        if (Input.GetButtonUp("Fire3"))
        {
            _maxRunningSpeed = _beginMaxRunningSpeed;
            _isSprinting = false;
        }
    }

    private void ApplyGround()
    {
        if (_characterController.isGrounded)
        {
            _velocity -= Vector3.Project(_velocity, Physics.gravity.normalized);
        }
    }

    private void ApplyGravity()
    {

            _velocity += Physics.gravity * Time.deltaTime; // g[m/s^2] * t[s]

    }

    private void ApplyMovement()
    {
        if (_characterController.isGrounded)
        {
            Vector3 xzAbsoluteForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));

            Quaternion forwardRotation =
                Quaternion.LookRotation(xzAbsoluteForward);

            Vector3 relativeMovement = forwardRotation * _movement;

            _velocity += relativeMovement * _mass * _acceleration * Time.deltaTime; // F(= m.a) [m/s^2] * t [s]
        }
    }

    private void ApplyGroundDrag()
    {
        if (_characterController.isGrounded)
        {
            _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void LimitMaximumRunningSpeed()
    {
        Vector3 yVelocity = Vector3.Scale(_velocity, new Vector3(0, 1, 0));

        Vector3 xzVelocity = Vector3.Scale(_velocity, new Vector3(1, 0, 1));
        Vector3 clampedXzVelocity = Vector3.ClampMagnitude(xzVelocity, _maxRunningSpeed);

        _velocity = yVelocity + clampedXzVelocity;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Axe" && 
            Input.GetButtonDown("Fire1") && 
            _movement == Vector3.zero && 
            _isHoldingAxe == false && 
            _isHoldingLog == false)
        {
            Debug.Log("[CHAR] Picking Up");
            _isPickingUpAxe = true;
        }


    }




}
