using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossIddleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0f; //How much time the boss will state

    Transform player;

    public float dectectionAreaRadius = 18f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //--Transisiton to Walk State-- //
        timer += Time.deltaTime;
        if (timer > idleTime)
        {
            animator.SetBool("isWalking", true);
        }

        //--Transisiton to Chase State-- //
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < dectectionAreaRadius)
        {
            animator.SetBool("isChasing", true );
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
