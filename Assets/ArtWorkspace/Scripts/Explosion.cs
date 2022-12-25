using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    private CameraShake cameraShake;
    public GameObject particle;
    public GameObject smoke;
    public GameObject boneSpawner;

    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    public float animDuration;

    private Animator myAnimator;

    public Button _button; 


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = transform.GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || CameraShake._btnEx)
        {
            myAnimator.SetBool("explosion", true);
            CameraShake._btnEx = false;
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
            if(obj.tag == "enemy")
            {
                Instantiate(boneSpawner, obj.transform.position, Quaternion.identity);
                Destroy(obj.gameObject);
            }
            else
            {
                Vector2 direction = obj.transform.position - transform.position;
                obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
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
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
}
