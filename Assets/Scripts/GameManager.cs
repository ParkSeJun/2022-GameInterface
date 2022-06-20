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

	public enum ScreenMode
	{
		IDLE,
		LT_Mode,
		Inventory,
		Setting,
	}

	public Player player;

	[SerializeField]
	RectTransform canvas;
	[SerializeField]
	Camera cam;
	[SerializeField]
	GameObject aimZoomObject;
	[SerializeField]
	GameObject playerPlane;

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

	[Space(20)]
	[Header("LT모드 설정")]
	[SerializeField]
	GameObject LTObject;
	[SerializeField]
	TMPro.TextMeshProUGUI ltGuideInventoryText;
	[SerializeField]
	TMPro.TextMeshProUGUI ltGuideSettingText;
	[SerializeField, Range(0f, 1f)]
	float ltRangeWidth;
	[SerializeField, Range(0f, 1f)]
	float ltRangeHeight;
	int ltTouchIndex; // LT에 관여하는 터치인덱스 저장


	[Space(20)]
	[SerializeField]
	GameObject fireObject;
	[SerializeField]
	GameObject hpTextObject;
	[SerializeField]
	GameObject ammoTextObject;
	[SerializeField]
	GameObject crouchButtonObject;
	[SerializeField]
	GameObject jumpButtonObject;
	[SerializeField]
	GameObject zoomButtonObject;
	[SerializeField]
	GameObject[] ltBaseObject;
	[SerializeField]
	GameObject inventoryBaseObject;
	[SerializeField]
	GameObject settingBaseObject;

	ScreenMode mode;
	public ScreenMode Mode => mode;

	public bool IsZoomToggle => isZoomToggle;
	bool isZoomToggle; // 줌 토글
	const int fovDefault = 60;
	const int fovZoom = 15;




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

		isZoomToggle = false;
		mode = ScreenMode.IDLE;

		ltTouchIndex = 0;

		touchPad.SetActive(false);
		SetZoom(false);

		RefreshScreen();
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
		float screenWidth = canvas.sizeDelta.x * canvas.localScale.x;
		float screenHeight = canvas.sizeDelta.y * canvas.localScale.y;
		//Debug.Log(position + " " + screenWidth  + " " + screenHeight);

		if (eventType == ETouchEvent.START)
		{
			if (Utils.PointInRect(position, new Rect(0, 0, screenWidth * touchRangeWidth, screenHeight * touchRangeHeight)))
			{
				touchPadTouchIndex = touchIndex;
				touchPad.SetActive(true);
				touchPad.transform.position = position;
				touchPadInnerCircle.localPosition = Vector3.zero;
			}
			else if (Utils.PointInRect(position, new Rect(0, screenHeight - screenHeight * (1f - touchRangeHeight), screenWidth * ltRangeWidth, screenHeight * (1f - touchRangeHeight))))
			{
				ltTouchIndex = touchIndex;
				mode = ScreenMode.LT_Mode;
				ltGuideInventoryText.color = Color.white;
				ltGuideInventoryText.fontSize = 20;
				ltGuideSettingText.color = Color.white;
				ltGuideSettingText.fontSize = 15;
				RefreshScreen();
			}
			else // 나머지 구간은 전부 에이밍으로 처리
			{
				if (mode != ScreenMode.Inventory && mode != ScreenMode.Setting)
					lastAimingPosition = position;
			}
		}
		else if (eventType == ETouchEvent.MOVE)
		{
			if (touchPadTouchIndex == touchIndex && touchPad.activeSelf)
			{
				touchPadInnerCircle.localPosition = position - (Vector2)touchPad.transform.position;
			}
			else if (ltTouchIndex == touchIndex && LTObject.activeSelf)
			{
				if (position.x > screenWidth * ltRangeWidth)
				{
					ltGuideInventoryText.color = new Color(0f, 0.5568628f, 1f);
					ltGuideInventoryText.fontSize = 25;
					ltGuideSettingText.color = Color.white;
					ltGuideSettingText.fontSize = 15;
				}
				else if (position.y < screenHeight * (1f - ltRangeHeight))
				{
					ltGuideInventoryText.color = Color.white;
					ltGuideInventoryText.fontSize = 20;
					ltGuideSettingText.color = new Color(1f, 0.6705883f, 0f);
					ltGuideSettingText.fontSize = 20;
				}
				else
				{
					ltGuideInventoryText.color = Color.white;
					ltGuideInventoryText.fontSize = 20;
					ltGuideSettingText.color = Color.white;
					ltGuideSettingText.fontSize = 15;
				}
			}
			else // 나머지 구간은 전부 에이밍으로 처리
			{
				if (mode != ScreenMode.Inventory && mode != ScreenMode.Setting)
				{
					player.AddRotate(position - lastAimingPosition);
					lastAimingPosition = position;
				}
			}
		}
		else if (eventType == ETouchEvent.RELEASE)
		{
			if (touchPadTouchIndex == touchIndex && touchPad.activeSelf)
			{
				touchPad.SetActive(false);
				player.Tilt(0f);
			}
			else if (ltTouchIndex == touchIndex && LTObject.activeSelf)
			{
				if (position.x > screenWidth * ltRangeWidth)
				{
					mode = ScreenMode.Inventory;
					RefreshScreen();
				}
				else if (position.y < screenHeight * (1f - ltRangeHeight))
				{
					mode = ScreenMode.Setting;
					RefreshScreen();
				}
				else
				{
					mode = ScreenMode.IDLE;
					RefreshScreen();
				}
			}
		}
	}

	public void RefreshScreen()
	{
		Debug.Log(mode);

		// 격발
		fireObject.SetActive(mode == ScreenMode.IDLE);

		// HP 텍스트
		hpTextObject.SetActive(mode == ScreenMode.LT_Mode);
		// Ammo 텍스트
		ammoTextObject.SetActive(mode == ScreenMode.LT_Mode);

		// 앉기버튼
		crouchButtonObject.SetActive(mode == ScreenMode.IDLE);
		// 점프버튼
		jumpButtonObject.SetActive(mode == ScreenMode.IDLE);
		// 조준버튼
		zoomButtonObject.SetActive(mode == ScreenMode.IDLE);

		// LT Guide
		LTObject.SetActive(mode == ScreenMode.LT_Mode);
		// LTBase
		foreach (var e in ltBaseObject)
			e.SetActive(mode == ScreenMode.LT_Mode);

		// Setting
		settingBaseObject.SetActive(mode == ScreenMode.Setting);
		// inventory
		inventoryBaseObject.SetActive(mode == ScreenMode.Inventory);
	}


	public void ZoomToggle()
	{
		SetZoom(!isZoomToggle);
	}

	private void SetZoom(bool flag)
	{
		isZoomToggle = flag;

		cam.fieldOfView = isZoomToggle ? fovZoom : fovDefault;
		aimZoomObject.SetActive(isZoomToggle);
		playerPlane.SetActive(!isZoomToggle);
	}

	private void OnDrawGizmos()
	{
		float screenWidth = canvas.sizeDelta.x * canvas.localScale.x;
		float screenHeight = canvas.sizeDelta.y * canvas.localScale.y;

		Gizmos.color = Color.green;

		Gizmos.DrawLine(Vector3.up * screenHeight * touchRangeHeight, new Vector3(screenWidth * touchRangeWidth, screenHeight * touchRangeHeight));
		Gizmos.DrawLine(Vector3.right * screenWidth * touchRangeWidth, new Vector3(screenWidth * touchRangeWidth, screenHeight * touchRangeHeight));

		Gizmos.color = Color.yellow;

		//Debug.Log(canvas.sizeDelta);
		//Gizmos.DrawLine(Vector3.zero, Vector3.right * canvas.sizeDelta.x * canvas.localScale.x + Vector3.up * canvas.sizeDelta.y * canvas.localScale.y);

		Gizmos.DrawLine(Vector3.up * screenHeight * (1f - ltRangeHeight), new Vector3(screenWidth * ltRangeWidth, screenHeight * (1f - ltRangeHeight)));
		Gizmos.DrawLine(Vector3.right * screenWidth * ltRangeWidth + Vector3.up * screenHeight, new Vector3(screenWidth * ltRangeWidth, screenHeight * (1f - ltRangeHeight)));
	}


}
