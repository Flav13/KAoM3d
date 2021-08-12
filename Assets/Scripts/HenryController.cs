using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenryController : PlayerController
{
    public GameObject bullet;

    private float fireRate = 0.1f;
    float nextFire = 0.0f;

    public void Reset()
    {
        currentCharacter = characterMap["Henry"];
    }

    public override void Attack()
    {
        if (Time.time >= nextFire)
        {
            nextFire = Time.time + fireRate;
            shootBullet();
        }
    }

    void shootBullet()
    {
        GameObject bTransform = Instantiate(bullet, transform.position, Quaternion.identity);
        bTransform.GetComponent<BulletController>().setDirection(getShootDirection());
        Destroy(bTransform, 5f);
    }
}
