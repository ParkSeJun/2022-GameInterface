using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    TMPro.TMP_Text cached_text;
    float lastUpdateTime;

    // Start is called before the first frame update
    void Start()
    {
        lastUpdateTime = 0f;
        cached_text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - lastUpdateTime <= 0.1f)
            return;

        cached_text.text = "FPS : " + Mathf.Floor(1f / Time.deltaTime).ToString();
        lastUpdateTime = Time.realtimeSinceStartup;
    }
}
