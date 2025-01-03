using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View : MonoBehaviour
{
    public static event Action OnButtonClick;

    public TextMeshProUGUI question;
    public QuizButton[] buttons;

    public GameObject CompletePanel;
    public GameObject completePopUpPanel;

    private void OnEnable()
    {
        Controller.OnNextLevel += DisablePopUp;
        Controller.OnNextLevel += ClearButtonsValues;

        Controller.OnLevelComplete += SetActivePopUpPanel;

        Controller.OnGameComplete += SetActiveCompletePanel;
        Controller.OnGameComplete += DisablePopUp;
    }

    private void OnDisable()
    {
        Controller.OnNextLevel -= DisablePopUp;
        Controller.OnNextLevel -= ClearButtonsValues;

        Controller.OnLevelComplete -= SetActivePopUpPanel;

        Controller.OnGameComplete -= SetActiveCompletePanel;
        Controller.OnGameComplete -= DisablePopUp;
    }

    public void SetDefaultValues(DataModel dataModel)
    {
        for(int i=0; i< buttons.Length; i++)
        {
            buttons[i].isCorrect = dataModel.correctQ[i];                           // correct buttons
            buttons[i].buttonText.text = dataModel.words[i];                        // Button Text
        }

        question.text = dataModel.question;
    }

    public void ClearButtonsValues()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button b = buttons[i].button;

            ColorBlock color = b.colors;
            color = ColorBlock.defaultColorBlock;
            b.colors = color;

            b.interactable = true;
        }
    }

    public void onClickButtonColors(QuizButton quizButton)
    {
        Button b = quizButton.button;
        b.interactable = false;

        ColorBlock cb = b.colors;

        if(quizButton.isCorrect)
        {
            cb.disabledColor = Color.green;
            b.colors = cb;
        }
        else
        {
            cb.disabledColor = Color.red;
            b.colors = cb;
        }
    }

    public void SetActiveCompletePanel()
    {
        CompletePanel.SetActive(true);
    }

    public void SetActivePopUpPanel()
    {
        completePopUpPanel.SetActive(true);
    }

    public void DisablePopUp()
    {
        completePopUpPanel.SetActive(false);
    }

}
