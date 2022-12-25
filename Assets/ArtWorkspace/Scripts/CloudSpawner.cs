using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // spawn edilecek obje
    public float spawnInterval = 5f; // spawn aralığı
    public float objectSpeed = 5f; // obje hızı

    private float elapsedTime = 0f; // geçen süre

    void Update()
    {
        elapsedTime += Time.deltaTime; // geçen süreyi güncelle

        if (elapsedTime > spawnInterval) // spawn aralığını geçtiyse
        {
            elapsedTime = 0f; // geçen süreyi sıfırla

            int spawnSide = Random.Range(0, 2); // 0 ve 1 arasında random bir değer üret

            Vector2 spawnPosition = Vector2.zero; // spawn pozisyonu

            if (spawnSide == 0) // sol taraf spawn edilecekse
            {
                spawnPosition = new Vector2(-7.5f, Random.Range(-4f, 4f)); // sol taraf spawn pozisyonu
            }
            else // sağ taraf spawn edilecekse
            {
                spawnPosition = new Vector2(7.5f, Random.Range(-4f, 4f)); // sağ taraf spawn pozisyonu
            }

            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity); // obje spawn et

            float objectSpeedRandom = Random.Range(0.1f, objectSpeed);
            if (spawnSide == 0) // sol taraf spawn edildiyse
            {
                spawnedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(objectSpeedRandom, 0f); // sola doğru hareket et
            }
            else // sağ taraf spawn edildiyse
            {
                spawnedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-objectSpeedRandom, 0f); // sağa doğru hareket et
            }
        }
    }
}


