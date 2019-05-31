using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustColor : MonoBehaviour
{
    public enum ColorState
    {
        None,
        PingPong,
        One,
        Inverse,
    }

    ColorState colorState = ColorState.None;

    List<Renderer> list_renderer = new List<Renderer>();
    Tween<Color> tw_color = new Tween<Color>();

    Color start;
    Color end;
    System.Action action; // State에 따른 실행 함수를 받을 delegate 함수
    System.Func<Color, Color, float, Color> function; // 연산될 함수

    float time = 1;
    float elapsedTime = 0.0f;
    [SerializeField]
    bool executed = false; // 실행이 완료되었는가
    bool pingpong = false;

    private void Awake()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>(true);

        if (renderer != null)
            list_renderer.AddRange(renderer);
    }

    void Update()
    {
        if (action != null)
            action();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 0, 200, 200), "PingPong"))
        {
            Execute(Color.white, Color.blue, 2f, Color.Lerp, ColorState.PingPong);
        }
        if (GUI.Button(new Rect(400, 0, 200, 200), "One"))
        {
            Execute(Color.white, Color.blue, 2f, Color.Lerp, ColorState.One);
        }
        if (GUI.Button(new Rect(600, 0, 200, 200), "Inverse"))
        {
            Execute(Color.white, Color.blue, 2f, Color.Lerp, ColorState.Inverse);
        }        
    }

    // AdjustColor의 값들을 세팅하고 state를 바꿔줌
    public void Execute(Color start, Color end, float time, System.Func<Color, Color, float, Color> function,
                        ColorState state = ColorState.One)
    {
        this.start = start;
        this.end = end;
        this.time = time;
        this.function = function;
        tw_color.SetEnd(false);
        ChangeState(state);
    }

    public void ChangeState(ColorState state)
    {
        colorState = state;
        switch (state)
        {
            case ColorState.One:
                action = One;                
                break;

            case ColorState.PingPong:
                action = PingPong;
                break;

            case ColorState.Inverse:
                action = One;
                tw_color.SetTween(end, start, time, function);
                return;
        }
        tw_color.SetTween(start, end, time, function);
    }

    void One()
    {
        Color color = tw_color.Update();

        foreach(var r in list_renderer)
        {
            for(int i=0; i<r.materials.Length; i++)
                r.materials[i].color = color;
        }
    }

    void PingPong()
    {       
        if (tw_color.IsEnd)
        {
            pingpong = !pingpong;
            tw_color.SetEnd(false);
            if (!pingpong)
                tw_color.SetTween(start, end, time, function);
            else
                tw_color.SetTween(end, start, time, function);
        }
        One();
    }
}