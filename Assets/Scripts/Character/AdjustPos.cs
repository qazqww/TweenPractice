using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustPos : AdjustTween<Vector3, Transform>
{
    Vector3 originPos;

    public override void Init()
    {
        originPos = transform.position;
        components.Add(GetComponent<Transform>());
    }

    public override void SetValue(Vector3 t)
    {
        for (int i = 0; i < components.Count; i++)
            components[i].position = t;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 400, 200, 200), "PingPong"))
        {
            Execute(Vector3.zero + originPos, Vector3.right * 2 + originPos, 1f, MathHelper.EaseOutExpo, State.PingPong);
        }
        if (GUI.Button(new Rect(400, 400, 200, 200), "One"))
        {
            Execute(Vector3.zero + originPos, Vector3.right * 2 + originPos, 1f, MathHelper.EaseOutExpo, State.One);
        }
        if (GUI.Button(new Rect(600, 400, 200, 200), "Inverse"))
        {
            Execute(Vector3.zero + originPos, Vector3.right * 2 + originPos, 1f, MathHelper.EaseOutExpo, State.Inverse);
        }
    }
}
