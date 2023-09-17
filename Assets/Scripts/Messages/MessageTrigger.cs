using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageTrigger : MonoBehaviour
{
    public static MessageTrigger _instance;
    public TextMeshProUGUI textComponent;

    private float angle = 0f;
    private float radius = 50f; // distance from the cursor
    private float speed = 2f; // speed of rotation

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Input.mousePosition.x + radius * Mathf.Cos(angle);
        float y = Input.mousePosition.y + radius * Mathf.Sin(angle);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    public void SetAndShowToolTip(string message)
    {
        gameObject.SetActive(true);
        textComponent.text = message;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }
}

