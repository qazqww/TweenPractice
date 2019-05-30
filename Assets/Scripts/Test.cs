using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform player;

    Tween<Vector3> scale;

    void Start()
    {
        
    }
    
    void Update()
    {
        if(scale != null)
        {
            player.localScale = scale.Update();
            if (scale.IsEnd)
                scale = null;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0,0,100,100), "Test")) {
            scale = new Tween<Vector3>();
            scale.SetTween(Vector3.one, Vector3.one * 3, 3.0f, MathHelper.EaseOutExpo, 1);
        }
    }
}
