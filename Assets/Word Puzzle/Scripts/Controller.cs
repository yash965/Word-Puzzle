using System;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static event Action SetButtonsValue;
    public static event Action OnNextLevel;
    public static event Action OnLevelComplete;
    public static event Action OnGameComplete;

    public DataModel dataModel;
    public Animator animator;
    public View view;
    public Level levels;

    private int correct = 0;

    // Button Listeners will be added here.

    void Start()
    {
        view.SetDefaultValues(dataModel);
        UpdateWeatherEffect();
        SetListeners();
    }

    public void UpdateWeatherEffect()
    {
        if (animator != null)
        {
            animator.SetInteger("WeatherType", (int)dataModel.selectedWeatherType);
        }
    }

    public void SetCompletionWeather()
    {
        if (animator != null)
        {
            animator.SetInteger("WeatherType", (int)dataModel.completeWeatherType);
        }
    }

    public void SetListeners()
    {
        foreach(QuizButton b in view.buttons)
        {
            b.button.onClick.AddListener(() => ButtonFunctions(b));
        }
    }

    public void ButtonFunctions(QuizButton button)
    {
        view.onClickButtonColors(button);
        OnCorrectOptionClicked(button);
    }

    public void OnCorrectOptionClicked(QuizButton button)
    {
        if (button.isCorrect)
        {
            correct++;
        }

        if(correct >= dataModel.QuesToClearLevel)
        {
            SetCompletionWeather();
            OnLevelComplete?.Invoke();
        }
    }

    public void MoveToNextLevel()
    {
        levels.levelIndex += 1;

        if(levels.levels.Length <= levels.levelIndex)
        {
            OnGameComplete?.Invoke();
            return;
        }

        dataModel = Resources.Load<DataModel>(levels.levels[levels.levelIndex]);
        OnNextLevel?.Invoke();
        view.SetDefaultValues(dataModel);
        UpdateWeatherEffect();
        correct = 0;
    }
}
