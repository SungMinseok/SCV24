using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


public class DBManager : MonoBehaviour
{    
    public static DBManager instance;
    public bool ActivateLoad;
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
        public bool helperDone;
        public float curMineral;
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
        public bool[] unlockedNextBuilding;//건물 갯수
        
        //채취로봇
        public List<int> botSaved;

        //업그레이드
        public bool[] unlockedNextUpgrade;//업그레이드 패널 수 만큼, 0,1,2,3/4,5,6,7/8,9,10,11/12,13,14,15

        //팩토리
        public bool[] unlockedNextProduce;//scv종류만큼
        
        //랜덤상자
        public int boxCount;
        //보급
        public int maxPopulation = 5;
        //퀘스트
        //public int[] testArr;
        public int nowPhase;
        public List<int> questOverList;
        //버프
        public float[] remainingCoolTime;
        public float[] remainingDuration;
        public int[] count;
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
        data.unlockedNextBuilding = BuildingManager.instance.unlockedNextBuilding;
 
    //채취로봇
        data.botSaved = BotManager.instance.botSaved;

    //업글
        data.unlockedNextUpgrade = UpgradeManager.instance.unlockedNextUpgrade;
    //팩토리
        data.unlockedNextProduce = FactoryManager.instance.unlockedNextProduce;

    //랜덤상자
        data.boxCount =BuffManager.instance.boxCount;
        

    //보급
        data.maxPopulation = BotManager.instance.maxPopulation;
        
    //퀘스트
        data.nowPhase = QuestManager.instance.nowPhase;
        data.questOverList = QuestManager.instance.questOverList;

    //버프

        for(int i=0;i<BuffManager.instance.buffs.Count;i++){
            data.remainingCoolTime[i] = BuffManager.instance.buffs[i].remainingCoolTime;
            data.remainingDuration[i] = BuffManager.instance.buffs[i].remainingDuration;
            data.count[i] = BuffManager.instance.buffs[i].count;
        }

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

                //Debug.Log(Application.);
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
                if(data.buildTimeCounter==null){//아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.
               // Debug.Log("아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.");
                    data.buildTimeCounter = new float[BuildingManager.instance.buildTimeCounter.Length];
                }
                else if(data.buildTimeCounter.Length!=BuildingManager.instance.buildTimeCounter.Length){//게임내 배열 길이 증가한 경우 저장된값들까지만 로드
                //Debug.Log("게임내 배열 길이 증가한 경우 저장된값들까지만 로드");
                    
                    for(int i=0;i<data.buildTimeCounter.Length;i++){
                        BuildingManager.instance.buildTimeCounter[i] = data.buildTimeCounter[i];
                    }
                    data.buildTimeCounter = new float[BuildingManager.instance.buildTimeCounter.Length];

                }
                else{
                //Debug.Log("그대로 로드");
                    BuildingManager.instance.buildTimeCounter = data.buildTimeCounter;
                }

                //BuildingManager.instance.unlockedNextBuilding = data.unlockedNextBuilding ;
                if(data.unlockedNextBuilding==null){//아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.
                    data.unlockedNextBuilding = new bool[BuildingManager.instance.unlockedNextBuilding.Length];
                }
                else if(data.unlockedNextBuilding.Length!=BuildingManager.instance.unlockedNextBuilding.Length){//게임내 배열 길이 증가한 경우 저장된값들까지만 로드
                    for(int i=0;i<data.unlockedNextBuilding.Length;i++){
                        BuildingManager.instance.unlockedNextBuilding[i] = data.unlockedNextBuilding[i];
                    }
                    data.unlockedNextBuilding = new bool[BuildingManager.instance.unlockedNextBuilding.Length];

                }
                else{                        
                    BuildingManager.instance.unlockedNextBuilding= data.unlockedNextBuilding;

                }
                //채취로봇
                BotManager.instance.botSaved = data.botSaved;

                
                //업글
                if(data.unlockedNextUpgrade!=null) UpgradeManager.instance.unlockedNextUpgrade = data.unlockedNextUpgrade;

                //팩토리
                if(data.unlockedNextProduce!=null) FactoryManager.instance.unlockedNextProduce = data.unlockedNextProduce;

                //랜덤상자
                BuffManager.instance.boxCount = data.boxCount;

                //보급
                BotManager.instance.maxPopulation = data.maxPopulation;

                //퀘스트 //배열 : data는 초기화 하지말고 여기서 널체크 후 배열 길이 만들어주기.
                QuestManager.instance.nowPhase = data.nowPhase;
                if(data.questOverList!=null) QuestManager.instance.questOverList = data.questOverList;

                //버프
                // if(data.remainingCoolTime.Length == BuffManager.instance.buffs.Count){
                //     for(int i=0;i<BuffManager.instance.buffs.Count;i++){
                //         BuffManager.instance.buffs[i].remainingCoolTime = data.remainingCoolTime[i];
                //         BuffManager.instance.buffs[i].remainingDuration = data.remainingDuration[i];
                //         BuffManager.instance.buffs[i].count = data.count[i];
                //     }
                // }
                // else{
                //     data.remainingCoolTime = new float[BuffManager.instance.buffs.Count];
                //     data.remainingDuration = new float[BuffManager.instance.buffs.Count];
                //     data.count = new int[BuffManager.instance.buffs.Count];
                // }
                if(data.remainingCoolTime==null){//아예처음생성시 게임 내 데이터 배열 길이 맞춰줌.
                    data.remainingCoolTime = new float[BuffManager.instance.buffs.Count];
                    data.remainingDuration = new float[BuffManager.instance.buffs.Count];
                    data.count = new int[BuffManager.instance.buffs.Count];
                }
                else if(data.remainingCoolTime.Length!=BuffManager.instance.buffs.Count){//게임내 배열 길이 증가한 경우 저장된값들까지만 로드
                    for(int i=0;i<BuffManager.instance.buffs.Count;i++){
                        BuffManager.instance.buffs[i].remainingCoolTime = data.remainingCoolTime[i];
                        BuffManager.instance.buffs[i].remainingDuration = data.remainingDuration[i];
                        BuffManager.instance.buffs[i].count = data.count[i];
                    }
                    data.remainingCoolTime = new float[BuffManager.instance.buffs.Count];
                    data.remainingDuration = new float[BuffManager.instance.buffs.Count];
                    data.count = new int[BuffManager.instance.buffs.Count];
                }
                else{                    
                    for(int i=0;i<BuffManager.instance.buffs.Count;i++){
                        BuffManager.instance.buffs[i].remainingCoolTime = data.remainingCoolTime[i];
                        BuffManager.instance.buffs[i].remainingDuration = data.remainingDuration[i];
                        BuffManager.instance.buffs[i].count = data.count[i];
                    }
                }
            }


            file.Close();
        }
    }

    public void ResetDB(){
        SettingManager.instance.testMode = true;
        
        PlayerManager.instance.curMineral = 1000000;
        PlayerManager.instance.curRP = 1000000;
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
        
        //버프
        BuffManager.instance.boxCount = 10;

        //보급
        BotManager.instance.maxPopulation = 5;
    }


}
