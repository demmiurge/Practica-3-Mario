using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehavior : StateMachineBehaviour
{
    PlayerMovementWithRigidbody m_MarioController;
    public float m_StartPctTime = 0.3f;
    public float m_EndPctTime = 0.3f;
    public PlayerMovementWithRigidbody.JumpType m_JumpType;
    bool m_JumpActive = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MarioController = animator.GetComponent<PlayerMovementWithRigidbody>();
        m_MarioController.SetJumpActiveType(m_JumpType);
        m_JumpActive = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_JumpActive && stateInfo.normalizedTime > m_StartPctTime && stateInfo.normalizedTime < m_EndPctTime)
        {
            m_MarioController.SetJumpActiveType(m_JumpType);
            m_JumpActive = true;
        } 
        else if(m_JumpActive && stateInfo.normalizedTime > m_EndPctTime)
        {
            m_MarioController.SetJumpActiveType(m_JumpType);
            m_JumpActive = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MarioController.SetJumpActiveType(m_JumpType);
        m_MarioController.SetJumpActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
