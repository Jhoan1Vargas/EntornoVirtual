using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class QuestionController : MonoBehaviour
{
    Question question;
    TargetButton leftButton;
    TargetButton rightButton;
    TMP_Text questionText;
    [HideInInspector]
    public Player player;

    void Awake()
    {
        leftButton = transform.Find("Panel/LeftButton").GetComponent<TargetButton>();
        rightButton = transform.Find("Panel/RightButton").GetComponent<TargetButton>();
        questionText = transform.Find("Panel/Text").GetComponent<TMP_Text>();
        this.gameObject.SetActive(false);
    }

    void Start()
    {
        FillQuestion();
    }

    public void FillQuestion()
    {
        question = GameManager.instance.GetQuestion();
        questionText.text = question.Message;

        //limpiar los eventos 
        leftButton.action = new UnityEvent();
        rightButton.action = new UnityEvent();
        if(Random.Range(0, 2) == 0)
        {
            leftButton.transform.Find("Text").GetComponent<TMP_Text>().text = question.CorrectOption;
            leftButton.action.AddListener(() => {
                player.MoveToNextStep();
                this.gameObject.SetActive(false);
            });

            rightButton.transform.Find("Text").GetComponent<TMP_Text>().text = question.IncorrectOption;
            rightButton.action.AddListener(() => {
                player.MoveBack();
                this.gameObject.SetActive(false);
            });
        }
        else
        {
            rightButton.transform.Find("Text").GetComponent<TMP_Text>().text = question.CorrectOption;
            rightButton.action.AddListener(() => {
                player.MoveToNextStep();
                this.gameObject.SetActive(false);
            });

            leftButton.transform.Find("Text").GetComponent<TMP_Text>().text = question.IncorrectOption;
            leftButton.action.AddListener(() => {
                player.MoveBack();
                this.gameObject.SetActive(false);
            });
        }
    }
}
