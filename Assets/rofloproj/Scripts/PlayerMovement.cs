using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using MoreMountains.NiceVibrations;

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
        Physics.gravity = new Vector3(0, transform.localScale.y * -9.81f, 0);
        RenderSettings.fogStartDistance = transform.position.y;
        RenderSettings.fogEndDistance = RenderSettings.fogStartDistance * 25f;
        RenderSettings.fogColor = GrowableObject.Instance.CurrentColor;
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Social.ReportScore(PlayerPrefs.GetInt("BestScore"), "CgkI-ZOI2vYUEAIQAQ", (bool success) =>
            {
            });
        }

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
    void DetectDeath(bool fall = false)
    {
       
        if (startScale.x > transform.localScale.x || fall)
        {
            MMVibrationManager.Haptic(HapticTypes.Failure);
            alive = false;
            joystick.SetActive(false);
            // rend.sharedMaterial = material[1];
            //rb.useGravity = false;
            enabled = false;
            LookAtPlayer.enabled = false;
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                Social.ReportScore(PointManager.Instance.bestscore.scoreBestValue, "CgkI-ZOI2vYUEAIQAQ", (bool success) =>
                {
                    SetScoreBeforeDeath();
                });
            }
            else
            {
                SetScoreBeforeDeath();
            }
        }
    }
    private void SetScoreBeforeDeath()
    {
        if (PlayerPrefs.GetInt("BestScore") < PointManager.Instance.bestscore.scoreBestValue)
        {
            PlayerPrefs.SetInt("BestScore", PointManager.Instance.bestscore.scoreBestValue);
        }
        CancelInvoke();
        Invoke("DeathDelay", deathDelay);
    }

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject.tag == "Enemy")
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            PointManager.playerDownscaleDelegate.Invoke(.2f);
            PointManager.Instance.CollectedPoints -= 2;
            DetectDeath();
        }

        if (collide.gameObject.tag == "Death")
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            alive = false;
            joystick.SetActive(false);
            // rend.sharedMaterial = material[1];
            //rb.useGravity = false;
            DetectDeath(true);
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
            MMVibrationManager.Haptic(HapticTypes.Selection);
            StopCoroutine("CancleCrush");
            StoneBreaker = true;
            Destroy(collision.gameObject);
            StartCoroutine("CancleCrush");
        }

        if (collision.gameObject.tag == "Magnet")
        {
            MMVibrationManager.Haptic(HapticTypes.Selection);
            magnetEnabled.Invoke(transform);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Shield")
        {
            MMVibrationManager.Haptic(HapticTypes.Selection);
            StopCoroutine("CancelShield");
            Destroy(collision.gameObject);
            StartCoroutine("CancelShield");
        }
    }
    private IEnumerator CancleCrush()
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
    public void DeathDelay()
    {
        // yield return new WaitForSeconds(deathDelay);
        scoreCounter.scoreValue = 0;
        Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(0);
    }
}