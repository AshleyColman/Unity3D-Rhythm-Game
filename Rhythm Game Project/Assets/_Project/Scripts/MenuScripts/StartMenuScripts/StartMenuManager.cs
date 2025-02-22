﻿namespace StartMenuScripts
{
    using System.Collections;
    using UnityEngine;
    using TimingScripts;
    using UIScripts;
    using AllMenuScripts;
    using AccountScripts;
    using ModeMenuScripts;

    public sealed class StartMenuManager : InputMenu
    {
        [SerializeField] private GameObject startPanel = default;
        [SerializeField] private TitleText titleText = default;
        [SerializeField] private FadeText fadeText = default;
        [SerializeField] private TimeManager timeManager = default;
        [SerializeField] private FadeTransition fadeTransition = default;
        [SerializeField] private TextTyper textTyper = default;
        [SerializeField] private MenuStack menuStack = default;
        [SerializeField] private OptionMenu optionMenu = default;
        [SerializeField] private ModeMenu modeMenu = default;
        [SerializeField] private InformationPanel informationPanel = default;
        private IEnumerator startGameCoroutine;
        private bool gameStarted = false;

        protected override IEnumerator TransitionInCoroutine()
        {
            titleText.SetText(string.Empty);
            fadeText.SetText(string.Empty);
            screen.SetActive(true);
            startPanel.SetActive(true);
            fadeTransition.TransitionIn();
            fadeText.PlayAlphaCanvasTweenLoop();
            timeManager.SetTimeManager(191, 1000);
            yield return new WaitForSeconds(1f);
            textTyper.TypeTextCancelFalse("rhythm game", titleText.TextArr);
            yield return new WaitForSeconds(textTyper.TextTypingDuration("rhythm game"));
            textTyper.TypeTextCancelFalse("click anywhere to start", fadeText.TextArr);
            yield return new WaitForSeconds(textTyper.TextTypingDuration("click anywhere to start"));
            CheckInput();
            yield return null;
        }
        protected override IEnumerator TransitionOutCoroutine()
        {
            fadeTransition.TransitionOut();
            yield return new WaitForSeconds(1f);
            screen.gameObject.SetActive(false);
            modeMenu.TransitionIn();
            yield return null;
        }
        protected override IEnumerator CheckInputCoroutine()
        {
            while (startPanel.gameObject.activeSelf == true)
            {
                CheckInputToStartGame();
                yield return null;
            }
        }
        private void CheckInputToStartGame()
        {
            if (gameStarted == false)
            {
                if (Input.GetMouseButtonDown(0) == true || Input.GetMouseButtonDown(1))
                {
                    StartGame();
                }
            }
        }
        private void StartGame()
        {
            if (startGameCoroutine != null)
            {
                StopCoroutine(startGameCoroutine);
            }
            startGameCoroutine = StartGameCoroutine();
            StartCoroutine(startGameCoroutine);
        }
        private IEnumerator StartGameCoroutine()
        {
            gameStarted = true;
            fadeText.StopAlphaCanvasTween();
            fadeText.PlayAlphaCanvasTween(0.25f, 5);
            fadeText.PlaySetEasePunchTween();
            textTyper.TypeTextCancelFalse("game started", titleText.TextArr);
            textTyper.TypeTextCancelFalse("welcome", fadeText.TextArr);
            yield return new WaitForSeconds(2f);
            informationPanel.gameObject.SetActive(true);
            fadeText.PlayAlphaCanvasTweenLoop();
            menuStack.TransitionToMenu(optionMenu);
        }
    }
}
