using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    void Awake(){
        
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    public string btnClickSound ;
    public float totalTime = 86341f; //2 minutes
    [SerializeField] public Slider fuelBar;
    [SerializeField] public Text minText;
    [SerializeField] public Text timerText;
    bool timerBtn;
    int second, minute, hour;

    [Header("센터")]
    public GameObject centerUI;
    [Header("Bay")]
    public GameObject bayUI;
    public Text[] upgradeTextArr;
    [Header("센터")]
    public List<BoxCollider2D> boxes;

    public Texture2D characterTexture2D;
    //public SpriteRenderer center;
    
    void Start(){
        StartTimer();
    
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

        PlayerManager.instance.goTo = !PlayerManager.instance.goTo;    
            }
            else{
                
                PlayerManager.instance.YesSound();
                PlayerManager.instance.destination = GameObject.Find(where).transform;
            }
        }
//목적지 없을 때
        else{
                PlayerManager.instance.StopAuto();
            
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
        UpdateTimer(totalTime );
    }


    public void UpdateTimer(float totalSeconds)
    {     
        totalTime += Time.deltaTime;
        hour = (int)(totalSeconds / 3600f);
        minute = (int)(totalSeconds / 60f)%60;
        second = (int)totalSeconds % 60;
        timerText.text = string.Format("{0:00} : {1:00} : {2:00}", hour, minute, second);
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
}
