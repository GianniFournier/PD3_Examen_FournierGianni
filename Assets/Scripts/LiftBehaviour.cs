using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftBehaviour : MonoBehaviour {

    [SerializeField]
    private CharacterControllerBehaviour _charCTRLScript;

    private bool _isButtonPressedAgain;

    private Vector3 _liftUp;

    void Start()
    {

        _isButtonPressedAgain = false;

        _liftUp = new Vector3(50, 0, 0);

    }

    void Update ()
    {

        if (_charCTRLScript._isButtonPressed == true && _isButtonPressedAgain == false)
        {
            LiftOn();
        }

        if (_charCTRLScript._isButtonPressed == true && _isButtonPressedAgain == true)
        {
            LiftOff();
        }


    }

    private void LiftOff()
    {
        Debug.Log("[LIFT] Lift Off");

        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.Rotate(new Vector3(-i, 0, 0));

            if (gameObject.transform.rotation == Quaternion.Euler(Vector3.zero))
            {
                _isButtonPressedAgain = false;
                _charCTRLScript._isButtonPressed = false;
            }
        }
    }

    private void LiftOn()
    {
        Debug.Log("[LIFT] Lift On");

        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.Rotate(new Vector3(i,0,0));

            if(gameObject.transform.rotation == Quaternion.Euler(_liftUp))
            {
                _isButtonPressedAgain = true;
                _charCTRLScript._isButtonPressed = false;
            }
        }


            

    }
}
