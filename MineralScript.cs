using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineralScript : MonoBehaviour
{
    public int totalAmount = 10000;
    public int curAmount;
    public Slider bar;
    //public Queue<string> q;
    // Start is called before the first frame update
    void Start()
    {
        curAmount = totalAmount;
        bar.value = (float) curAmount / (float) totalAmount;
    }
    // public void MineralIn(string name){
    //     q.Enqueue(name);
    // }
    // public void MineralOut(){//캐는 도중에 나감 . 강제로 디큐
    //     q.Dequeue();

    // }

    public void GotMined(int gotAmount){
        curAmount -= gotAmount;
        if(curAmount<0){
            gameObject.SetActive(false);
        }
        else{

            bar.value = (float) curAmount / (float) totalAmount;
        }
    }
}
