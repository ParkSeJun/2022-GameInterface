using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static bool PointInRect(Vector2 pt, Rect rect)
	{
		if (pt.x < rect.xMin || rect.xMax < pt.x || pt.y < rect.yMin || rect.yMax < pt.y)
			return false;
		return true;
	}
}
