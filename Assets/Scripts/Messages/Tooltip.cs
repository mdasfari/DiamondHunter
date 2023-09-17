using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour

{
    public string message;
    public float detectionRadius = 0.5f; // Adjust as needed
    private bool isPlayerNear = false;

    private void Update()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player"));
        if (playerCollider && !isPlayerNear)
        {
            isPlayerNear = true;
            MessageTrigger._instance.SetAndShowToolTip(message);
        }
        else if (!playerCollider && isPlayerNear)
        {
            isPlayerNear = false;
            MessageTrigger._instance.HideToolTip();
        }
    }
}

