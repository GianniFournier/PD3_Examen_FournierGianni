using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour {

    [SerializeField]
    private CharacterControllerBehaviour _charCTRLScript;

    [SerializeField]
    private CharacterController _charCTRL;

    [SerializeField]
    private Animator _anim;
	
	void Update ()
    {
        Movement();
        Sprint();
        PickUpAxe();
        HoldingAxe();
        SwingAxe();
        PickUpLog();
        HoldLog();
    }

    private void HoldLog()
    {
        if(_charCTRLScript._isHoldingLog == true)
        {
            Debug.Log("[ANIMATOR] Holding Log");
            _anim.SetBool("IsHoldingLog", true);
        }
        else
        {
            _anim.SetBool("IsHoldingLog", false);
        }
        
    }

    private void PickUpLog()
    {
        if (_charCTRLScript._isPickingUpLog == true)
        {
            _anim.SetTrigger("PickUpTrigger");
            Debug.Log("[ANIMATOR] Is Picking Up Log");
            _charCTRLScript._isPickingUpLog = false;
            _charCTRLScript._isHoldingLog = true;
        }
    }

    private void SwingAxe()
    {
        if (_charCTRLScript._swingAxe == true)
        {
            _anim.SetBool("SwingingAxeBool", true);
            _anim.SetTrigger("SwingingAxe");
            Debug.Log("[ANIMATOR] Swinging Axe");
        }
    }

    private void Movement()
    {
        if (_charCTRLScript._stopMovement == false)
        {            
            Vector3 XZvel = Vector3.Scale(_charCTRL.velocity, new Vector3(1, 0, 1));
            Vector3 localVelXZ = _charCTRLScript.gameObject.transform.InverseTransformDirection(XZvel);
            localVelXZ.Normalize();

            _anim.SetFloat("HorizontalVelocity", localVelXZ.x);
            _anim.SetFloat("VerticalVelocity", localVelXZ.z);
        }
    }

    private void Sprint()
    {
        if (_charCTRLScript._isSprinting == true)
        {
            _anim.SetBool("Sprinting", true);
            Debug.Log("[ANIMATOR] Is Sprinting");
        }

        if (_charCTRLScript._isSprinting == false)
        {
            _anim.SetBool("Sprinting", false);
        }
    }

    private void PickUpAxe()
    {

        if (_charCTRLScript._isPickingUpAxe == true)
        {
            _anim.SetTrigger("PickUpTrigger");
            Debug.Log("[ANIMATOR] Is Picking Up Axe");
            _charCTRLScript._isPickingUpAxe = false;
            _charCTRLScript._isHoldingAxe = true;
        }
    }

    private void HoldingAxe()
    {
        if(_charCTRLScript._isHoldingAxe == true)
        {
            _anim.SetBool("IsHoldingAxe", true);
        }
    }

    //-------------EVENTS----------------\\

    public void StopMovement()
    {
        _charCTRLScript._stopMovement = true;
    }

    public void StartMovement()
    {
        _charCTRLScript._stopMovement = false;
    }

    public void SwingAxeStopped()
    {

        _charCTRLScript._swingAxe = false;
        _anim.SetBool("SwingingAxeBool", false);

    }

    public void HitTree()
    {
        _charCTRLScript._hitTree = true;
    }

    public void HitTreeStop()
    {
        _charCTRLScript._hitTree = false;
    }
}
