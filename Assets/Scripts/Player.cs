using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class Player : NetworkBehaviour
{
    [SerializeField]
    public Transform[] Positions;
    public int CurrentPos = 0;
    public VRCamera VRPlayer;
    [SerializeField]
    GameObject[] questions;
    [SerializeField]
    float moveSpeed = 3;
    [SerializeField]
    GameObject bridges;

    public void StartPlayer()
    {
        CurrentPos = 0;
        VRPlayer.transform.localPosition = new Vector3(0f, -0.17f, 0f);
        VRPlayer.XRRig.position = Positions[CurrentPos].position;
    }

    Coroutine coroutine;
    public void MoveToNextStep()
    {
        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(MoveToPoint(VRPlayer, Positions[++CurrentPos].position, moveSpeed, CurrentPos == Positions.Length - 1));
    }

    public void RestartMoveToNextStep()
    {
        if(CurrentPos == 0)
        {
            foreach (GameObject question in questions)
            {
                question.SetActive(false);
                question.GetComponent<QuestionController>().FillQuestion();
            }
            if (coroutine != null) StopCoroutine(coroutine);
            CurrentPos = 1;
            coroutine = StartCoroutine(MoveToPoint(VRPlayer, Positions[CurrentPos].position, moveSpeed, CurrentPos == Positions.Length - 1));
        }
    }

    public void MoveBack()
    {
        bridges.SetActive(false);
        StartCoroutine(Fall(VRPlayer, GameManager.instance.floorPosition.position, 5.3f));
    }

    public void ResetPlayer()
    {
        bridges.SetActive(true);
        CurrentPos = 0;
        VRPlayer.transform.rotation = Quaternion.Euler(new Vector3(0f,-90f,0f));
        VRPlayer.transform.localPosition = new Vector3(0f, -0.17f, 0f);
        VRPlayer.XRRig.position = Positions[CurrentPos].position;

        MoveToNextStep();
        foreach(GameObject question in questions)
        {
            question.GetComponent<QuestionController>().FillQuestion();
        }
    }

    IEnumerator MoveToPoint(VRCamera VRPlayer, Vector3 target, float speed, bool lastPosition)
    {
        VRPlayer.transform.Find("Persiano").GetComponent<Animator>().SetBool("walking", true);
        while (Vector3.Distance(VRPlayer.XRRig.position, target) > 0.1f)
        {
            float step = speed * Time.deltaTime;
            VRPlayer.XRRig.position = Vector3.MoveTowards(VRPlayer.XRRig.position, target, step);
            yield return null;
        }
        if(!lastPosition)
        { 
            StartCoroutine(AnimateQuestion(questions[CurrentPos - 1]));
        }
        VRPlayer.transform.Find("Persiano").GetComponent<Animator>().SetBool("walking", false);
    }

    IEnumerator Fall(VRCamera VRPlayer, Vector3 floorPosition, float gravity)
    {
        float speed = 0;
        while (VRPlayer.XRRig.position.y >= floorPosition.y)
        {
            speed += gravity * Time.deltaTime;
            float step = speed * Time.deltaTime;
            VRPlayer.XRRig.position = new Vector3(VRPlayer.XRRig.position.x, VRPlayer.XRRig.position.y - step, VRPlayer.XRRig.position.z);
            yield return null;
        }
        ResetPlayer();
    }

    IEnumerator AnimateQuestion(GameObject question)
    {
        Vector3 initialScale = question.transform.localScale;
        question.transform.localScale *= 0;
        question.SetActive(true);
        float scale = 0f;
        while (scale < 1.1f)
        {
            scale += Time.deltaTime * 6f;
            question.transform.localScale = scale * initialScale;
            yield return null;
        }
        while(scale > 1f)
        {
            scale -= Time.deltaTime * 2f;
            question.transform.localScale = scale * initialScale;
            yield return null;
        }
    }
}
