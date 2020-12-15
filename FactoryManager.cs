﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryManager : MonoBehaviour
{   
    public static FactoryManager instance;

    public Transform botManager;
    public Transform factoryPos;
    public Transform parentPanel;
    Transform[] childPanels;
    public GameObject[] robots;
    public int nowNum;

    [Header("생산 패널")]
    public Text nameText;
    public Text efficiencyText;
    public Text priceText;
    public Text populationText;



    void Start()
    {
        instance = this;
        childPanels = parentPanel.GetComponentsInChildren<Transform>();
        for(int i=0;i<parentPanel.transform.childCount;i++){
            int temp = i;
            parentPanel.GetChild(temp).GetComponent<Button>().onClick.AddListener(()=>OpenProducePanel(temp));
        }   
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
    }
    public void OpenProducePanel(int num){
        nowNum = num;

        nameText.text = BotManager.instance.botInfoList[num].name;
        efficiencyText.text = "본체의 "+(BotManager.instance.botInfoList[num].efficiency*100).ToString()+"%";
        priceText.text = BotManager.instance.botInfoList[num].price.ToString();

    }

    public void ProduceBtn(){
        var clone = Instantiate(robots[nowNum],factoryPos.position,Quaternion.identity);
        clone.GetComponent<BotScript>().botState = BotState.Mine;
        clone.GetComponent<BotScript>().efficiency = BotManager.instance.botInfoList[nowNum].efficiency;
        clone.transform.parent = botManager;
        BotManager.instance.RefreshBotEquip(BotManager.instance.transform.childCount-1);


        BotManager.instance.botSaved.Add(nowNum);
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
    }    
    public void ProduceByLoad(){
        var clone = Instantiate(robots[nowNum],factoryPos.position,Quaternion.identity);
        clone.GetComponent<BotScript>().botState = BotState.Mine;
        clone.GetComponent<BotScript>().efficiency = BotManager.instance.botInfoList[nowNum].efficiency;
        clone.transform.parent = botManager;
        BotManager.instance.RefreshBotEquip(BotManager.instance.transform.childCount-1);


        //BotManager.instance.botSaved.Add(nowNum);
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
    }
}