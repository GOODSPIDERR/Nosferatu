using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootScript : MonoBehaviour
{
    Transform mainCamera;
    public GameObject impactVFX, weapon1, weapon2;
    public LayerMask layerMask;
    public bool canShoot = false, canSwitch = false;
    public int currentWeapon = 1;
    public AudioSource shotSound;

    [Header("Screenshake Options")]
    public float shakeDuration = 0f;
    public Vector3 shakeStrength = new Vector3(0, 0, 0);
    public int shakeFrequency = 0;
    public float shakeRandomness = 0f;
    [Header("Muzzle Flash")]
    public Transform muzzleFlashSpot;
    public GameObject muzzleFlashObject;
    Animator animator;
    float maxHitDistance = 2000f;
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canShoot)
        {
            if (Input.GetButton("Fire1"))
            {
                //Camera shake
                mainCamera.localPosition = new Vector3(0, 0.891f, 0);
                Sequence shakeSequence = DOTween.Sequence();
                shakeSequence.Append(mainCamera.DOShakePosition(shakeDuration, shakeStrength, shakeFrequency, shakeRandomness, false, true));
                shakeSequence.Append(mainCamera.DOLocalMove(new Vector3(0, 0.891f, 0), 0.4f));
                shakeSequence.Play();

                //Creates the muzzle flash
                Instantiate(muzzleFlashObject, muzzleFlashSpot);

                //Plays the shooting sound and randomizes the pitch slightly
                shotSound.pitch = Random.Range(0.8f, 1.2f);
                shotSound.Play();

                //Sets off the animation trigger
                animator.SetTrigger("Shoot");

                //Finds where the bullet lands
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.position, mainCamera.TransformDirection(Vector3.forward), out hit, maxHitDistance, layerMask))
                {
                    //Creates the burst VFX
                    GameObject vfx = Instantiate(impactVFX, hit.point, Quaternion.LookRotation(hit.normal, Vector3.back));
                    vfx.transform.parent = hit.transform;

                    //If it hits an enemy, do damage with its script
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
