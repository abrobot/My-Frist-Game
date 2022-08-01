using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletInfo bulletInfo;
    public GameObject bulletGameObject;
    bool shot = false;
    float Spawned;

    public Rigidbody bulletRigidbody;
    void Start()
    {
        Spawned = Time.time;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.otherCollider);
            Enemy enemy = contact.otherCollider.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.takeDamage(bulletInfo.damage, bulletInfo.player);

            }

            // print(!this.gameObject.transform.IsChildOf(bulletInfo.player.transform));
            // print(this.gameObject != bulletInfo.player);

            if (contact.otherCollider.gameObject != bulletInfo.player && !contact.otherCollider.transform.IsChildOf(bulletInfo.player.transform))
            {
                print(contact.otherCollider.transform.name);
                GetComponent<ParticleSystem>().Play();
                bulletGameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shot == false && bulletInfo.forword != null && bulletInfo.speed != 0)
        {
            bulletRigidbody.AddForce(bulletInfo.forword * bulletInfo.speed, ForceMode.Impulse);
            bulletRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            shot = true;
        }


        if (Time.time >= Spawned + bulletInfo.lifetime)
        {
            Destroy(this.gameObject);
        }
        
        if (bulletRigidbody.useGravity) bulletRigidbody.AddForce((Physics.gravity * 5) * Mathf.Clamp(Time.time - Spawned, 0, 1));

    }
}


public struct BulletInfo
{
    public GameObject player;
    public Vector3 forword;
    public int speed;
    public int damage;
    public int lifetime;

    public BulletInfo(Vector3 forword, int speed, int damage, GameObject player, int lifetime) : this()
    {
        this.forword = forword;
        this.speed = speed;
        this.damage = damage;
        this.player = player;
        this.lifetime = lifetime;
    }

    public BulletInfo(BulletInfo bulletInfo, Vector3 forword) : this()
    {
        this.forword = forword;
        this.speed = bulletInfo.speed;
        this.damage = bulletInfo.damage;
        this.player = bulletInfo.player;
        this.lifetime = bulletInfo.lifetime;
    }
}
