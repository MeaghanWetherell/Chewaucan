using System.Collections;
using Cinemachine;
using UnityEngine;

//script written 6-6-2026 using Claude Code
//intended to fix the weird offset issue with the virtual camera that happens when you are in the water.
public class SwimCameraAdjust : MonoBehaviour
{
    [Tooltip("Normal follow offset Y (on land)")]
    public float normalOffsetY = 1.5f;

    [Tooltip("Follow offset Y when surface swimming")]
    public float swimOffsetY = 0.5f;

    [Tooltip("Follow offset Y when diving underwater")]
    public float diveOffsetY = 0f;

    [Tooltip("How quickly the camera lerps to the new offset")]
    public float lerpSpeed = 3f;

    private CinemachineTransposer _transposer;
    private float _targetOffsetY;

    private void Awake()
    {
        _transposer = GetComponent<CinemachineVirtualCamera>()
            .GetCinemachineComponent<CinemachineTransposer>();
        _targetOffsetY = normalOffsetY;
    }

    public void SetSwimming(bool isSwimming)
    {
        _targetOffsetY = isSwimming ? swimOffsetY : normalOffsetY;
        print(swimOffsetY);
    }

    public void SetDiving(bool isDiving)
    {
        _targetOffsetY = isDiving ? diveOffsetY : swimOffsetY;
        print(diveOffsetY);
    }

    private void Update()
    {
        float current = _transposer.m_FollowOffset.y;
        if (Mathf.Approximately(current, _targetOffsetY)) return;
        _transposer.m_FollowOffset.y = Mathf.Lerp(current, _targetOffsetY, Time.deltaTime * lerpSpeed);
    }
}