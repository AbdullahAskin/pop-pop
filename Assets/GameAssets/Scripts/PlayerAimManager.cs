using UnityEngine;

public class PlayerAimManager : MonoBehaviour
{
    private static PlayerAimManager _instance;

    private PlayerAimManager()
    {
        _instance = this;
    }
    
    public static PlayerAimManager GetInstance()
    {
        return _instance;
    }
    
    public void StepAimGuide()
    {
        Debug.Log("StepGuide");
    }
}
