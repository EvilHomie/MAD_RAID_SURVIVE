using System;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowService : MonoBehaviour
{
    public Action CustomUpdate;
    public Action CustomFixedUpdate;
    public Action<object> SetPause;
    public Action<object> SetResume;
    public GameFlowStatus GetStatus => Time.timeScale == 1 ? GameFlowStatus.Play : GameFlowStatus.Pause;

    List<object> _pauseObjects = new();

    void OnEnable()
    {
        SetPause += OnSetPause;
        SetResume += OnSetResume;
    }
    private void OnDisable()
    {
        SetPause -= OnSetPause;
        SetResume -= OnSetResume;
    }

    void OnSetPause(object obj)
    {
        _pauseObjects.Add(obj);
        Time.timeScale = 0f;
    }

    void OnSetResume(object obj)
    {
        _pauseObjects.Remove(obj);
        if (_pauseObjects.Count == 0)
        {
            Time.timeScale = 1f;
        }
    }

    void Update()
    {
        CustomUpdate?.Invoke();
    }

    void FixedUpdate()
    {
        CustomFixedUpdate?.Invoke();
    }


}

public enum GameFlowStatus
{
    Play,
    Pause
}