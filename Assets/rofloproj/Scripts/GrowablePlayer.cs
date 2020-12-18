using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowablePlayer : MonoBehaviour
{
    private Vector3 initScale;
    private bool locked;
    private void OnEnable()
    {
        initScale = transform.localScale;
        PointManager.playerGrowDelegate += Grow;
        PointManager.playerDownscaleDelegate += DownScale;
    }
    private void Grow(int multiplier)
    {
        StopAllCoroutines();
       // GrowableObject.Instance.locked = true;
      //  if (!locked) locked = true;
        StartCoroutine(GrowCoroutine(multiplier));
    }

    private void DownScale(float multiplier)
    {
       // if (!locked) locked = true;
        StartCoroutine(DownscaleCoroutine(multiplier));
    }

    private IEnumerator GrowCoroutine(int multiplier)
    {
       
        float endScale = initScale.x + (multiplier / 10f);
        while (transform.localScale.x < endScale)
        {
            transform.localScale += new Vector3(multiplier, multiplier , multiplier) * Time.deltaTime;
            yield return null;
        }
        transform.localScale= new Vector3(endScale,endScale ,endScale);
        initScale = transform.localScale;
       // GrowableObject.Instance.locked = false;
        //   locked = false;
    }

    private IEnumerator DownscaleCoroutine(float multiplier)
    {
        float endScale = initScale.x - (multiplier * PointManager.Instance.scoreCounter.scoreValue);
       
        while (transform.localScale.x > endScale&& transform.localScale.x>0)
        {
            transform.localScale -= new Vector3(multiplier, multiplier, multiplier ) * Time.deltaTime;
            if ((transform.localScale.x <= endScale)) break;
            yield return null;
        }
        initScale = transform.localScale;
       // locked = false;
    }

    private void OnDisable()
    {
        PointManager.playerDownscaleDelegate -= DownScale;
        PointManager.playerGrowDelegate -= Grow;
    }

    private void OnDestroy()
    {
        PointManager.playerDownscaleDelegate -= DownScale;
        PointManager.playerGrowDelegate -= Grow;
    }
}
