using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHelp : MonoBehaviour
{
    public Canvas helpCanvas;
    public Transform showLocation;

    public void OnTriggerEnter2D(Collider2D other)
    {
        helpCanvas.enabled = true;
        //helpCanvas.transform.position = Vector2.up * 100;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        helpCanvas.enabled = false;
        //helpCanvas.transform.position = Vector2.up * -100;
    }

}
