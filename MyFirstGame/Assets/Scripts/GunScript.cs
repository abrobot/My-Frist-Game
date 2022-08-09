// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GunScript : MonoBehaviour
// {
//     [SerializeField] Transform bulletOriginPoint;
//     [SerializeField] Camera playerCamera;
//     [SerializeField] LayerMask layerMask;

//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
//         // if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, ~layerMask)) {
//         //     Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward, new Color(343, 3, 54), 5);
//         Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
//         Debug.DrawLine (playerCamera.transform.position, playerCamera.transform.forward * 50000000, Color.red);
//         // }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject owner;
    public Camera playerCamera;
    public PlayerStatus playerStatus;

    public Camera ownerCamera;
    [SerializeField] LayerMask layerMask;
    [SerializeField] AudioSource gunshot;
    public int damage;

    public GameObject bullet;
    BulletInfo bulletInfo;

    List<GameObject> bullets = new List<GameObject>();


    void Start()
    {
        bulletInfo = new BulletInfo(default, 15, damage, owner, 3, playerStatus);
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        GameObject newBulletObject = Instantiate(bullet, owner.transform.position + (owner.transform.forward * 1), default);
        Bullet NewBullet = newBulletObject.GetComponent<Bullet>();
        NewBullet.bulletInfo = new BulletInfo(bulletInfo, ownerCamera.transform.forward);
        NewBullet.playerCamera = playerCamera;

        gunshot.pitch = Random.Range(0.8f, 1f);
        gunshot.time = 2.5f;
        gunshot.Play();
    }
}