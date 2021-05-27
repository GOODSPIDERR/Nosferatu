using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int hp = 3;
    public GameObject deadObject;
    void Start()
    {

    }

    void Update()
    {

    }

    public void TakeDamage()
    {
        hp--;
        if (hp <= 0)
        {
            Instantiate(deadObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
