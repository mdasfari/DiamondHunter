using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    public RectTransform textToAnimate;

    private Vector3 originalTransform;

    private void Start()
    {
        originalTransform = textToAnimate.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textToAnimate.position = new Vector3(transform.position.x, transform.position.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        textToAnimate.position = originalTransform;
    }

}
