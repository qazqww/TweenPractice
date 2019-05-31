using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustScale : AdjustTween<Vector3, Transform>
{
    public override void Init()
    {
        components.Add(GetComponent<Transform>());
    }

    public override void SetValue(Vector3 t)
    {
        for (int i = 0; i < components.Count; i++)
            components[i].localScale = t;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 200, 200, 200), "PingPong"))
        {
            Execute(Vector3.one, Vector3.one * 2, 1f, Vector3.Lerp, State.PingPong);
        }
        if (GUI.Button(new Rect(400, 200, 200, 200), "One"))
        {
            Execute(Vector3.one, Vector3.one * 2, 1f, Vector3.Lerp, State.One);
        }
        if (GUI.Button(new Rect(600, 200, 200, 200), "Inverse"))
        {
            Execute(Vector3.one, Vector3.one * 2, 1f, Vector3.Lerp, State.Inverse);
        }
    }
}