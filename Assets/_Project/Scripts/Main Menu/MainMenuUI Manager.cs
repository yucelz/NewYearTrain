using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject _MainMenuScreen;
    [SerializeField] private GameObject _HowToPlayScreen;
    [SerializeField] private GameObject _CreditsScreen;

    [Header("Buttons")]
    [SerializeField] private Button _StartButton;
    [SerializeField] private Button _HowToPlayButton;
    [SerializeField] private Button _CreditsButton;
    [SerializeField] private Button _ExitButton;
    [SerializeField] private Button _BackToMainMenuButton;


    private UnityAction _StartButtonAction;
    private UnityAction _HowToPlayButtonAction;
    private UnityAction _CreditsButtonAction;
    private UnityAction _ExitButtonAction;
    private UnityAction _BackToMainMenuButtonAction;


    private void InitializeButtons()
    {
        _StartButtonAction = new UnityAction(OnStartButtonClicked);
        _HowToPlayButtonAction = new UnityAction(OnHowToPlayButtonClicked);
        _CreditsButtonAction = new UnityAction(OnCreditsButtonClicked);
        _ExitButtonAction = new UnityAction(OnExitButtonClicked);
        _BackToMainMenuButtonAction = new UnityAction(OnBackToMainMenuButtonClicked);

        _StartButton.onClick.AddListener(_StartButtonAction);
        _HowToPlayButton.onClick.AddListener(_HowToPlayButtonAction);
        _CreditsButton.onClick.AddListener(_CreditsButtonAction);
        _ExitButton.onClick.AddListener(_ExitButtonAction);
    }

    private void OnStartButtonClicked()
    {
        int GameJam2Index = 1;
        SceneManager.LoadSceneAsync(GameJam2Index);
    }

    private void OnHowToPlayButtonClicked()
    {
        _HowToPlayScreen.SetActive(true);
    }

    private void OnCreditsButtonClicked()
    {
        _CreditsScreen.SetActive(true);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnBackToMainMenuButtonClicked()
    {
        if (_CreditsScreen.activeInHierarchy)
        {
            _CreditsScreen.SetActive(false);
            return;
        }
        _HowToPlayScreen.SetActive(false);
    }
}
