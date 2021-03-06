﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : ICharacterState
{
    private CharacterController character;

    public void Enter(CharacterController controller)
    {
        character = controller;
        character.throwHurtbox.enabled = false;
        character.anim.SetBool("IsJumping", true);
        //character.ToggleGroundCollider();
        //character.Jump();
        Debug.Log("Entering Jump State");
    }

    public void Execute()
    {
        if(character.IsLanding() && character.rb.velocity.y <= 0)
        {
            character.ChangeState(new IdleState());
        }

        if(!character.airAttackPerformed)
        {
            character.HandleAttackPress();
        }

        Debug.Log("Velocity " + character.rb.velocity.normalized);
        Debug.DrawRay(character.transform.position, character.rb.velocity.normalized);
    }

    public void Exit()
    {
        Debug.Log("Exiting Jump State");
        character.anim.SetBool("IsJumping", false);
       // character.anim.SetFloat("MoveX", 0);
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            AttackData atk = other.GetComponent<AttackData>();
            if (character.IsBlocking(atk))
            {
                character.OnBlock(atk);
            }
            else
            {
                character.OnHit(atk);
            }
        }
    }
}
