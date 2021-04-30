using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject theObject;
    public LayerMask layerMask;
    public bool canShoot = false;
    [Header("Muzzle Flash")]
    public Transform muzzleFlashSpot;
    public GameObject muzzleFlashObject;
    Animator animator;
    float maxHitDistance = 2000f;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            //Creates the muzzle flash
            Instantiate(muzzleFlashObject, muzzleFlashSpot);

            //Sets off the animation trigger
            animator.SetTrigger("Shoot");

            //Finds where the bullet lands
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, transform.TransformDirection(Vector3.forward), out hit, maxHitDistance, layerMask))
            {
                Instantiate(theObject, hit.point, Quaternion.LookRotation(hit.normal, Vector3.back));
            }
        }

    }
}
