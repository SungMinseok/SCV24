﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueScript : MonoBehaviour
{
    public GameObject[] texts;
    public GameObject prologuePanel;
    public GameObject objectPanel;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Event());
    }

    IEnumerator Event()
    {
        yield return new WaitForSeconds(4f);
        texts[0].SetActive(false);
        texts[1].SetActive(true);
        yield return new WaitForSeconds(3.5f);
        texts[1].SetActive(false);
        texts[2].SetActive(true);//어이 김씨
        yield return new WaitForSeconds(3f);
        texts[2].SetActive(false);
        texts[3].SetActive(true);
        yield return new WaitForSeconds(4f);
        texts[3].SetActive(false);
        texts[4].SetActive(true);
        yield return new WaitForSeconds(4f);
        texts[4].SetActive(false);
        texts[5].SetActive(true);
        yield return new WaitForSeconds(5f);
        EndPrologue();
    }

    public void EndPrologue(){
        SoundManager.instance.BGMPlay(1);
        prologuePanel.SetActive(false);
        objectPanel.SetActive(true);
        SoundManager.instance.Play("rescue");
    }
    public void ExploSound(){
        SoundManager.instance.Play("explo");
    } 
    public void YesSound(){
        SoundManager.instance.Play("RaYes0");
    } 
}
