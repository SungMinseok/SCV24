using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    // void Awake(){
        
    //     if (instance != null)
    //     {
    //         Destroy(this.gameObject);
    //     }
    //     else
    //     {
    //         DontDestroyOnLoad(this.gameObject);
    //         instance = this;
    //     }
    // }
    public string btnClickSound ;
    public float totalTime = 86341f; //2 minutes
    
    [Header("상단 UI")]
    [SerializeField] public Slider fuelBar;
    public Text fuelPercentText;
    [SerializeField] public Text minText;
    [SerializeField] public Text rpText;
    [SerializeField] public Text timerText;
    [Header("하단 UI")]
    public GameObject buildLock;
    bool timerBtn;
    int second, minute, hour, day;

    [Header("센터")]
    public GameObject centerUI;
    [Header("Bay")]
    public GameObject bayUI;
    public Text[] upgradeTextArr;
    public Text totalLevel;
    [Header("센터")]
    public List<BoxCollider2D> boxes;

    public Texture2D characterTexture2D;

    [Header("빌딩 입장")]
    public GameObject[] buildings;
    public GameObject selectPanel;
    //public SpriteRenderer center;
    
    [Header("UI Bundle ( Fix Camera )")]
    public GameObject[] uis;
    void Start(){
        instance = this;
        //StartTimer();
    //UpdateCharacterTexture();
    }

    // void Update(){
    //     if(timerBtn){
    //         // second = (int)(Time.time - curTime);
                        
    //         // if( second > 59)
    //         // {
    //         //     curTime = Time.time;
    //         //     second = 0;
    //         //     minute++;
               
    //         //     if( minute > 59)
    //         //     {
    //         //         minute = 0;
    //         //         hour++;
    //         //     }
    //         // }
           
    //         // timerText.text = string.Format("{0:00} : {1:00} : {2:00}", hour, minute, second);
    //     }
    // }
    public void ToggleAuto(){ 
        SoundManager.instance.Play("btn1");

        PlayerManager.instance.goTo = false;
        //if(PlayerManager.instance.isAuto || PlayerManager.instance.goToCenter) PlayerManager.instance.StopAuto();
        if(!PlayerManager.instance.isAuto) {
            
            PlayerManager.instance.YesSound();
        }
        else{
            
                PlayerManager.instance.StopAuto();
        }
        PlayerManager.instance.isAuto = !PlayerManager.instance.isAuto;
    }    
    public void TogglegoTo(string where){ 
        SoundManager.instance.Play("btn1");
        PlayerManager.instance.isAuto = false;
        //if(PlayerManager.instance.goToCenter||PlayerManager.instance.isAuto) PlayerManager.instance.StopAuto();
//목적지로 향할 때 버튼 클릭. 1. 같은 목적지 2. 다른 목적지
        if(PlayerManager.instance.goTo){
            if(PlayerManager.instance.destination.name == where){//같은 목적지 버튼일경우 멈춤

                PlayerManager.instance.StopAuto();

                //PlayerManager.instance.goTo = !PlayerManager.instance.goTo;    
            }
            else{
                PlayerManager.instance.orderType = OrderType.Enter;
                PlayerManager.instance.YesSound();
                PlayerManager.instance.destination = GameObject.Find(where).transform;
            }
        }
//목적지 없을 때
        else{
                PlayerManager.instance.StopAuto();
            
                PlayerManager.instance.orderType = OrderType.Enter;
            PlayerManager.instance.YesSound();

            PlayerManager.instance.destination = GameObject.Find(where).transform;
            
        PlayerManager.instance.goTo = !PlayerManager.instance.goTo;    
        }

        
        // PlayerManager.instance.StopAuto();

        // PlayerManager.instance.isAuto = false;
        // //if(PlayerManager.instance.goToCenter||PlayerManager.instance.isAuto) PlayerManager.instance.StopAuto();

        // // if(PlayerManager.instance.destination !=null){
        // //     if(PlayerManager.instance.destination.name == where){//같은 목적지 버튼일경우 멈춤

        // //     }
        // // }

        // if(!PlayerManager.instance.goTo) {
            
        //     PlayerManager.instance.YesSound();

        //     PlayerManager.instance.destination = GameObject.Find(where).transform;

        // }
        // PlayerManager.instance.goTo = !PlayerManager.instance.goTo;
    }

    public void ChargeFuel(){
        PlayerManager.instance.HandleFuel(1000f, false);
    }
    public void ChargeFullFuel(){

        PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel, false);
        //PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel);
        //PlayerManager.instance.curFuel = PlayerManager.instance.maxFuel;
    }

    public void StartTimer(){
        timerBtn = !timerBtn;
    }
 
    private void Update()
    {
        if(timerBtn){
            UpdateTimer(totalTime );
        }

        if(PlayerManager.instance.goTo || BuildingManager.instance.ConstructingCheck()){
            buildLock.SetActive(true);
        }
        else{
            buildLock.SetActive(false);
        }
    }


    // public void UpdateTimer(float totalSeconds)
    // {     
    //     totalTime += Time.deltaTime*20;
    //     day = (int)(totalSeconds / 86400f) + 1;
    //     hour = (int)(totalSeconds / 3600f)%24;
    //     minute = (int)(totalSeconds / 60f)%60;
    //     //second = (int)totalSeconds % 60;
    //     timerText.text = string.Format("<color=red>Day</color> {0} <color=red>/</color> {1:00}:{2:00}", day, hour, minute); // <color=red>Day</color> 1 <color=red>/</color> 24:00
    // }
    public void UpdateTimer(float totalMinutes)
    {     
        totalTime += Time.deltaTime*2;
        day = (int)(totalMinutes / 1440f) + 1;
        hour = (int)(totalMinutes / 60f)%24;
        minute = (int)(totalMinutes)%60;
        //second = (int)totalSeconds % 60;
        timerText.text = string.Format("<color=red>Day</color> {0} <color=red>/</color> {1:00}:{2:00}", day, hour, minute); // <color=red>Day</color> 1 <color=red>/</color> 24:00
    }

/////////////////////////////////////////////센터
    public void ExitCenterBtn(){
        PlayerManager.instance.ExitCenter();
    }

    /////////////////콜라이더 설정
    
    public void EnableColliders(){
        foreach(BoxCollider2D col in boxes){
                col.isTrigger = false;
        }
        boxes.Clear();
    }

    public void DisableColliders(){
        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        foreach(BoxCollider2D col in collider2Ds){
            if(!col.isTrigger){
                boxes.Add(col);
                col.isTrigger = true;
            }
        }
    }
    
    public void ExitGameBtn(){
        Application.Quit();
    }

    public void RefreshEquipUI(){

        // for(int i=0; i<UpgradeManager.instance.upgradeList.Count; i++){
        //     upgradeTextArr[i].text = UpgradeManager.instance.upgradeList[i].te
            
        // }
    }
    public void EnterBuilding(){
        PlayerManager.instance.StopAuto();
        //GameObject temp = GameObject.Find(PlayerManager.instance.enterableBuilding + "Panel");
        for(int i=0; i<buildings.Length; i++){
            if(buildings[i].name == PlayerManager.instance.enterableBuilding + "Panel"){
                buildings[i].SetActive(true);
                //selectPanel.SetActive(false);
                return;        
            }
            
        }
        Debug.Log("없음");
        
    }
    public void ExitBuilding(){
        for(int i=0; i<buildings.Length; i++){
            if(buildings[i].activeSelf){
                buildings[i].SetActive(false);
                //ActivateSelectPanel();
            }
            // if(buildings[i].name == PlayerManager.instance.enterableBuilding + "Panel"){
            //     buildings[i].SetActive(true);
            //     selectPanel.SetActive(false);
                
            // }
        }
    }
    // public void ActivateSelectPanel(Collider2D collision =null){
    //     if(collision!=null){

    //         if((collision.tag == "Building" || collision.tag == "Center") && !selectPanel.activeSelf){
    //             selectPanel.SetActive(true);
    //             PlayerManager.instance.enterableBuilding = collision.name;
    //         }
    //     }
    //     else{
    //         if(!selectPanel.activeSelf){

    //                         selectPanel.SetActive(true);
    //         }
    //     }

    // }
    public bool OnUI(){  //UI켜져있으면 true
        for(int i=0;i<uis.Length;i++){
            if(uis[i].activeSelf){
                return true;
            }
        }
        return false;
    }

}
