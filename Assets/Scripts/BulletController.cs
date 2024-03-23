using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    [SerializeField] private float SurviveTime = 5f;

    private float currentSurviveTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if (currentSurviveTime > SurviveTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }
}
