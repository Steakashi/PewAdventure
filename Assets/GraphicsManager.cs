using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager : MonoBehaviour
{

    public float slowRatio;

    private float fixedDeltaTime;

    void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }

    public float ponderate(float time, bool multiply=true) {
        return multiply ? time * Time.timeScale : time / Time.timeScale;
    }

    public void slow()
    {
        Time.timeScale = slowRatio;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }

    public void restore()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}
