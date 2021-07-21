using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MikeController : PlayerController
{
    public GameObject sticks;

    SticksController sticksController;
    
    public override void Start()
    {
        base.Start();
        sticksController = sticks.GetComponent<SticksController>();

    }
    public void Reset()
    {
        currentCharacter = characterMap["Mike"];
    }

    IEnumerator throwSticks()
    {
        yield return new WaitForSeconds(0.6f);
        sticksController.setPlayerSpeed(rb.velocity);

        if (gamepad.rightStick.IsActuated())
            sticksController.StartSpin(shootLocation);
        else
            sticksController.StartSpin(shootLocationMouse);
        animator.SetBool("is_attacking", false);
    }

    public override void Flip()
    {
        base.Flip();
        sticksController.Flip();

    }

    public override void Attack()
    {
        if (sticksController.getState() == SticksController.State.WithPlayer && shootingInFront())
        {
            StartCoroutine(throwSticks());
            animator.SetBool("is_attacking", true);
        }
        else
        {
            is_crouching = false;
            is_attacking = false;
        }
    }

}
