using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[DefaultExecutionOrder(10)]
public class RaineLevelEvent : MonoBehaviour
{
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private GameObject _restartHandler;

    private void Start()
    {
        if (DialogueLua.GetVariable("Level_6_Puzzle_Done").asBool) 
        {
            _globalLight.intensity = 1f;
            _restartHandler.SetActive(true);
        }
        else
        {
            _globalLight.intensity = 0f;
        }

        if (DialogueLua.GetVariable("Level_7_Done").asBool)
        {
            _restartHandler.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (DialogueLua.GetVariable("Level_6_Puzzle_Done").asBool) 
        {
            _globalLight.intensity = 1f;
            _restartHandler.SetActive(true);
        }
        else
        {
            _globalLight.intensity = 0f;
        }

        if (DialogueLua.GetVariable("Level_7_Done").asBool)
        {
            _restartHandler.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _globalLight.intensity = 1f;
        _restartHandler.SetActive(false);
    }
}
