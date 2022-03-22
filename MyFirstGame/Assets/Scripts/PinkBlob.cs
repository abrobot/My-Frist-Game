using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBlob : Enemy
{
    public PinkBlob(int health, int killScoreValue, AudioSource deathSound)
    {
        this.health = health;
        this.deathSound = deathSound;
        this.killScoreValue = killScoreValue;
;       

    }



    public override IEnumerator ActivateBehavior() {
        
        behaviorActive = true; 
        while (behaviorActive == true) {
            MoveTo(target.transform.position);
            yield return new WaitForSeconds(.1f);
        }
        behaviorActive = false;
    }
}