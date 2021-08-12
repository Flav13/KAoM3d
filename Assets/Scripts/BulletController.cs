using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector2 shootDirection;

    public Rigidbody rb;
    float shootSpeed = 20f;


    void Update()
    {

        rb.velocity = shootDirection * shootSpeed;

      //  float hitDetectionSize = 3f;
       // Enemy enemy = enemy.GetClosest(transform.position, hitDetectiomSize);
      //  if (enemy != null)
     //   {
     //       enemy.TakeDamage(10);
     //       Destroy(gameObject);
     //   }
    }

    public void setDirection(Vector2 shootDir)
    {
        shootDirection = shootDir;
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
