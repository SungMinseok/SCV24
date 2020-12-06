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

    public float totalTime = 86341f; //2 minutes
    [SerializeField] public Slider fuelBar;
    [SerializeField] public Text minText;
    [SerializeField] public Text timerText;
    bool timerBtn;
    int second, minute, hour;

    [Header("센터")]
    public GameObject centerUI;
    [Header("센터")]
    public List<BoxCollider2D> boxes;
    void Start(){
        StartTimer();
    
    
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
        PlayerManager.instance.StopAuto();
        PlayerManager.instance.goToCenter = false;
        //if(PlayerManager.instance.isAuto || PlayerManager.instance.goToCenter) PlayerManager.instance.StopAuto();
        PlayerManager.instance.isAuto = !PlayerManager.instance.isAuto;
    }    
    public void ToggleGoToCenter(){ 
        PlayerManager.instance.StopAuto();
        PlayerManager.instance.isAuto = false;
        //if(PlayerManager.instance.goToCenter||PlayerManager.instance.isAuto) PlayerManager.instance.StopAuto();
        PlayerManager.instance.goToCenter = !PlayerManager.instance.goToCenter;
    }

    public void ChargeFuel(){
        PlayerManager.instance.HandleFuel(1000f);
    }
    public void ChargeFullFuel(){

        //PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel);
        PlayerManager.instance.curFuel = PlayerManager.instance.maxFuel;
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
        totalTime -= Time.deltaTime;
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
        
        //BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        // EdgeCollider2D[] collider2Ds2=FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];
        // CircleCollider2D[] collider2Ds3=FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];
        // PolygonCollider2D[] collider2Ds4=FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];
    
        foreach(BoxCollider2D col in boxes){
            //col.enabled = true;
                col.isTrigger = false;
        }
        boxes.Clear();
        // foreach(EdgeCollider2D col2 in collider2Ds2){
        //     col2.enabled = true;
        // }
        // foreach(CircleCollider2D col3 in collider2Ds3){
        //     col3.enabled = true;
        // }
        // foreach(PolygonCollider2D col4 in collider2Ds4){
        //     col4.enabled = true;
        // }

        // foreach(BoxCollider2D colRe in blocks){
        //     colRe.enabled =false;
        // }
    }

    public void DisableColliders(){
        //boxes.Clear();
        BoxCollider2D[] collider2Ds=FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
        // EdgeCollider2D[] collider2Ds2=FindObjectsOfType(typeof(EdgeCollider2D)) as EdgeCollider2D[];
        // CircleCollider2D[] collider2Ds3=FindObjectsOfType(typeof(CircleCollider2D)) as CircleCollider2D[];
        // PolygonCollider2D[] collider2Ds4=FindObjectsOfType(typeof(PolygonCollider2D)) as PolygonCollider2D[];
    
        foreach(BoxCollider2D col in collider2Ds){
            if(!col.isTrigger){
                boxes.Add(col);
                col.isTrigger = true;
            }
        }
        // foreach(EdgeCollider2D col2 in collider2Ds2){
        //     col2.enabled = false;
        // }
        // foreach(CircleCollider2D col3 in collider2Ds3){
        //     col3.enabled = false;
        // }
        // foreach(PolygonCollider2D col4 in collider2Ds4){
        //     col4.enabled = false;
        // }

        // foreach(BoxCollider2D colRe in blocks){
        //     colRe.enabled =true;
        // }
    }
}
