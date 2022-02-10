using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.SetActive(false);
        MainManager.Instance.GameOver();
    }
}
