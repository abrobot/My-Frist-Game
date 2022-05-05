using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlob : Enemy
{
    public BlueBlob(int health, int killScoreValue, AudioSource deathSound)
    {
        this.health = health;
        this.deathSound = deathSound;
        this.killScoreValue = killScoreValue;
    }


   


    public override IEnumerator ActivateBehavior()
    {

        //print("Start");
        behaviorActive = true;

        float lastJumpTime = Time.time;
        float nextJumpTime = Random.Range(5, 6);


        bool midJump = false;

        navMeshAgent.enabled = false;
        navMeshAgent.enabled = true;

        while (alive == true && target.enabled ==  true)
        {

            if (Time.time >= lastJumpTime + 2.5f && midJump)
            {
                navMeshAgent.enabled = true;
                lastJumpTime = Time.time;
                nextJumpTime = Random.Range(5, 6);

                midJump = false;
            }

            MoveTo(target.position);

            if (Time.time >= lastJumpTime + nextJumpTime && !midJump)
            {
                midJump = true;
                Lunge();

            }
            yield return new WaitForSeconds(0.1f);
        }

        StopMove();
        navMeshAgent.enabled = true;
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
       
        behaviorActive = false;
        behaviorCoroutine = null;
    }


    public void Lunge()
    {
        if (gameObject)
        {   
            StopMove();
            navMeshAgent.enabled = false;
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
            rigidBody.velocity += transform.forward * 60;
            rigidBody.velocity += transform.up * 10;
        }
    }
}


