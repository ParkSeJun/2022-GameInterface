using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
	[SerializeField]
	Transform innerCircleTransform;

	[SerializeField]
	float deadZoneDistance = 10f;

	// Update is called once per frame
	void Update()
	{
		if (Vector2.Distance(Vector2.zero, (Vector2)innerCircleTransform.localPosition) < deadZoneDistance)
			return;

		Vector2 direction = ((Vector2)innerCircleTransform.localPosition).normalized;

		if (GameManager.Instance.IsZoomToggle)
		{
			// y�� 66% �̻��̸� �׳� ����
			// y�� 33% �̻��̸� ���� ����̱�
			// y�� -33% �̻��̸� �翷�̵�
			// y�� -66% �̻��̸� �ɾ� ����̱�
			// �׿�, �ɱ�

			if (direction.y >= 0.66f)
			{
				GameManager.Instance.player.StandUp();
				GameManager.Instance.player.Tilt(0f);
			}
			else if (direction.y >= 0.22f)
			{
				GameManager.Instance.player.StandUp();
				if (Mathf.Abs(innerCircleTransform.localPosition.x) < deadZoneDistance)
					GameManager.Instance.player.Tilt(0f);
				else
					GameManager.Instance.player.Tilt(innerCircleTransform.localPosition.x < 0f ? -1f : 1f);
			}
			else if (direction.y >= -0.22f)
			{
				GameManager.Instance.player.Tilt(0f);
				GameManager.Instance.player.AddVelocity(new Vector3(direction.x < 0f ? -1f : 1f, 0f));
			}
			else if (direction.y >= -0.66f)
			{
				GameManager.Instance.player.CrouchDown();
				if (Mathf.Abs(innerCircleTransform.localPosition.x) < deadZoneDistance)
					GameManager.Instance.player.Tilt(0f);
				else
					GameManager.Instance.player.Tilt(innerCircleTransform.localPosition.x < 0f ? -1f : 1f);
			}
			else
			{
				GameManager.Instance.player.CrouchDown();
				GameManager.Instance.player.Tilt(0f);
			}
		}
		else
		{
			GameManager.Instance.player.AddVelocity(new Vector3(direction.x, direction.y));
		}
	}
}
