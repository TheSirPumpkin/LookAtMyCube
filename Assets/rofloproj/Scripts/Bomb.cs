using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : MonoBehaviour
{
    public Transform Platfrom;
    private void Start()
    {
        Respawn();
    }

    void Update()
    {
        transform.localScale = Platfrom.transform.localScale * 0.3f;
        if (transform.position.y <= 0)
            Respawn();
    }

    public void Respawn()
    {

        

        float randomX = UnityEngine.Random.Range(-3.1f * Platfrom.transform.localScale.x, 3.1f * Platfrom.transform.localScale.x);
        float randomY = UnityEngine.Random.Range(10 * Platfrom.transform.localScale.x, 20 * Platfrom.transform.localScale.x);
        float randomZ = UnityEngine.Random.Range(-3.1f * Platfrom.transform.localScale.x, 3.1f * Platfrom.transform.localScale.x);
        transform.position = new Vector3(randomX, randomY, randomZ);

        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
    }

    public void OnCollisionEnter(Collision ground)
    {
        if (ground.gameObject.tag == "Ground")
        {
            Respawn();
        }
        if (ground.gameObject.tag == "Death")
        {
            Respawn();
        }

        if (ground.gameObject.tag == "Player")
        {
            Respawn();
        }
    }
}