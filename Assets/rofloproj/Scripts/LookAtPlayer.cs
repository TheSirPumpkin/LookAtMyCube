using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform Target;
    public Transform Player;
    public float AdjustX;
    public float AdjustY;
    public float AdjustZ;
    public float SmoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
        PlayerPrefs.SetInt("CleaeredLevels", 1);
    }
    private void Update()
    {
        // Debug.Log(PlayerPrefs.GetInt("CleaeredLevels"));
        //  RenderSettings.fogStartDistance=transform.position.y;
        //  RenderSettings.fogEndDistance = ((transform.position.y + GrowableObject.Instance.TransfromToGrow.localScale.y) * PlayerPrefs.GetInt("CleaeredLevels"))+150f;
        StartCoroutine(AdjustHeight());
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Player.position.x, Player.localScale.y * AdjustY, Player.position.z + (Player.localScale.z * AdjustZ)), ref velocity, SmoothTime);
    }
    IEnumerator AdjustHeight()
    {
        while (true)
        {
            cam.farClipPlane = Mathf.Abs(transform.position.y * 25);
            QualitySettings.shadowDistance= transform.position.y *2;
            yield return new WaitForSeconds(1f);
        }
    }
}