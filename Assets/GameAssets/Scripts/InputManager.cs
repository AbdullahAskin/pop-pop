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

// Bind or unbind the touch input event handlers.
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
    // Show the aim indicators.
    AimManager.Instance.ToggleIndicators(true);
    // Play the "prep for jump" animation.
    GameManager.Instance.CurrentPlayer.Animation.TogglePrepForJump(true);
}

private void OnFingerUp(LeanFinger finger)
{   
    AimManager.Instance.ToggleIndicators(false);
    GameManager.Instance.CurrentPlayer.Animation.TogglePrepForJump(false);

    // If the aim indicators are active (i.e. the player is aiming to throw), throw the player.
    if (AimManager.Instance.Active)
    {
        // Calculate the force to apply to the throw based on the finger's movement.
        var fingerDir = (finger.ScreenPosition - finger.StartScreenPosition).normalized;
        var force = GetFingerForceRatio(finger) * -fingerDir;
        GameManager.Instance.CurrentPlayer.Throw(force);
    }
}

private void OnFingerUpdate(LeanFinger finger)
{
    // Update the aim indicators with the finger's movement.
    var fingerDir = (finger.ScreenPosition - finger.StartScreenPosition).normalized;
    var fingerForceRatio = GetFingerForceRatio(finger);
    AimManager.Instance.StepAimGuide(fingerDir, fingerForceRatio);
    
    // Update the "prep for jump" animation with the finger's movement.
    GameManager.Instance.CurrentPlayer.Animation.UpdatePrepForJump(fingerForceRatio);
}

// Calculate the ratio of the finger's movement to the maximum force that can be applied.
private float GetFingerForceRatio(LeanFinger finger)
{
    var fingerDeltaPos = finger.ScreenPosition - finger.StartScreenPosition;
    return Mathf.Clamp(fingerDeltaPos.magnitude / (Screen.width * screenRateMaxForce), 0, 1);
}
}
