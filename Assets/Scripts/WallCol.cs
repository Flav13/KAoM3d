using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCol : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Stick" && col.gameObject.GetComponent<SticksController>().getState() != SticksController.State.WithPlayer)
        {
            col.gameObject.GetComponent<SticksController>().setState(SticksController.State.Recalling);
        }
    }
}
