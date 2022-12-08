using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    public CameraShake cameraShake;
    public GameObject particle;

    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ExplosionField();
            Instantiate(particle, transform.position, Quaternion.identity);
            StartCoroutine(cameraShake.Shake(.1f, 0.1f));
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            btn_RESTART();
        }
    }

    void ExplosionField()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayerToHit);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldofImpact);
    }

    public void btn_RESTART()
    {
        Scene scene;
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
