using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Question", menuName="Question/Empty question", order=1)]
public class Question : ScriptableObject
{
    [SerializeField, TextArea(5, 20)]
    string message;
    [SerializeField]
    string correctOption;
    [SerializeField]
    string incorrectOption;

    public string Message {get => message; set => message = value;}
    public string CorrectOption {get => correctOption; set => correctOption = value;}
    public string IncorrectOption {get => incorrectOption; set => incorrectOption = value;}
}
