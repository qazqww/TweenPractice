using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustColor : AdjustTween<Color, Renderer>
{
    public override void Init()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>(true);

        if (renderer != null)
            components.AddRange(renderer);
    }

    public override void SetValue(Color color)
    {
        foreach (var r in components)
        {
            for (int i = 0; i < r.materials.Length; i++)
                r.materials[i].color = color;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 0, 200, 200), "PingPong"))
        {
            Execute(Color.white, Color.blue, 2f, Color.Lerp, State.PingPong);
        }
        if (GUI.Button(new Rect(400, 0, 200, 200), "One"))
        {
            Execute(Color.white, Color.blue, 2f, Color.Lerp, State.One);
        }
        if (GUI.Button(new Rect(600, 0, 200, 200), "Inverse"))
        {
            Execute(Color.white, Color.blue, 2f, Color.Lerp, State.Inverse);
        }
    }
}