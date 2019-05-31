using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustTween<T, TCOMPONENT> : MonoBehaviour
{
    public enum State
    {
        None,
        PingPong,
        One,
        Inverse,
    }

    State state;

    protected List<TCOMPONENT> components = new List<TCOMPONENT>();
    Tween<T> tween = new Tween<T>();

    T start;
    T end;
    System.Action action;
    System.Func<T, T, float, T> function;

    float time = 1;
    float elapsedTime = 0.0f;
    [SerializeField]
    bool executed = false;
    bool pingpong = false; // 정->역->정 반복

    public virtual void Init()
    {

    }

    private void Awake()
    {
        Init();        
    }

    void Update()
    {
        if (action != null)
            action();
    }    

    public void Execute(T start, T end, float time, System.Func<T, T, float, T> function,
                        State state = State.One, bool delay = false, float delayTime = -1)
    {
        this.start = start;
        this.end = end;
        this.time = time;
        this.function = function;
        tween.SetEnd(false);
        ChangeState(state, delayTime);
    }

    public void ChangeState(State state, float delayTime = -1)
    {
        this.state = state;
        switch (state)
        {
            case State.One:
                action = One;
                break;

            case State.PingPong:
                action = PingPong;
                break;

            case State.Inverse:
                action = One;
                tween.SetTween(end, start, time, function, delayTime);
                return;
        }
        tween.SetTween(start, end, time, function, delayTime);
    }

    public virtual void SetValue(T t)
    {

    }

    void One()
    {
        SetValue(tween.Update());
        if (tween.IsEnd)
            action = null;
    }

    void PingPong()
    {
        One();
        action = PingPong;
        if (tween.IsEnd)
        {            
            pingpong = !pingpong;
            tween.SetEnd(false);
            if (!pingpong)
                tween.SetTween(start, end, time, function);
            else
                tween.SetTween(end, start, time, function);
        }        
    }
}