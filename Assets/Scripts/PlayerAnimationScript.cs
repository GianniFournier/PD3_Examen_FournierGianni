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

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
        CheckSprint();
    }

    private void Movement()
    {
        //adjust velocity to use in animation
        Vector3 XZvel = Vector3.Scale(_charCTRL.velocity, new Vector3(1, 0, 1));
        Vector3 localVelXZ = _charCTRLScript.gameObject.transform.InverseTransformDirection(XZvel);
        localVelXZ.Normalize();

        //set animation velocity
        _anim.SetFloat("HorizontalVelocity", localVelXZ.x);
        _anim.SetFloat("VerticalVelocity", localVelXZ.z);

    }

    private void CheckSprint()
    {
        if (_charCTRLScript._isSprinting == true)
        {
            _anim.SetBool("Sprinting", true);
            Debug.Log("[ANIMATOR] Is Sprinting");
        }

        if (_charCTRLScript._isSprinting == false)
        {
            _anim.SetBool("Sprinting", false);
            Debug.Log("[ANIMATOR] Stopped Sprinting");
        }
    }
}
