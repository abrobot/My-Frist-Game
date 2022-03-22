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
        rigidbody.isKinematic = true;

        float nextJumpTime = Random.Range(10, 20);
        float lastJumpTime = 0f;



        while (behaviorActive == true)
        {
            if (this != null)
            {
                if (Time.time >= lastJumpTime + 2.5f)
                {
                    navMeshAgent.enabled = true;
                    rigidbody.isKinematic = true;
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
        }
        behaviorActive = false;
    }


    public void Lunge()
    {
        if (gameObject)
        {
            print("Lunge");
            navMeshAgent.enabled = false;
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.velocity += transform.forward * 50;
            rigidbody.velocity += transform.up * 10;

        }
    }

    void FixedUpdate()
    {
        // if (navMeshAgent.enabled == false)
        // {
        //     rigidbody.velocity += transform.up * 1000;
        //     print(rigidbody.velocity);
        // }
    }
}


