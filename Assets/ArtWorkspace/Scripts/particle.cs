using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class particle : MonoBehaviour
{

    Rigidbody2D rb;
    SpriteRenderer sr;
    float randomX;
    float randomY;
    float randomScale;
    public float minRandom;
    public float maxRandom;
    public Color[] particleColors;
    float alpha;
    int randomColorIndex;

    public float minXForce, maxXForce, minYForce, maxYForce, torqueForce;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 1;
        float rndFade = Random.Range(1f, 2f);
        Invoke("fadeOut", rndFade);
        randomScale = Random.Range(minRandom, maxRandom);
        transform.localScale = new Vector3(randomScale,randomScale,randomScale);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.AddTorque(torqueForce);
        randomX = Random.Range(minXForce, maxXForce);
        randomY = Random.Range(minYForce, maxYForce);
        Throw();
        randomColorIndex = Random.Range(0, 3);
        sr.color = particleColors[randomColorIndex];
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,alpha);
    }

    void Throw()
    {
        rb.velocity = new Vector2(randomX, randomY);
    }
    void fadeOut()
    {
        //DOTween.To(x => alpha = x, alpha, 0 ,0.2f);
    }
    
}
