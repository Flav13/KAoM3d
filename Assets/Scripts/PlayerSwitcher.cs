using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSwitcher : MonoBehaviour
{

	public GameObject mike, flav;

	int whichAvatarIsOn = 1;


	void Start()
	{
		mike.gameObject.SetActive(true);
		flav.gameObject.SetActive(false);
	}

	public void SwitchCharacter(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			switch (whichAvatarIsOn)
			{

				// if the first avatar is on
				case 1:

					// then the second avatar is on now
					whichAvatarIsOn = 2;

					// disable the first one and anable the second one
					mike.gameObject.SetActive(false);
					flav.gameObject.SetActive(true);
					break;

				// if the second avatar is on
				case 2:

					// then the first avatar is on now
					whichAvatarIsOn = 1;

					// disable the second one and anable the first one
					mike.gameObject.SetActive(true);
					flav.gameObject.SetActive(false);
					break;
			}
		}

	}

}


