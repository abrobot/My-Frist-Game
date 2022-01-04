using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float Health = 50f;
    [SerializeField] AudioSource deathSound;

    public void takeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            deathSound.Play();
            Destroy(gameObject);
        }
    }
}