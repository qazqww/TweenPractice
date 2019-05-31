using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 트위닝이란? 오브젝트의 시간에 따른 변화 (크기, 위치, 방향 등)
public class Tween<T>
{
    T start;
    T end;
    T current; // 트위닝 도중 현재 프레임의 상태값를 받음
    public T Current
    {
        get { return current; }
    }

    System.Func<T, T, float, T> function;
    // 연산될 함수 (시작값, 끝값, 시간, 결과값(out))
    // Tween에선 대부분 Lerp 함수가 해당되는 듯

    float time;
    float elapsedTime;

    bool state; // 업데이트가 완료된 상태인가
    public bool IsEnd
    {
        get { return state; }
    }

    public T Update()
    {
        if (state) // 완료된 상태
            return end;
        
        if (function != null) // 시간 경과
        {
            elapsedTime += Time.deltaTime / time;
            elapsedTime = Mathf.Clamp01(elapsedTime);
            current = function(start, end, elapsedTime);
            if (elapsedTime >= 1.0f)
                state = true;
        }

        return current;
    }

    // 트위닝을 실행할 수 있도록 세팅해주는 함수
    public void SetTween(T start, T end, float time, System.Func<T, T, float, T> function)
    {
        this.start = start;
        this.end = end;
        this.time = time;
        this.function = function;
        elapsedTime = 0;
    }

    public void SetEnd(bool state)
    {
        this.state = state;
    }
}