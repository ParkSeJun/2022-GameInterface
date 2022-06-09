using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    [SerializeField]
    Transform innerCircleTransform;

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = ((Vector2)innerCircleTransform.localPosition).normalized;

        GameManager.Instance.player.AddVelocity(new Vector3(direction.x, direction.y));
    }
}
