using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachLevelController : MonoBehaviour
{
    [Header("Intro")]

    [Header("Animation Settings")]
    public Transform shipObject;
    public Transform playerObject;
    public float SailSpeed = 20f;

    public Vector2 shipEndPosition = new Vector2(20.07f, 0.14f);
    public Vector2 playerEndPosition = new Vector2(-9.7f, -3.237f);

    private FiniteStateMachine stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        IntroState introState = new IntroState(this);
        stateMachine = new FiniteStateMachine(introState);
    }

    // Update is called once per frame
    private void Update()
    {
        stateMachine.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}
