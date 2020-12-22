using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public delegate void OnPlayerGrowDelegate(int multiplier);
    public static OnPlayerGrowDelegate playerGrowDelegate;
    public delegate void OnObjectGrowDelegate(int multiplier);
    public static OnObjectGrowDelegate objectGrowDelegate;
    public delegate void OnPlayerDwonscaleDelegate(float multiplier);
    public static OnPlayerDwonscaleDelegate playerDownscaleDelegate;
    public delegate void OnChangeColor();
    public static OnChangeColor changeColor;
    [SerializeField]
    public Color[] LevelColors;

    public static PointManager Instance { get; private set; }
    public ScoreCounter scoreCounter;
    public BestScore bestscore;
    public PlayerMovement playerDeath;

    public float timer;
    public bool scoreMultiplier;
    public GameObject multiplierIndicator;
    public GameObject score;
    public GameObject bestscoreobj;
    public int CollectedPoints;
    public BombManager BombManager;
    private int collectToRise;
    private int levelpassed;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        timer = 2f;
        multiplierIndicator.SetActive(false);
        bestscoreobj.SetActive(false);
    }
    void Update()
    {
        if (playerDeath.alive == false)
        {
            bestscoreobj.SetActive(true);
        }
        if (scoreMultiplier == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 2f;
                scoreMultiplier = false;
                multiplierIndicator.SetActive(false);
            }
        }
    }
    public void AddScore()
    {
        if (scoreMultiplier == false)
        {
            CollectedPoints++;
            playerGrowDelegate?.Invoke(1);
            scoreCounter.scoreValue += 1;
        }
        else if (scoreMultiplier == true)
        {
            CollectedPoints++;
            playerGrowDelegate?.Invoke(1);
            scoreCounter.scoreValue += 2;
        }
        if (CollectedPoints > collectToRise)
        {
            collectToRise = CollectedPoints;
            // if (collectToRise % 5 == 0)
            // {
            if (GrowableObject.Instance.TransfromToGrow.localScale.x / playerDeath.transform.localScale.x <= 1.5f)
            {
                objectGrowDelegate?.Invoke((int)(10 * playerDeath.transform.localScale.x));
                levelpassed++;
                if (levelpassed == 5)
                {
                    Random.seed = System.DateTime.Now.Millisecond;
                    GrowableObject.Instance.CurrentColor = LevelColors[Random.Range(0, LevelColors.Length)];
                    changeColor.Invoke();
                    PlayerPrefs.SetInt("CleaeredLevels", PlayerPrefs.GetInt("CleaeredLevels") + 1);
                    levelpassed = 0;
                }
            }
            // }

            if (collectToRise % 30 == 0)
            {
                BombManager.SpawnNewBomb();
            }
        }
        scoreMultiplier = true;
        timer = 2f;
        multiplierIndicator.SetActive(true);
        StartCoroutine(X2Scale());
        StartCoroutine(ScoreScale());

        if (scoreCounter.scoreValue > bestscore.scoreBestValue)
        {
            bestscore.scoreBestValue = scoreCounter.scoreValue;
        }


    }
    public IEnumerator X2Scale()
    {
        multiplierIndicator.transform.localScale = new Vector3(0, 0, 0);
        for (float q = 0f; q < 1f; q += 0.1f)
        {
            multiplierIndicator.transform.localScale = new Vector3(q, q, q);
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator ScoreScale()
    {
        score.transform.localScale = new Vector3(2, 2, 2);
        for (float q = 2f; q > 1f; q -= 0.1f)
        {
            score.transform.localScale = new Vector3(q, q, q);
            yield return new WaitForSeconds(0.01f);
        }
    }
}