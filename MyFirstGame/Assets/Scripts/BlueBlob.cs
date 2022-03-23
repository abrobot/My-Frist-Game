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
        behaviorActive = true;
        rigidBody.isKinematic = true;

        float nextJumpTime = Random.Range(10, 20);
        float lastJumpTime = 0f;



        while (alive == true)
        {
            if (Time.time >= lastJumpTime + 2.5f)
            {
                navMeshAgent.enabled = true;
                rigidBody.isKinematic = true;
                rigidBody.useGravity = false;
            }
            MoveTo(target.transform.position);

            if (Time.time >= lastJumpTime + nextJumpTime)
            {
                Lunge();
                lastJumpTime = Time.time;
                nextJumpTime = Random.Range(5, 10);
            }

            yield return new WaitForSeconds(.1f);
        }
        behaviorActive = false;
    }


    public void Lunge()
    {
        if (gameObject)
        {
            navMeshAgent.enabled = false;
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
            rigidBody.velocity += transform.forward * 60;
            rigidBody.velocity += transform.up * 10;

        }
    }
}


