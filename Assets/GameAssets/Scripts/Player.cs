using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [NonSerialized] public new PlayerAnimation animation;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        animation = GetComponent<PlayerAnimation>();
        _rb = GetComponent<Rigidbody2D>();
    }
    
    public void Throw(Vector2 force)
    {
        animation.ToggleSpin(true);
        
        _rb.AddForce(force);
        print(force);
    }
}
