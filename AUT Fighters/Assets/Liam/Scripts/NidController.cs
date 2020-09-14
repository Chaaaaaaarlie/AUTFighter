﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Script that handles character controls
public class NidController : CharacterController
{
    public Collider2D myHitbox;
    public Collider2D enemyGroundCol;

    // Start is called before the first frame update
    void Start()
    {
        rb.gravityScale *= 1.25f;
        Time.timeScale = 1f;
        airAttackPerformed = false;
        stats.ResetHp();
        stats.ResetSuperMeter();
        ChangeState(new IdleState());
        //Debug.Log(transform.Find("Hitbox").GetComponent<BoxCollider2D>());
        //Physics2D.IgnoreCollision(transform.Find("Hitbox").GetComponent<BoxCollider2D>(), opponent.transform.Find("GroundCollider").GetComponent<BoxCollider2D>());
        //Physics2D.IgnoreCollision(myHitbox, enemyGroundCol, true);
    }

    // Update is called once per frame
    void Update()
    {
        //DirectionToBeFacing();
        currentState.Execute();
    }

    public override void OnHit(CharacterController opponent)
    {
        //Apply the push back force of an attack
        opponent.rb.AddForce(transform.right * -opponent.direction * opponent.currentAttackData.pushback, ForceMode2D.Impulse); //Maybe shift this into hitstun state, change the argument for the constructor
        rb.AddForce(transform.right * -direction * opponent.currentAttackData.pushforward, ForceMode2D.Impulse);

        if(opponent.currentAttackData.causeKnockdown) //If the attack causes a knockdown
        {
            anim.Play("NidKnockdown");
            if (opponent.currentAttackData.launchForce != new Vector2(0, 0))        //If the attack launches the target, apply the launch force
            {
                rb.AddForce(new Vector2(-direction * opponent.currentAttackData.launchForce.x, 1 * opponent.currentAttackData.launchForce.y), ForceMode2D.Impulse); //Maybe shift this into kncokdown state, change the argument for the constructor
                ChangeState(new LaunchState());
            }
            else
            {
                //Debug.Log(new Vector2(1 * -opponent.direction * opponent.currentAttackData.launchForce.x, 1 * opponent.currentAttackData.launchForce.y));
                ChangeState(new KnockdownState());
            }
        }
        else //The attack does not cause a knockdown
        {
            anim.SetBool("InHitStun", true);
            ChangeState(new HitStunState(opponent));
            if (inputs.crouch.ReadValue<float>() != 0)
            {
                Debug.Log("Play crouch hit stun");
                //anim.Play("NidCrouchHit", 0, 0);
                anim.Play("NidCrouchHit", 0, 0);
            }
            else
            {
                Debug.Log("Play stand hit stun");
                anim.Play("NidStandHit", 0, 0);
            }
        }

        stats.TakeDamage(opponent.currentAttackData.damage);
        stats.GainMeter(opponent.currentAttackData.damage * 0.3f);
    }

    public override void OnBlock(CharacterController opponent)
    {
        anim.SetBool("InBlockStun", true);

        opponent.rb.AddForce(transform.right * -opponent.direction * opponent.currentAttackData.pushback, ForceMode2D.Impulse); //Maybe shift this into blockstun state, change the argument for the constructor
        rb.AddForce(transform.right * -direction * opponent.currentAttackData.pushforward, ForceMode2D.Impulse);
        ChangeState(new BlockStunState(opponent));

        if (inputs.crouch.ReadValue<float>() != 0)
        {
            anim.Play("NidCrouchBlocking", 0, 0);
        }
        else
        {
            anim.Play("NidStandBlocking", 0, 0);
        }

        stats.TakeDamage(opponent.currentAttackData.damage * 0.2f);
        stats.GainMeter(opponent.currentAttackData.damage * 0.2f);
    }

    public override void OnThrown(CharacterController opponent)
    {
        ChangeState(new ThrownState(opponent));
        anim.SetBool("IsThrown", true);
        anim.Play("NidGetThrown");
        stats.GainMeter(opponent.currentAttackData.damage * 0.3f);
    }

}
