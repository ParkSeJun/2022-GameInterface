using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
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
		GameManager.Instance.player.Jump();
	}
}
