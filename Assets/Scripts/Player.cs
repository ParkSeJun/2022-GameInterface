using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public enum PoseState
	{
		STAND,
		CROUCH,
		// KNEEL_DOWN,
	}


	[SerializeField]
	Transform cameraArm;
	[SerializeField]
	Transform cameraTilt;
	[SerializeField]
	Transform plane;
	[SerializeField]
	Transform standOrCrouch;

	[SerializeField]
	float speed = 5f;
	[SerializeField]
	float jumpPower = 500f;

	[SerializeField]
	float angleSpeedX = 8f;
	[SerializeField]
	float angleSpeedY = 6f;

	[SerializeField]
	float cameraTiltScale = 25f;

	CharacterController controller;
	float cameraArmRotation;
	float cameraTiltRotation;
	float cameraTiltRotationDest;

	PoseState pose;
	public PoseState Pose => pose;

	private void Start()
	{
		controller = GetComponent<CharacterController>();

		cameraArmRotation = 1.59f;
		cameraTiltRotation = 0f;
		cameraTiltRotationDest = 0f;

		pose = PoseState.STAND;
	}

	private void Update()
	{
		cameraTiltRotation = Mathf.Lerp(cameraTiltRotation, cameraTiltRotationDest, 0.1f);
		cameraTilt.localEulerAngles = Vector3.back * cameraTiltRotation;
	}

	public void AddVelocity(Vector3 direction)
	{
		//controller.Move((transform.forward * direction.y + transform.right * direction.x) * Time.deltaTime * speed);
		transform.position += (transform.forward * direction.y + transform.right * direction.x) * Time.deltaTime * speed * (pose == PoseState.CROUCH ? 0.5f : 1f);
	}

	public void Tilt(float normalizedDirection)
	{
		cameraTiltRotationDest = normalizedDirection * cameraTiltScale;
	}

	public void StandUp()
	{
		pose = PoseState.STAND;
		plane.localPosition = new Vector3(plane.localPosition.x, 0f, plane.localPosition.z);
		plane.localScale = new Vector3(plane.localScale.x, plane.localScale.y, 0.19272f);
		standOrCrouch.localPosition = Vector3.zero;
	}

	public void CrouchDown()
	{
		pose = PoseState.CROUCH;
		plane.localPosition = new Vector3(plane.localPosition.x, -0.6f, plane.localPosition.z);
		plane.localScale = new Vector3(plane.localScale.x, plane.localScale.y, 0.19272f *0.5f);
		standOrCrouch.localPosition = Vector3.down * 1.2f;
	}

	public void Jump()
	{
		Rigidbody rig = GetComponent<Rigidbody>();
		rig.AddForce(0f, jumpPower, 0f);
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
