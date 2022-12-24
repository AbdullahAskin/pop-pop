using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerAnimation animation;

    private void Awake()
    {
        animation = GetComponent<PlayerAnimation>();
    }
}
