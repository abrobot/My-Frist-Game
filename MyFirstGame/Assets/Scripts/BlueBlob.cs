using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlob : Enemy
{

    public override IEnumerator ActivateBehavior()
    {

        behaviorActive = true;

        //float lastJumpTime = Time.time;
        float nextJumpTime = Random.Range(5, 6);


        bool midJump = false;

        navMeshAgent.enabled = false;
        navMeshAgent.enabled = true;

        float stopLungeAtTime = Time.time;


        while (alive == true && target.enabled == true)
        {

            if (Time.time >= stopLungeAtTime && midJump)
            {
                navMeshAgent.enabled = true;
                nextJumpTime = Random.Range(5, 6);
                midJump = false;
            }

            MoveTo(target.position);

            if (Time.time >= stopLungeAtTime + nextJumpTime && !midJump)
            {
                midJump = true;
                stopLungeAtTime = Lunge();

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


    public float Lunge()
    {
        if (gameObject)
        {
            StopMove();
            navMeshAgent.enabled = false;
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
            rigidBody.velocity += transform.forward * 60;
            rigidBody.velocity += transform.up * 15;

        }
        return Time.time + 2.5f;
    }
}


