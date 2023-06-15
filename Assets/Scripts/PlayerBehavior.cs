using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public int PlayerSpeed = 7;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");

        Debug.Log("hInput: " + hInput);

        transform.position = new Vector3(hInput + Time.deltaTime * PlayerSpeed, 0);
    }
}
