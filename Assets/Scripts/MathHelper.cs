using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper
{
    // 가속도를 구하는 함수
    public static float EaseOutExpo(float start, float end, float delta)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * delta / 1) + 1) + start;
    }

    public static Vector3 EaseOutExpo(Vector3 start, Vector3 end, float delta)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * delta / 1) + 1) + start;
    }
}
