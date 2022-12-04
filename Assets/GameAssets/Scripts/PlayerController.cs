using System;
using Lean.Touch;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 _touchStartPos;

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
        else LeanTouch.OnFingerUpdate -= OnFingerUpdate;
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        Debug.Log("Finger Down");
        // init guide system
    }
    
    private void OnFingerUp(LeanFinger finger)
    {
        Debug.Log("Finger Up");
        _touchStartPos = finger.ScreenPosition;
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        Debug.Log("Finger Update");
    }
    
    
}
