using System;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float screenRateMaxForce;

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
        GameManager.Instance.currentPlayer.animation.TogglePrepForJump(true);
    }
    
    private void OnFingerUp(LeanFinger finger)
    {   
        AimManager.Instance.ToggleIndicators(false);
        GameManager.Instance.currentPlayer.animation.TogglePrepForJump(false);
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        var fingerDeltaPos = finger.ScreenPosition - finger.StartScreenPosition;
        var fingerForceRatio = Mathf.Clamp(fingerDeltaPos.magnitude / (Screen.width * screenRateMaxForce), 0, 1);

        AimManager.Instance.StepAimGuide(fingerDeltaPos.normalized, fingerForceRatio);
        GameManager.Instance.currentPlayer.animation.UpdatePrepForJump(fingerForceRatio);
    }
    
    
}
