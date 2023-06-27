using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewMainMenuUI : MonoBehaviour
{
    
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button quitButton;

    private void Awake() 
    {
        newGameButton.onClick.AddListener(NewGameButtonRoutine);
        optionButton.onClick.AddListener(OptionButtonRoutine);
        quitButton.onClick.AddListener(QuitButtonRoutine);
    }

    private void NewGameButtonRoutine()
    {
        SceneManager.LoadScene(1);
    }

    private void OptionButtonRoutine()
    {

    }

    private void QuitButtonRoutine()
    {
        Application.Quit();
    }




}
