using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	public bool canLook = true;
	public float cameraSmoothingFactor = 1;

	public Transform player;
	public Quaternion camRotation;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		camRotation = transform.localRotation;
	}

	void Update()
	{
		camRotation.x = Input.GetAxis ("Mouse X") * cameraSmoothingFactor * (-1f) * Time.deltaTime;
		camRotation.y = Input.GetAxis ("Mouse Y") * cameraSmoothingFactor * Time.deltaTime;

		camRotation.x = Mathf.Clamp (camRotation.x, -90f, 90f);

		if (canLook)
		{
			player.localRotation = Quaternion.Euler (camRotation.x, camRotation.y, camRotation.z);
			transform.localRotation = Quaternion.Euler (camRotation.x, camRotation.y, camRotation.z);
		}
	}
}
