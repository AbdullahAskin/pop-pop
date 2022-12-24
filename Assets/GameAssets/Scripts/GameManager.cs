using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public Player CurrentPlayer;
    
    public float minForce;
    public float maxForce;

    void Awake()
    {
        CurrentPlayer = FindObjectOfType<Player>();
    }

    private GameManager()
    {
        Instance = this;
    }

    public static GameManager Instance { get; private set; }
}
