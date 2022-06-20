using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchButton : MonoBehaviour
{
	UnityEngine.UI.Button button;

	// Start is called before the first frame update
	void Start()
	{
		button = GetComponent<UnityEngine.UI.Button>();

		button.onClick.AddListener(OnClick);
	}

	void OnClick()
	{
		if (GameManager.Instance.player.Pose == Player.PoseState.CROUCH)
			GameManager.Instance.player.StandUp();
		else
			GameManager.Instance.player.CrouchDown();
	}
}
