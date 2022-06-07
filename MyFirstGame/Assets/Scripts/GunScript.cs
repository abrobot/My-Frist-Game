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
    public PlayerStatus playerStatus;

    public Camera ownerCamera;
    [SerializeField] LayerMask layerMask;
    [SerializeField] AudioSource gunshot;


    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        Ray ray = ownerCamera.ScreenPointToRay(Input.mousePosition);
        gunshot.pitch = Random.Range(0.95f, 1.05f);
        gunshot.Play();
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, ~layerMask))
        {
            Enemy enemy = raycastHit.transform.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.takeDamage(playerStatus.damage, owner);
            }
        }
    }
}