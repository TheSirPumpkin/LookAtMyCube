﻿using System.Collections;
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
    private void Update()
    {
        RenderSettings.fogStartDistance=transform.position.y;
        RenderSettings.fogEndDistance = transform.position.y + 200f;
    }
    private void FixedUpdate()
    {
        // transform.position = new Vector3(Player.position.x, transform.position.y, transform.position.z);
       
         transform.position = Vector3.SmoothDamp(transform.position, new Vector3(Player.position.x, Player.localScale.y* AdjustY, Player.position.z+ (Player.localScale.z*AdjustZ)), ref velocity, SmoothTime);
        // transform.LookAt(Target.position);
    }

}