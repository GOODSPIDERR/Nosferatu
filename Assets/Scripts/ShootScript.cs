using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject mainCamera, impactVFX, weapon1, weapon2;
    public LayerMask layerMask;
    public bool canShoot = false, canSwitch = false;
    public int currentWeapon = 1;
    public AudioSource shotSound;
    [Header("Muzzle Flash")]
    public Transform muzzleFlashSpot;
    public GameObject muzzleFlashObject;
    Animator animator;
    float maxHitDistance = 2000f;
    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canShoot)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                //Creates the muzzle flash
                Instantiate(muzzleFlashObject, muzzleFlashSpot);

                //Plays the shooting sound and randomizes the pitch slightly
                shotSound.pitch = Random.Range(0.8f, 1.2f);
                shotSound.Play();

                //Sets off the animation trigger
                animator.SetTrigger("Shoot");

                //Finds where the bullet lands
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.transform.position, transform.TransformDirection(Vector3.forward), out hit, maxHitDistance, layerMask))
                {
                    //Creates the burst VFX
                    GameObject vfx = Instantiate(impactVFX, hit.point, Quaternion.LookRotation(hit.normal, Vector3.back));
                    vfx.transform.parent = hit.transform;

                    if (hit.transform.tag == "Enemy")
                    {
                        EnemyScript enemyScript = hit.transform.gameObject.GetComponent<EnemyScript>();
                        enemyScript.TakeDamage();
                    }
                }
            }


        }

        if (canSwitch)
        {
            if (Input.GetButtonDown("Weapon1"))
            {
                weapon1.SetActive(true);
                weapon2.SetActive(false);

            }

            if (Input.GetButtonDown("Weapon2"))
            {
                weapon1.SetActive(false);
                weapon2.SetActive(true);
            }
        }


    }
}
