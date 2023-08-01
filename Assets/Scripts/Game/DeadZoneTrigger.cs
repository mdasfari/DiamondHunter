﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0f;
        }
    }
        
}
