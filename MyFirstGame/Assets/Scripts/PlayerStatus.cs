using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public float health = 100f;
    [SerializeField] public bool alive = true;
    private float lastPlayerHit = 0f;

    public void takeDamage(float amount)
    {
        if (alive)
        {
            health -= amount;
            print("Player health is " + health);
            if (health <= 0f)
            {
                Debug.Log("Player is dead");
                alive = false;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if ((Time.time - lastPlayerHit) > 0.5f)
            {
                takeDamage(10f);
                lastPlayerHit = Time.time;
            }
        }
    }
}
