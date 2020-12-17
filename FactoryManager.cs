using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryManager : MonoBehaviour
{   
    public static FactoryManager instance;

    [Header("재판매 할인율")]
    public float discount = 0.5f;
    public Transform botManager;
    public Transform factoryPos;
    public Transform parentPanel;
    public Transform[] childPanels;
    public GameObject[] robots;
    public int nowNum;
    public int pricePerUpgrade = 500;
    public Text priceNextUpgradeText;

    [Header("생산 패널")]
    public Text nameText;
    public Text efficiencyText;
    public Text priceText;
    public Text populationText;
    public int population;

    public bool[] unlockedNextProduce;
    public GameObject unlockCover;

    [Header("장비 리스트")]
    public GameObject botStatusPanel;
    public Transform botStatusScroll;
    public Transform[] botStatusScrollChildren;
    public Text nameText_Status;
    public Text priceText_Status;
    public GameObject sellLock;
    public Sprite nullSprite;
    void Awake(){
        
        //unlockedNextProduce.Initialize();
    }
    void Start()
    {
        instance = this;
        
        unlockedNextProduce = new bool[8];
        childPanels = new Transform[parentPanel.childCount];
        botStatusScrollChildren = new Transform[botStatusScroll.childCount];
        //childPanels = parentPanel.GetComponentsInChildren<Transform>();
        for(int i=0;i<parentPanel.transform.childCount;i++){
            childPanels[i]=parentPanel.GetChild(i);
            int temp = i;
            parentPanel.GetChild(temp).GetComponent<Button>().onClick.AddListener(()=>OpenProducePanel(temp));
            childPanels[temp].GetChild(2).GetComponent<Button>().onClick.AddListener(()=>OpenLockedBtn(temp));
        }   
        for(int i=0;i<botStatusScroll.childCount;i++){
            int temp = i;
            botStatusScrollChildren[temp] = botStatusScroll.GetChild(temp);
            botStatusScrollChildren[temp].GetComponent<Button>().onClick.AddListener(()=>ShowBotStatusEach(temp));
        }
        populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;
    }
    public void OpenProducePanel(int num){
        nowNum = num;

        nameText.text = BotManager.instance.botInfoList[num].name;
        efficiencyText.text = "본체의 "+(BotManager.instance.botInfoList[num].efficiency*100).ToString()+"%"+"(<color=#C0F678>"+Mathf.RoundToInt(BotManager.instance.botInfoList[num].efficiency*PlayerManager.instance.capacity)+"</color>)";
        priceText.text = BotManager.instance.botInfoList[num].price.ToString();

    }

    public void ProduceBtn(){
        if(BotManager.instance.botSaved.Count<BotManager.instance.maxPopulation){ 

            if(PlayerManager.instance.curMineral>=BotManager.instance.botInfoList[nowNum].price){
                
                PlayerManager.instance.HandleMineral(-BotManager.instance.botInfoList[nowNum].price);
                    
                var clone = Instantiate(robots[nowNum],factoryPos.position,Quaternion.identity);
                clone.GetComponent<BotScript>().botState = BotState.Mine;
                clone.GetComponent<BotScript>().efficiency = BotManager.instance.botInfoList[nowNum].efficiency;
                clone.transform.parent = botManager;
                BotManager.instance.RefreshBotEquip(BotManager.instance.transform.childCount-1);


                BotManager.instance.botSaved.Add(nowNum);
                populationText.text = "인구수 : "+BotManager.instance.botSaved.Count.ToString()+"/"+BotManager.instance.maxPopulation;

                SoundManager.instance.Play("ready");
            } 
            else{
                SoundManager.instance.Play("notenoughmin");
            }

        }
        else{
            
            Debug.Log("인구수 부족");
        }

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

    public void ApplyUnlocked(){
        for(int i=0;i<unlockedNextProduce.Length;i++){
            if(unlockedNextProduce[i]){
                childPanels[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else{
                childPanels[i].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        childPanels[0].transform.GetChild(2).gameObject.SetActive(false);
    }
    
    public void OpenLockedBtn(int num){
        nowNum = num;
        
        priceNextUpgradeText.text = (num * (num-1) * pricePerUpgrade).ToString();
        
        if(PlayerManager.instance.curRP >= num * (num-1) * pricePerUpgrade){
            unlockCover.SetActive(false);
        }
        else{
            unlockCover.SetActive(true);
        }
        
    }
    public void Unlock(){

        unlockedNextProduce[nowNum] = true;
        childPanels[nowNum].transform.GetChild(2).gameObject.SetActive(false);                
        PlayerManager.instance.curRP -= nowNum* (nowNum-1) * pricePerUpgrade;
    }

    public void ResetData(){
        
        for(int i=0;i<unlockedNextProduce.Length;i++){
            unlockedNextProduce[i] = false;
            childPanels[i].transform.GetChild(2).gameObject.SetActive(true);
        }
        childPanels[0].transform.GetChild(2).gameObject.SetActive(false);
    }
    public void EnrollBot(){
        
    }
    public void OpenBotStatusPanel(){
        //Debug.Log(BotManager.instance.botSaved.Count + "개의 로봇");
        Debug.Log("~"+BotManager.instance.botSaved.Count);
        for(int i=0;i<BotManager.instance.botSaved.Count;i++){
            botStatusScrollChildren[i].GetChild(0).GetComponent<Image>().sprite = childPanels[BotManager.instance.botSaved[i]].GetChild(1).GetComponent<Image>().sprite;
            botStatusScrollChildren[i].GetChild(0).GetComponent<Image>().color = childPanels[BotManager.instance.botSaved[i]].GetChild(1).GetComponent<Image>().color;
        }
        Debug.Log(BotManager.instance.botSaved.Count+"~"+tempPre);
        for(int i=BotManager.instance.botSaved.Count;i<tempPre;i++){

        botStatusScrollChildren[i].GetChild(0).GetComponent<Image>().sprite = nullSprite;
        }
    }
    int selectedNum;
    public void ShowBotStatusEach(int num){ // BotManager.instance.botSaved[num] : scv번호
    selectedNum = num;
    //Debug.Log(num + "/ "+BotManager.instance.botSaved.Count);
        if(BotManager.instance.botSaved.Count>num){

            nameText_Status.text = childPanels[BotManager.instance.botSaved[num]].GetChild(0).GetComponent<Text>().text;
            priceText_Status.text = Mathf.RoundToInt((float)BotManager.instance.botInfoList[BotManager.instance.botSaved[num]].price*discount).ToString();
            sellLock.SetActive(false);
        }
        else{
            
            nameText_Status.text = "";
            priceText_Status.text = "";
            sellLock.SetActive(true);
        }
    }
    public void SellBot(){
        tempPre = BotManager.instance.botSaved.Count;
        BotManager.instance.botSaved.RemoveAt(selectedNum);
        botManager.GetChild(selectedNum).GetComponent<BotScript>().DestroyBot();
        PlayerManager.instance.HandleMineral(int.Parse(priceText_Status.text));

        nameText_Status.text = "";
        priceText_Status.text = "";
        sellLock.SetActive(true);

        OpenBotStatusPanel();
    }
    int tempPre;
    public void SelltheSameTypeBot(){
        //List<int> temp = new List<int>();
        for(int i=0;i<BotManager.instance.botSaved.Count;i++){
            if(BotManager.instance.botSaved[i]==BotManager.instance.botSaved[selectedNum]){
                
                //BotManager.instance.botSaved.RemoveAt(i);
                botManager.GetChild(i).GetComponent<BotScript>().DestroyBot();
                PlayerManager.instance.HandleMineral(int.Parse(priceText_Status.text));
                //temp.Add(i);
            }
        }
        tempPre = BotManager.instance.botSaved.Count;
        Debug.Log("선택한 로봇 번호"+BotManager.instance.botSaved[selectedNum]);
        Debug.Log("선택한 로봇 개수"+BotManager.instance.botSaved.RemoveAll(delegate (int x){return x==BotManager.instance.botSaved[selectedNum];}));
        

        // for(int i=0;i<BotManager.instance.botSaved.Count;i++){

        // BotManager.instance.botSaved.RemoveAll(delegate (int x){return x==BotManager.instance.botSaved[selectedNum];});
        // }

        //BotManager.instance.botSaved.Remove(n > n.StartWith(BotManager.instance.botSaved[selectedNum]));


        //Debug.Log("같은 갯수 : "+temp.Count);
        
        // for(int i=0;i<temp.Count+1;i++){
        //     //Debug.Log(temp[i]);
        //     BotManager.instance.botSaved.RemoveAt(temp[i]);
        // }
        // temp.Clear();

        //BotManager.instance.botSaved.RemoveAt(selectedNum);
        //botManager.GetChild(selectedNum).GetComponent<BotScript>().DestroyBot();
        //PlayerManager.instance.HandleMineral(int.Parse(priceText_Status.text));

        nameText_Status.text = "";
        priceText_Status.text = "";
        sellLock.SetActive(true);

        OpenBotStatusPanel();
    }
}
