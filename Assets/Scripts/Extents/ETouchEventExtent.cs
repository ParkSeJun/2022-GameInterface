using UnityEngine;

public static class ETouchEventExtent
{
	public static ETouchEvent ToTouchEvent(this TouchPhase phase)
	{
		switch (phase)
		{
			case TouchPhase.Began:
				return ETouchEvent.START;

			case TouchPhase.Moved:
				return ETouchEvent.MOVE;

			case TouchPhase.Stationary:
				return ETouchEvent.STAY;

			case TouchPhase.Canceled:
			case TouchPhase.Ended:
				return ETouchEvent.RELEASE;
		}

		return ETouchEvent.START;
	}
}
