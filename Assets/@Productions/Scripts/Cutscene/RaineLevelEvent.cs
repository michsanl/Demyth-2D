using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(10)]
public class RaineLevelEvent : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;

    private void Start()
    {
        if (DialogueLua.GetVariable("Level_6_Puzzle_Done").asBool) 
        {
            _globalLight.intensity = 1f;
        }
        else
        {
            _globalLight.intensity = 0f;
        }
    }

    private void OnEnable()
    {
        if (DialogueLua.GetVariable("Level_6_Puzzle_Done").asBool) 
        {
            _globalLight.intensity = 1f;
        }
        else
        {
            _globalLight.intensity = 0f;
        }
    }

    private void OnDisable()
    {
        _globalLight.intensity = 1f;
    }
}
