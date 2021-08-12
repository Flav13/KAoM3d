using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSwitcher : MonoBehaviour
{

	public GameObject mike, flav, henry;
	public GameObject camera; 
	PlayerController mikeController;
	PlayerController flavController;
	PlayerController henryController;


	int whichAvatarIsOn = 1;


	void Start()
	{
		mike.gameObject.SetActive(true);
		mikeController.MakeActive();

		flav.gameObject.SetActive(false);
		henry.gameObject.SetActive(false);
	}

	void Awake()
    {
		mikeController = mike.GetComponent<MikeController>();
		flavController = flav.GetComponent<FlavController>();
		henryController = henry.GetComponent<HenryController>();
	}

	public void SwitchCharacter(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			switch (whichAvatarIsOn)
			{

				case 1:
					// then the second avatar is on now
					whichAvatarIsOn = 2;

					// disable the first one and anable the second one
					mike.gameObject.SetActive(false);
					mikeController.MakeInactive();

					flavController.MakeActive();
					flavController.switchCharacter();
					flavController.ResetPosition(mikeController.transform.position);
					flav.gameObject.SetActive(true);
					camera.GetComponent<CameraController>().player = flav;

					break;

				case 2:
					// then the first avatar is on now
					whichAvatarIsOn = 3;

					// disable the second one and anable the first one
					henry.gameObject.SetActive(true);
					henryController.switchCharacter();
					henryController.MakeActive();
					henryController.ResetPosition(flavController.transform.position);

					flavController.MakeInactive();
					flav.gameObject.SetActive(false);
					camera.GetComponent<CameraController>().player = henry;

					break;

				case 3:
					// then the first avatar is on now
					whichAvatarIsOn = 1;

					// disable the second one and anable the first one
					mike.gameObject.SetActive(true);
					mikeController.switchCharacter();
					mikeController.MakeActive();
					mikeController.ResetPosition(henryController.transform.position);

					henryController.MakeInactive();
					henry.gameObject.SetActive(false);
					camera.GetComponent<CameraController>().player = mike;

					break;


			}
		}

	}

}


