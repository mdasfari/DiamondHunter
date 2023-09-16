using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        MessageTrigger._instance.SetAndShowToolTip(message);
    }

    private void OnMouseExit()
    {
        MessageTrigger._instance.HideToolTip();
    }
}
