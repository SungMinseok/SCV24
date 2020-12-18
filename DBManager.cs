using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class DBManager : MonoBehaviour
{    
    public static DBManager instance;
    public bool loadComplete;
    private void Awake()
    {
        instance = this;
    }
    [System.Serializable]   //SL에 필수적 속성 : 직렬화. 컴퓨터가 읽고쓰기 쉽게.
    public class Data{
        //현재 위치, 현재 시간, 현재 미네랄, 현재 연료, 현재 개스, 현재 장비 레벨, 튜토리얼 유무
        public float playerX,playerY,playerZ;
        public float timer; // UI
        // public float mineral;
        // public float fuel;
        // public float gas;
        
        
    [Header("기타 값 ( Save & Load )")]
        public int helperDone;
        public int curMineral;
        public int curRP;
        public float curFuel;


    [Header("장비 단계 ( Save & Load )")]
        public int weldingLevel;
        public int engineLevel;
        public int fuelLevel;
        public int bodyLevel;
        public int weightLevel;

        //건설
        public float[] buildTimeCounter;
        
        //채취로봇
        public List<int> botSaved;

        //업그레이드
        public bool[] unlockedNextUpgrade = new bool[16];//업그레이드 패널 수 만큼, 0,1,2,3/4,5,6,7/8,9,10,11/12,13,14,15

        //팩토리
        public bool[] unlockedNextProduce = new bool[8];//업그레이드 패널 수 만큼, 0,1,2,3/4,5,6,7/8,9,10,11/12,13,14,15
    }
    UIManager theUI;
    PlayerManager thePlayer;
    public Data data;

    bool isPaused = false; 


    public void CallSave(int num){

        theUI=FindObjectOfType<UIManager>();
        thePlayer=FindObjectOfType<PlayerManager>();

        // data.playerX = thePlayer.transform.position.x;
        // data.playerY = thePlayer.transform.position.y;
        // data.playerZ = thePlayer.transform.position.z;

        data.timer = theUI.totalTime;
        
    //[Header("기타 값 ( Save & Load )")]
        data.helperDone = thePlayer.helperDone;
        data.curMineral = thePlayer.curMineral;
        data.curFuel = thePlayer.curFuel;
        data.curRP = thePlayer.curRP;

    //[Header("장비 단계 ( Save & Load )")]
        data.weldingLevel = thePlayer.weldingLevel;
        data.engineLevel = thePlayer.engineLevel;
        data.fuelLevel = thePlayer.fuelLevel;
        data.bodyLevel = thePlayer.bodyLevel;

    //건설
        data.buildTimeCounter = BuildingManager.instance.buildTimeCounter;

    //채취로봇
        data.botSaved = BotManager.instance.botSaved;

    //업글
        data.unlockedNextUpgrade = UpgradeManager.instance.unlockedNextUpgrade;
    //팩토리
        data.unlockedNextProduce = FactoryManager.instance.unlockedNextProduce;
        

        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/SaveFile" + num +".dat");
        FileStream file = File.Create(Application.persistentDataPath + "/SaveFile.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public void CallLoad(int num){
        BinaryFormatter bf = new BinaryFormatter();
        FileInfo fileCheck = new FileInfo(Application.persistentDataPath + "/SaveFile.dat");

        if(fileCheck.Exists){
        FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);
        
            if(file != null && file.Length >0){

                data =(Data)bf.Deserialize(file);

                theUI=FindObjectOfType<UIManager>();
                thePlayer=FindObjectOfType<PlayerManager>();


                // Vector3 vector =new Vector3(data.playerX, data.playerY, data.playerZ);
                // thePlayer.transform.position = vector;

                theUI.totalTime = data.timer;
                
            //[Header("기타 값 ( Save & Load )")]
                thePlayer.helperDone =data.helperDone;
                thePlayer.curMineral =data.curMineral;
                thePlayer.curFuel =data.curFuel;
                thePlayer.curRP =data.curRP;

            //[Header("장비 단계 ( Save & Load )")]
                thePlayer.weldingLevel =data.weldingLevel;
                thePlayer.engineLevel =data.engineLevel;
                thePlayer.fuelLevel =data.fuelLevel;
                thePlayer.bodyLevel =data.bodyLevel;
                thePlayer.weightLevel =data.weightLevel;

                //건설 // 배열은 널체크
                
                if(data.buildTimeCounter!=null) BuildingManager.instance.buildTimeCounter=data.buildTimeCounter ;
                BotManager.instance.botSaved = data.botSaved;

                
                //업글
                if(data.unlockedNextUpgrade!=null) UpgradeManager.instance.unlockedNextUpgrade = data.unlockedNextUpgrade;

                //팩토리
                if(data.unlockedNextProduce!=null) FactoryManager.instance.unlockedNextProduce = data.unlockedNextProduce;
            }


            file.Close();
        }
    }

    public void ResetDB(){
        PlayerManager.instance.curMineral = 1000000;
        PlayerManager.instance.curRP = 10000;
        PlayerManager.instance.curFuel = PlayerManager.instance.defaultFuel;
        PlayerManager.instance.curSpeed = PlayerManager.instance. defaultSpeed;
        PlayerManager.instance.weldingLevel = 1;
        PlayerManager.instance.engineLevel = 1;
        PlayerManager.instance.fuelLevel = 1;
        PlayerManager.instance.bodyLevel = 1;

        //업글관련
        //UpgradeManager.instance.ApplyEquipsLevel();//업글패널 갱신
        UpgradeManager.instance.ResetUpgradePanelUI();//업글패널 초기화
        UpgradeManager.instance.ApplyEquipsLevel();
        PlayerManager.instance.RefreshEquip();//현재 장비 적용
        UpgradeManager.instance.ResetUnlocked();
        
        //건물 관련
        for(int i=0;i<BuildingManager.instance.buildings.Length;i++){
            
            BuildingManager.instance.buildTimeCounter[i] = 0;
            BuildingManager.instance.buildingsInMap[i].transform.GetChild(0).gameObject.SetActive(false);
            BuildingManager.instance.buildingsInMap[i].transform.GetChild(1).gameObject.SetActive(false);
            BuildingManager.instance.isConstructing[i]=false;
        }

        BuildingManager.instance.BuildingStateCheck();
        

        //봇 관련
        BotManager.instance.DestroyAllBot();

        //팩토리
        FactoryManager.instance.ResetData();
    }


}
