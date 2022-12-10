using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    public CameraShake cameraShake;
    public GameObject particle;
    public GameObject smoke;

    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    public float animDuration;

    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            myAnimator.SetBool("explosion", true);
            StartCoroutine(wait());
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
            if(obj.tag == "box")
            {
                //Box explosion
            }
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

    IEnumerator wait()
    {
        yield return new WaitForSeconds(animDuration);
        SpawnPlayer.spawned = true;
        ExplosionField();
        Instantiate(particle, transform.position, Quaternion.identity);
        Instantiate(smoke, transform.position, Quaternion.identity);
        StartCoroutine(cameraShake.Shake(.1f, 0.1f));
        Destroy(gameObject);
    }
}
