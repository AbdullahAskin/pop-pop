using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Player currentPlayer;

    void Awake()
    {
        currentPlayer = FindObjectOfType<Player>();
    }

    private GameManager()
    {
        Instance = this;
    }

    public static GameManager Instance { get; private set; }
}
