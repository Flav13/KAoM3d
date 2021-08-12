using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;


    public void Remove()
    {
        Destroy(gameObject);
    }


}
