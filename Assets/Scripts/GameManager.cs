using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance = null;
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = Object.FindObjectOfType<GameManager>();
			return _instance;
		}
	}

	public Player player;

	[SerializeField]
	RectTransform canvas;

	[Header("터치패드 설정")]
	[SerializeField]
	GameObject touchPad;
	[SerializeField, Range(0f, 1f)]
	float touchRangeWidth;
	[SerializeField, Range(0f, 1f)]
	float touchRangeHeight;
	[SerializeField]
	RectTransform touchPadInnerCircle;
	int touchPadTouchIndex; // 터치패드에 관여하는 터치인덱스 저장

	Dictionary<int, Vector2> lastTouchPositionDic;
	Vector2 lastAimingPosition;
	//[Space(20)]



	void Awake()
	{
		Application.targetFrameRate = 60;
	}

	// Start is called before the first frame update
	void Start()
	{
		touchPadTouchIndex = 0;
		lastTouchPositionDic = new Dictionary<int, Vector2>();
		lastAimingPosition = Vector2.zero;

		touchPad.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount > 0)
		{
			for (int i = 0; i < Input.touchCount; ++i)
			{
				ProcessTouch(Input.GetTouch(i).position, Input.GetTouch(i).phase.ToTouchEvent(), i);
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
				ProcessTouch(Input.mousePosition, ETouchEvent.START);
			else if (Input.GetMouseButtonUp(0))
				ProcessTouch(Input.mousePosition, ETouchEvent.RELEASE);
			else if (Input.GetMouseButton(0))
				ProcessTouch(Input.mousePosition, ETouchEvent.MOVE);
		}


	}

	void ProcessTouch(Vector2 position, ETouchEvent eventType, int touchIndex = 0)
	{
		//Debug.Log(position + " " + eventType.ToString());

		if (eventType == ETouchEvent.START)
		{
			if (Utils.PointInRect(position, new Rect(0, 0, canvas.rect.width * touchRangeWidth, canvas.rect.height * touchRangeHeight)))
			{
				touchPadTouchIndex = touchIndex;
				touchPad.SetActive(true);
				touchPad.transform.position = position;
				touchPadInnerCircle.localPosition = Vector3.zero;
			}
			//else if( TODO: 모드 전환부 터치 구간 판정 )
			//{
			//}
			else // 나머지 구간은 전부 에이밍으로 처리
			{
				lastAimingPosition = position;
			}
		}
		else if (eventType == ETouchEvent.MOVE)
		{
			if (touchPadTouchIndex == touchIndex && touchPad.activeSelf)
			{
				touchPadInnerCircle.localPosition = position - (Vector2)touchPad.transform.position;
			}
			//else if( TODO: 모드 전환부 터치 구간 판정 )
			//{
			//}
			else // 나머지 구간은 전부 에이밍으로 처리
			{
				player.AddRotate(position - lastAimingPosition);
				lastAimingPosition = position;
			}
		}
		else if (eventType == ETouchEvent.RELEASE)
		{
			if (touchPadTouchIndex == touchIndex && touchPad.activeSelf)
				touchPad.SetActive(false);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawLine(Vector3.up * canvas.rect.height * touchRangeHeight, new Vector3(canvas.rect.width * touchRangeWidth, canvas.rect.height * touchRangeHeight));
		Gizmos.DrawLine(Vector3.right * canvas.rect.width * touchRangeWidth, new Vector3(canvas.rect.width * touchRangeWidth, canvas.rect.height * touchRangeHeight));
	}


}
