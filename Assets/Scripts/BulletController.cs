using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector3 shootDir;

    public Rigidbody rb;
    float moveSpeed = 0.1f;


    public void Setup(Vector2 shootLocation)
    {
        Camera mainCamera = GameObject.FindObjectOfType<Camera>();

        Vector3 shootDirection = mainCamera.ScreenToWorldPoint(new Vector3(shootLocation.x, shootLocation.y, transform.position.z));
        shootDirection.z = transform.position.z;
        Debug.Log(shootDirection);
        this.shootDir = (shootDirection - transform.position).normalized;
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        rb.AddForce(shootDir* moveSpeed, ForceMode.Impulse);

      //  float hitDetectionSize = 3f;
       // Enemy enemy = enemy.GetClosest(transform.position, hitDetectiomSize);
      //  if (enemy != null)
     //   {
     //       enemy.TakeDamage(10);
     //       Destroy(gameObject);
     //   }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
