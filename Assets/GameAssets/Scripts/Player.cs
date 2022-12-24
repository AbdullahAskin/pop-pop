using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [NonSerialized] public PlayerAnimation Animation;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        Animation = GetComponent<PlayerAnimation>();

        _rb = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 force)
    {
        Animation.TriggerSpin();
        
        _rb.AddForce(force);
    }
}
