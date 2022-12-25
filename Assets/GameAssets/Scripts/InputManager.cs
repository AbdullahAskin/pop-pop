using System;
using Lean.Touch;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float minInputRateForAimAssistance;
    [SerializeField] private float maximumForceCap;

    [Header("Force Limits")]
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

    private Vector2 _force;
    private bool _active; 

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
        GameManager.Instance.currentPlayer.animation.TogglePrepForJump(false);

        if (_active)
        {
            GameManager.Instance.currentPlayer.Throw(_force);   
        }
    }
    
    private void OnFingerUpdate(LeanFinger finger)
    {
        var fingerDeltaPos = finger.ScreenPosition - finger.StartScreenPosition;
        var inputRate = Mathf.Clamp(fingerDeltaPos.magnitude / (Screen.width * maximumForceCap), 0, 1);

        _active = inputRate >= minInputRateForAimAssistance;
        if (!_active) return;

        var normalizedInputRate = (inputRate - minInputRateForAimAssistance) / (1f - minInputRateForAimAssistance);
        var forceMagnitude = minForce + (maxForce - minForce) * normalizedInputRate;
        _force = -fingerDeltaPos.normalized * forceMagnitude;

        AimManager.Instance.StepAimGuide(_force, normalizedInputRate);
        GameManager.Instance.currentPlayer.animation.UpdatePrepForJump(normalizedInputRate);
    }
}
