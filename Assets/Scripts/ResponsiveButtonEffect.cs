using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResponsiveButtonEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	GameObject gray;

	//public void Start()
	//{
	//	GetComponent<Button>().onClick.AddListener(() => {  });
	//}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		gray.SetActive(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		gray.SetActive(false);
	}

	
}
