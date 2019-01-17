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

    [SerializeField]
    private float _sprintingMultiplier;

    [SerializeField]
    private Vector3 _tempForward = Vector3.zero;
    [Header("Dependencies")]
    [SerializeField, Tooltip("What should determine the absolute forward when a player presses forward.")]
    private Transform _absoluteForward;

    private float _beginMaxRunningSpeed;

    private CharacterController _characterController;

    private Vector3 _velocity = Vector3.zero;

    private Vector3 _movement;

    [SerializeField]
    private Transform _modelTransform;

    private BoxCollider _col;

    [Header("Booleans")]
    public bool _isSprinting;

    public bool _isPickingUpAxe;

    public bool _stopMovement;

    public bool _isHoldingAxe;

    public bool _isHoldingLog;

    public bool _swingAxe;

    public bool _hitTree;

    public bool _isPickingUpLog;

    public bool _pickUpLogSpecific;

    public bool _dropLog;

    [Header("GameObjects")]

    [SerializeField]
    public GameObject _handAxe;

    [SerializeField]
    public GameObject _handLog;

    [SerializeField]
    public GameObject _axe;

    [SerializeField]
    public GameObject _axeActive;

    [SerializeField]
    public GameObject _log;

    [SerializeField]
    public GameObject _logActive;

    public bool _treeHit;

    public bool _isHoldingLogNow;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _isSprinting = false;

        _stopMovement = false;

        _isHoldingAxe = false;

        _isHoldingLog = false;

        _hitTree = false;

        _isHoldingLogNow = false;

        _pickUpLogSpecific = false;

        _dropLog = false;

        _beginMaxRunningSpeed = _maxRunningSpeed;

        _col = GetComponent<BoxCollider>();

    }

    void Update()
    {
        MovementUpdate();

    }

    private void MovementUpdate()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Vector3 rawMovement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (_isHoldingLog == true)
        {
            _movement /= 7.0f;
        }

        if (rawMovement.magnitude > 0)
        {
            _modelTransform.eulerAngles = Vector3.Scale(Vector3.up, Quaternion.LookRotation(_velocity).eulerAngles);
            Vector3 newCenter = _col.center;
            newCenter.x = -(rawMovement.normalized * 0.8f).z;
            newCenter.z = (rawMovement.normalized * 0.8f).x;
            _col.center = newCenter;
        }
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
            if(_isHoldingAxe == true)
            {
                Debug.Log("[CHAR] Axe Dropped");
                _axe.transform.SetParent(null);
                _axe.GetComponent<Rigidbody>().isKinematic = false;
                _axe.GetComponent<BoxCollider>().enabled = true;

                _isHoldingAxe = false;
            }

            if (_isHoldingLog == true && _movement == Vector3.zero)
            {
                Debug.Log("[CHAR] Log Dropped");

                _logActive.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                _logActive.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                _logActive.transform.GetChild(0).GetComponent<CapsuleCollider>().enabled = true;
                _logActive.transform.GetChild(0).transform.SetParent(null);

                if (_logActive.transform.childCount == 1)
                {
                        Destroy(_logActive.transform.GetChild(0).gameObject);
                }

                if (_logActive.transform.childCount == 2)
                {
                    Destroy(_logActive.transform.GetChild(0).gameObject);
                    Destroy(_logActive.transform.GetChild(0).gameObject);
                }


                _dropLog = true;
                _isHoldingLog = false;
            }

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
        if (Input.GetButtonDown("Fire3") && _movement != Vector3.zero)
        {
            _maxRunningSpeed += _sprintingMultiplier;
            _isSprinting = true;
        }

        if (Input.GetButtonUp("Fire3") || _movement == Vector3.zero)
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
            Debug.Log("[CHAR] Picking Up Axe");
            _isPickingUpAxe = true;
        }

        if (other.tag == "Log" &&
            Input.GetButtonDown("Fire1") &&
            _movement == Vector3.zero &&
            _isHoldingAxe == false &&
            _isHoldingLog == false
            )
        {
            Debug.Log("[CHAR] Picking Up Log");
            _isPickingUpLog = true;

            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<CapsuleCollider>().enabled = false;
            other.transform.SetParent(_logActive.transform);
            other.transform.position = _logActive.transform.position;
            other.transform.rotation = _logActive.transform.rotation;
        }

        

    }

}
