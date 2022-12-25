using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [NonSerialized] public new PlayerAnimation animation;

    private void Awake()
    {
        animation = GetComponent<PlayerAnimation>();
    }
    
    public void Spin()
    {
        animation.ToggleSpin(true);
        
        //throw
    }
}
