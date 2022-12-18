using System;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Start()
    {
        ToggleInput(true);
    }

    public void ToggleInput(bool bind)
    {
        if (bind)
        {
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUp += OnFingerUp;
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
        }
        else
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        }
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
       
        AimManager.Instance.ToggleIndicators(true);
    }
    
    private void OnFingerUp(LeanFinger finger)
    {   
        AimManager.Instance.ToggleIndicators(false);
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        AimManager.Instance.StepAimGuide(finger);
    }
    
    
}
