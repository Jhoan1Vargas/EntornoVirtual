using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtMessage;

    public string Message { set => txtMessage.text = value;}

    public Color FontColor {set => txtMessage.color = value;}
}
