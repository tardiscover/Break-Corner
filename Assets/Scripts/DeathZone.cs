using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private MainManager mainManager;

    protected virtual void Start()
    {
        mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.SetActive(false);
        mainManager.GameOver();
    }
}
