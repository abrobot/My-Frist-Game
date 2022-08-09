using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletInfo bulletInfo;
    public GameObject bulletGameObject;
    public Camera playerCamera;
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
            Enemy enemy = contact.otherCollider.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.takeDamage(bulletInfo.damage, bulletInfo.player);

            }

            // print(!this.gameObject.transform.IsChildOf(bulletInfo.player.transform));
            // print(this.gameObject != bulletInfo.player);

            if (contact.otherCollider.gameObject != bulletInfo.player && !contact.otherCollider.transform.IsChildOf(bulletInfo.player.transform))
            {
                GetComponent<ParticleSystem>().Play();
                bulletGameObject.SetActive(false);
                //Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shot == false && bulletInfo.forword != null && bulletInfo.speed != 0 && playerCamera)
        {

            // float PlayerRelativeVelocity;

            // switch (bulletInfo.playerStatus.moveDirection)
            // {
            //     case MoveDirection.N:
            //         PlayerRelativeVelocity = 0;
            //         break;
            //     case MoveDirection.F:
            //         PlayerRelativeVelocity = bulletInfo.playerStatus.speed;
            //         break;
            //     case MoveDirection.B:
            //         PlayerRelativeVelocity = -bulletInfo.playerStatus.speed;
            //         break;
            //     case MoveDirection.FR:
            //         PlayerRelativeVelocity = bulletInfo.playerStatus.speed / 2;
            //         break;
            //     case MoveDirection.FL:
            //         PlayerRelativeVelocity = bulletInfo.playerStatus.speed / 2;
            //         break;
            //     case MoveDirection.BR:
            //         PlayerRelativeVelocity = -bulletInfo.playerStatus.speed / 2;
            //         break;
            //     case MoveDirection.BL:
            //         PlayerRelativeVelocity = -bulletInfo.playerStatus.speed / 2;
            //         break;
            //     default:
            //         PlayerRelativeVelocity = 0f;
            //         break;
            // }

            Vector3 playerV = bulletInfo.playerStatus.characterController.velocity;


            bulletRigidbody.AddForce((bulletInfo.forword * bulletInfo.speed) + playerV, ForceMode.Impulse);
            bulletRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            shot = true;
        }



        if (Time.time >= Spawned + bulletInfo.lifetime)
        {
            Destroy(this.gameObject);
        }

        if (bulletRigidbody.useGravity)
        {
            bulletRigidbody.AddForce(((Physics.gravity * 200) * Mathf.Clamp(Time.time - Spawned, 0, 1)) * Time.deltaTime);
        }
    }
}


public struct BulletInfo
{
    public GameObject player;
    public PlayerStatus playerStatus;
    public Vector3 forword;
    public int speed;
    public int damage;
    public int lifetime;

    public BulletInfo(Vector3 forword, int speed, int damage, GameObject player, int lifetime, PlayerStatus playerStatus) : this()
    {
        this.forword = forword;
        this.speed = speed;
        this.damage = damage;
        this.player = player;
        this.lifetime = lifetime;
        this.playerStatus = playerStatus;
    }

    public BulletInfo(BulletInfo bulletInfo, Vector3 forword) : this()
    {
        this.forword = forword;
        this.speed = bulletInfo.speed;
        this.damage = bulletInfo.damage;
        this.player = bulletInfo.player;
        this.lifetime = bulletInfo.lifetime;
        this.playerStatus = bulletInfo.playerStatus;
    }
}
