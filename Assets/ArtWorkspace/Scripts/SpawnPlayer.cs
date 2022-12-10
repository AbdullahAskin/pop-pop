using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public static bool spawned = true;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        spawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawned)
        {
            Instantiate(player, transform.position, Quaternion.identity);
            spawned = false;
        }
    }
}
