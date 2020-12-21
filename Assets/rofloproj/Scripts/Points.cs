using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public Transform[] Bonus;
    public Transform GroundLevel;
    public Transform Platfrom;
    public ParticleSystem pointParticles;
    public float MagnetSpeed = 1;
    private bool startMagnet;

    private void OnEnable()
    {
        PlayerMovement.magnetEnabled += StartMagnet;
    }
    private void OnDisable()
    {
        PlayerMovement.magnetEnabled -= StartMagnet;
    }

    private void Start()
    {
        Respawn();
    }
    void Update()
    {
        if (PointManager.Instance.playerDeath.CurrentSizeMultipolier != 0)
        {
            transform.localScale = new Vector3(PointManager.Instance.playerDeath.CurrentSizeMultipolier * 0.3f, PointManager.Instance.playerDeath.CurrentSizeMultipolier * 0.3f, PointManager.Instance.playerDeath.CurrentSizeMultipolier * 0.3f);
        }
        if (transform.position.y <= GroundLevel.position.y)
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        int chance = UnityEngine.Random.Range(0, 30);
        if (chance == 0)
        {
            SpawnBonus();
        }
        transform.position = GetRandomVector();
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
    }

    public void StartMagnet(Transform player)
    {
        CancelInvoke();
        startMagnet = true;
        StartCoroutine(MagnetToPlayer(player));
        Invoke("StopMagnet", PlayerPrefs.GetFloat("MagnetTime"));
    }
    private void StopMagnet()
    {
        startMagnet = false;
    }
    private void SpawnBonus()
    {
        Transform bonus = Instantiate(Bonus[UnityEngine.Random.Range(0, Bonus.Length)], GetRandomVector(), Quaternion.identity);
        bonus.localScale = new Vector3(PointManager.Instance.playerDeath.CurrentSizeMultipolier * 0.6f, PointManager.Instance.playerDeath.CurrentSizeMultipolier * 0.6f, PointManager.Instance.playerDeath.CurrentSizeMultipolier * 0.6f);
    }
    private Vector3 GetRandomVector()
    {
        float randomX = UnityEngine.Random.Range(-3.1f * Platfrom.transform.localScale.x, 3.1f * Platfrom.transform.localScale.x);
        float randomY = UnityEngine.Random.Range(10 * Platfrom.transform.localScale.x, 20 * Platfrom.transform.localScale.x);
        float randomZ = UnityEngine.Random.Range(-3.1f * Platfrom.transform.localScale.x, 3.1f * Platfrom.transform.localScale.x);
        return new Vector3(randomX, randomY, randomZ);
    }

    private IEnumerator MagnetToPlayer(Transform player)
    {
      //  GetComponent<Rigidbody>().isKinematic = true;
        while (startMagnet)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, player.position, MagnetSpeed * (Time.deltaTime / distance)* player.transform.localScale.x);
            yield return null;
        }
       // GetComponent<Rigidbody>().isKinematic = false;
    }
    public void OnCollisionEnter(Collision collide)
    {
        if (collide.gameObject.tag == "Player")
        {
            Instantiate(pointParticles, transform.position, Quaternion.identity);
            PointManager.Instance.AddScore();
            Respawn();
        }
        if (collide.gameObject.tag == "Enemy")
        {
            Respawn();
        }

        if (collide.gameObject.tag == "Death")
        {
            Respawn();
        }
    }
}