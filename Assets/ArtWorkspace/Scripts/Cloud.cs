using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x < -10f || transform.position.x > 10f) // obje ekranın sol veya sağ tarafından dışına çıktıysa
        {
            Destroy(gameObject); // obje destroy et
        }
    }
}