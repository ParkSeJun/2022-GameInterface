using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	Transform cameraArm;

	[SerializeField]
	float speed = 5f;

	[SerializeField]
	float angleSpeedX = 8f;
	[SerializeField]
	float angleSpeedY = 6f;


	CharacterController controller;
	float cameraArmRotation;

	private void Start()
	{
		controller = GetComponent<CharacterController>();

		cameraArmRotation = 1.59f;
	}

	public void AddVelocity(Vector3 direction)
	{
		//controller.Move((transform.forward * direction.y + transform.right * direction.x) * Time.deltaTime * speed);
		transform.position += (transform.forward * direction.y + transform.right * direction.x) * Time.deltaTime * speed;
	}

	float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
			angle += 360f;

		if (angle > 360f)
			angle -= 360f;

		return Mathf.Clamp(angle, min, max);
	}

	public void AddRotate(Vector2 rotate)
	{
		rotate *= Time.deltaTime;
		transform.localEulerAngles += Vector3.up * rotate.x * angleSpeedX;

		cameraArmRotation = ClampAngle(cameraArmRotation - rotate.y * angleSpeedY, -45f, 70f);

		Quaternion q = Quaternion.Euler(cameraArmRotation, 0f, 0f);

		//Vector3 newCameraArmAngle = cameraArm.localEulerAngles;
		//newCameraArmAngle.x = newCameraArmAngleX;
		cameraArm.localRotation = q;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
	}


}
