using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TargetButton : MonoBehaviour
{
    [HideInInspector]
    public Image buttonImage;

    [HideInInspector]
    public UnityEvent action = new UnityEvent();

    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    public void Action()
    {
        if (action != null)
        {
            action.Invoke();
        }
    }
}
