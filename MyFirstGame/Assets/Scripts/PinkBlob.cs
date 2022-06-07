using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBlob : Enemy
{

    public override IEnumerator ActivateBehavior()
    {

        behaviorActive = true;
 
        navMeshAgent.enabled = false;
        navMeshAgent.enabled = true;

        while (alive == true && target.enabled == true)
        {
            MoveTo(target.position);
            yield return new WaitForSeconds(0.1f);
        }

        StopMove();
        behaviorActive = false;
        behaviorCoroutine = null;
    }
}