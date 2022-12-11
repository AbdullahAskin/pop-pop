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
        PlayerAimManager.Instance.ToggleIndicators(true);
    }
    
    private void OnFingerUp(LeanFinger finger)
    {   
        PlayerAimManager.Instance.ToggleIndicators(false);
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        PlayerAimManager.Instance.StepAimGuide(finger);
    }
    
    
}
