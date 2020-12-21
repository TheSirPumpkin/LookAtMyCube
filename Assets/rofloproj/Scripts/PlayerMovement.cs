using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public delegate void OnMagnetEnabled(Transform player);
    public static OnMagnetEnabled magnetEnabled;

    public ScoreCounter scoreCounter;
    public LookAtPlayer LookAtPlayer;
    public ParticleSystem deathParticles;
    public Rigidbody rb;
    public float forwardForce;
    public float sidewayForce;
    public float deathDelay;
    public Material[] material;
    // Renderer rend;
    public bool alive = true;
    public GameObject joystick;
    public float CurrentSizeMultipolier;
    public bool StoneBreaker;
    public bool Invulnerable;
    private Vector3 startScale;
    void Start()
    {
        PlayerPrefs.SetFloat("MagnetTime", 3f);
        PlayerPrefs.SetFloat("CrushTime", 3f);
        PlayerPrefs.SetFloat("ShieldTime", 3f);
        
        startScale = transform.localScale;
        joystick.SetActive(true);
        //rend = GetComponent<Renderer>();
        //rend.enabled = true;
        //rend.sharedMaterial = material[0];
    }
    private void Update()
    {
        CurrentSizeMultipolier = transform.localScale.x / startScale.x;
        DetectDeath();
    }
    void DetectDeath()
    {
        if (startScale.x > transform.localScale.x)
        {
            alive = false;
            joystick.SetActive(false);
            // rend.sharedMaterial = material[1];
            //rb.useGravity = false;
            StartCoroutine(DeathDelay());
            enabled = false;
            LookAtPlayer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Enemy")
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            PointManager.playerDownscaleDelegate.Invoke(.2f);
            PointManager.Instance.CollectedPoints -= 2;
            DetectDeath();
        }

        if (collide.gameObject.tag == "Death")
        {
            alive = false;
            joystick.SetActive(false);
            // rend.sharedMaterial = material[1];
            //rb.useGravity = false;
            StartCoroutine(DeathDelay());
            enabled = false;
            LookAtPlayer.enabled = false;
        }




    }
    private void OnCollisionExit(Collision collision)
    {
        CancelInvoke();
        Invoke("DropDown", .1f);

    }
    private void OnCollisionEnter(Collision collision)
    {
        CancelInvoke();
        if (collision.gameObject.tag == "Crusher")
        {
            StopCoroutine("CancelCrash");
            StoneBreaker = true;
            Destroy(collision.gameObject);
            StartCoroutine("CancelCrash");
        }

        if (collision.gameObject.tag == "Magnet")
        {
            magnetEnabled.Invoke(transform);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Shield")
        {
            StopCoroutine("CancelShield");
            Destroy(collision.gameObject);
            StartCoroutine("CancelShield");
        }
    }
    private IEnumerator CancelCrash()
    {
        yield return new WaitForSeconds(PlayerPrefs.GetFloat("CrushTime"));
        StoneBreaker = false;
    }
    private IEnumerator CancelShield()
    {
        yield return new WaitForSeconds(PlayerPrefs.GetFloat("ShieldTime"));
        Invulnerable = false;
    }
    void DropDown()
    {
        Debug.Log("DropDown");
        rb.AddForce(-Vector3.up * 10 * transform.localScale.x, ForceMode.Impulse);
    }
    public IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        scoreCounter.scoreValue = 0;
        Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(0);
    }
}