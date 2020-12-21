using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [System.Serializable]
// public class UpgradePanelWrite{
//     public string name;
//     public int grade;
// }
[System.Serializable]
[HideInInspector]
public class UpgradePanelInfo{
    public Text name;
    //public Image image;
    public Text level;
    public GameObject locked;
    public UpgradePanelInfo(Text _name/*, Image _image*/, Text _level, GameObject _locked/*, Sprite _image, string _grade, GameObject _locked*/){
        name = _name;
        level = _level;
        //image=_image;
        // grade=_grade;
        // //maxGrade = _maxGrade;
        locked=_locked;
    }
}

[System.Serializable]
public class Upgrade{
    [Header("설명")]
    [TextArea(1,2)]
    public string mainText;
    [Header("적재량")]
    public string desText;
    [Header("적재량 증가량")]
    public float upgradeDelta;//업그레이드 내용 증가량 : 적재량(8)
    [Header("가격 증가량")]
    public int priceDelta;//업그레이드 비용 증가량 : 100
    [Header("최대 레벨")]
    public int maxLevel;//업그레이드 비용 증가량 : 100
    public Upgrade(string _mainText,string _desText, float _upgradeDelta, int _priceDelta, int _maxLevel = 10 ){
        mainText = _mainText;
        desText = _desText;
        upgradeDelta = _upgradeDelta;
        priceDelta = _priceDelta;
        maxLevel = _maxLevel;

        // itemID= _itemID;
        // itemName= _itemName;
        // itemDescription= _itemDes;
        // itemType= _itemType;
        // //itemCount= _itemCount;
        // itemIcon= Resources.Load("ItemIcon/"+_itemID.ToString(), typeof(Sprite)) as Sprite;
        // itemTexture= Resources.Load("ItemIcon/"+_itemID+"_cursor".ToString(), typeof(Texture2D)) as Texture2D;
        // isEE= _isEE;
    }
}
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    int column = 4;//기본 서사 신화 전설
    int row = 4;//용접기 연료 ``
    [Header("기본,서사,전설 최대 레벨")]
    public int maxLevel0 = 20;
    [Header("기본,서사,전설 요구 연구점수")]
    public int[] rpRequirement;


    [Header("상점")]
    public GameObject upgradePanel;
    public GameObject upgradeBtn;
    public Text mainText;   //몸통강화 한번에 더많은 광물 운반합니다
    public Text descriptText;   //적재량
    public Text phaseText;  //단계
    public Text upgradeText;    //8 > 16
    public Text priceText;
    // Start is called before the first frame update

    public int nowPage;

    public int tempLevel = 0;
    float tempDefault = 0;

   // [SerializeField]
    [Header("작성해야할 것")]
    public List<Upgrade> upgradeList = new List<Upgrade>();
    public string[] upgradeNameList;


    //public List<UpgradePanelInfo> upgradePanelInfos = new List<UpgradePanelInfo>();
    public Transform upgradePanel_UI;
    public List<Transform> upgradePanelList_UI;
    public List<UpgradePanelInfo> accessibleUpgradePanelList= new List<UpgradePanelInfo>();
    


    public Upgrade nowUpgradePanel;

    public GameObject unlockCover;
    public bool[] unlockedNextUpgrade = new bool[16];//업그레이드 패널 수 만큼, 0,1,2,3/4,5,6,7/8,9,10,11/12,13,14,15
    public Text unlockRequirementText;
    //public List<bool> testList = new List<bool>(new bool[16]);
    
    void Awake(){
        instance = this;
    }
    void Start()
    
    {
        //if(unlockedNextUpgrade.Length ==0) unlockedNextUpgrade = new bool[16];
        // unlockedNextUpgrade = new bool[16];
        // unlockedNextUpgrade.Initialize();
        //int temp = upgradePanel_UI.childCount;
        for(int i=0;i<upgradePanel_UI.childCount;i++){
            int temp =i;

            upgradePanelList_UI.Add(upgradePanel_UI.GetChild(i));
            upgradePanelList_UI[temp].GetComponent<Button>().onClick.AddListener(()=>ShowUpgradePanel(temp));
            accessibleUpgradePanelList.Add(new UpgradePanelInfo(upgradePanelList_UI[i].GetChild(0).GetComponent<Text>()
            /*,upgradePanelList_UI[i].GetChild(2).GetComponent<Image>()*/
            ,upgradePanelList_UI[i].GetChild(1).GetComponent<Text>()
            ,upgradePanelList_UI[i].GetChild(3).gameObject));

            accessibleUpgradePanelList[temp].locked.GetComponent<Button>().onClick.AddListener(()=>LockedUpgradeBtn(temp));
            accessibleUpgradePanelList[i].name.text = upgradeNameList[i];
        }
        ResetUpgradePanelUI();
        ApplyEquipsLevel();
            // upgradePanelList_UI[0].GetComponent<Button>().onClick.AddListener(delegate { ShowUpgradePanel(0); });
            // upgradePanelList_UI[4].GetComponent<Button>().onClick.AddListener(delegate { ShowUpgradePanel(4); });
        // upgradeList.Add(new Upgrade("-용접기 강화-\n채굴 속도가 증가합니다","채굴속도", 0.1f, 100));
        // upgradeList.Add(new Upgrade("-엔진 강화-\n이동 속도가 증가합니다","이동속도", 0.1f, 100));
        // upgradeList.Add(new Upgrade("-연료통 강화-\n연료통 용량이 증가합니다","용량", 1000, 100));
        // upgradeList.Add(new Upgrade("-몸통 강화-\n한 번에 더 많은 광물을 운반합니다","적재량", 8, 100));
    }

    void BtnTest(int num){
        Debug.Log("test : "+num);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUpgradePanel(int num){//0~15
        // Debug.Log("Num : "+num);
        // Debug.Log("Num/row : "+num/row); num/now 는 어떤 업그레이드냐 : 용접기, 엔진 ~~

        nowPage = num;
        switch(num/row){
            case 0://용접기
                tempLevel = PlayerManager.instance.weldingLevel;
                tempDefault = PlayerManager.instance.defaultWeldingSec;
                break;
            case 1:
                tempLevel = PlayerManager.instance.engineLevel;
                tempDefault = PlayerManager.instance.defaultSpeed;
                break;
            case 2:
                tempLevel = PlayerManager.instance.fuelLevel;
                tempDefault = PlayerManager.instance.defaultFuel;
                break;
            case 3:
                tempLevel = PlayerManager.instance.bodyLevel;
                tempDefault = PlayerManager.instance.defaultCapacity;
                break;
        }
        
        nowUpgradePanel = upgradeList[num/row];

        // if(num%4 == 0){//기본

        // }
        // else if(num%4 == 1){//서사 해제
        //     if(tempLevel == 20){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
                
        //         unlockCover.SetActive(true);
        //     }
        // }
        // else if(num%4 == 2){

        //     if(tempLevel == 40){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
                
        //         unlockCover.SetActive(true);
        //     }
        // }
        // else if(num%4 == 3){

        //     if(tempLevel == 60){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
                
        //         unlockCover.SetActive(true);
        //     }
        // }
        















        mainText.text = nowUpgradePanel.mainText;
        descriptText.text = nowUpgradePanel.desText;



        if(num%4 !=3){//기본 서사 전설
                
            phaseText.text = tempLevel.ToString() + " / " + (maxLevel0*(num%4+1)).ToString() ;
            if(tempLevel<maxLevel0*(num%4+1)){
                    

                upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempDefault+tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;

                priceText.text = string.Format("{0:#,###0}", (tempLevel*nowUpgradePanel.priceDelta));//(tempLevel*nowUpgradePanel.priceDelta).ToString() ;
                upgradeBtn.SetActive(true);

            }
            else
            
            {

                upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString();
                
                priceText.text = "N/A";
                upgradeBtn.SetActive(false);
                
            }
        }
        else{
            
            phaseText.text = tempLevel.ToString() + " / "+((column-1)*maxLevel0 +1).ToString() ;
            if(tempLevel<maxLevel0*(num%4)+1){
                    

                upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempDefault+tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;

                priceText.text = string.Format("{0:#,###0}", (tempLevel*nowUpgradePanel.priceDelta));//(tempLevel*nowUpgradePanel.priceDelta).ToString() ;
                upgradeBtn.SetActive(true);

            }
            else
            
            {

                upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString();
                
                priceText.text = "N/A";
                upgradeBtn.SetActive(false);
                
            }
        }

        //UIManager.instance.upgradeTextArr[num].text = "<color=red>"+tempLevel.ToString()+"</color> / "+nowUpgradePanel.maxLevel.ToString() ;
        RefreshUpgradePanelUI(num/row);
        UIManager.instance.totalLevel.text = "장비 레벨 : "+AddTotalLevel().ToString();
        //UpgradeManager.instance.accessibleUpgradePanelList[num].level.text = "<color=red>"+tempLevel.ToString()+"</color> / "+nowUpgradePanel.maxLevel.ToString() ;

         upgradePanel.SetActive(true);

    }
    public void UpgradeBtn(){
        if(PlayerManager.instance.curMineral >= tempLevel*nowUpgradePanel.priceDelta){
            PlayerManager.instance.HandleMineral(-tempLevel*nowUpgradePanel.priceDelta);

            SoundManager.instance.Play("up");
            switch(nowPage/row){
                case 0 :
                    PlayerManager.instance.weldingLevel++;
                    break;

                case 1 :
                    PlayerManager.instance.engineLevel++;
                    break;

                case 2 :
                    PlayerManager.instance.fuelLevel++;
                    break;

                case 3 :
                    PlayerManager.instance.bodyLevel++;
                    break;

                default :
                    break;
            }
            PlayerManager.instance.RefreshEquip();
            ShowUpgradePanel(nowPage);  //최신화
        }
        else{
            //SoundManager.instance.Play("notenoughmin");
            UIManager.instance.SetPopUp("미네랄이 부족합니다.","notenoughmin");
        }
        

    }
    //장비 레벨 UI에 적용
    public void ApplyEquipsLevel(){
        for(int i=0; i<upgradeList.Count * column; i++){
            ShowUpgradePanel(i);
        }
        UIManager.instance.totalLevel.text = "장비 레벨 : "+AddTotalLevel().ToString();
        upgradePanel.SetActive(false);
    }
//베이 패널 초기화
    public void ResetUpgradePanelUI(){
        //초기화.
        for(int i=0;i<upgradeList.Count;i++){
            for(int j=column*i; j<column*(i+1); j++){

                if(j%4 == 3) accessibleUpgradePanelList[(i+1)*4-1].level.text = "<color=red>0</color> / 1" ;
                else accessibleUpgradePanelList[j].level.text = "<color=red>0</color> / 20" ;

                if(j%4 == 0) accessibleUpgradePanelList[j].locked.SetActive(false);
                else accessibleUpgradePanelList[j].locked.SetActive(true);
                
                upgradePanelList_UI[j].GetComponent<Button>().interactable = true;
            }

        }

        //ApplyEquipsLevel();

    }
//베이 패널 갱신
    public void RefreshUpgradePanelUI(int num){//0, 1, 2, 3


        if(tempLevel<=20){
        }
        else if(tempLevel<=40 && tempLevel>20){
            accessibleUpgradePanelList[num*4].level.text = "<color=red>20</color> / "+maxLevel0.ToString() ;
            accessibleUpgradePanelList[num*4+1].locked.SetActive(false);
            upgradePanelList_UI[num*4].GetComponent<Button>().interactable = false;
        }
        else if(tempLevel<=60 && tempLevel>40){
            accessibleUpgradePanelList[num*4].level.text = "<color=red>20</color> / "+maxLevel0.ToString() ;
            accessibleUpgradePanelList[num*4+1].level.text = "<color=red>20</color> / "+maxLevel0.ToString() ;
            accessibleUpgradePanelList[num*4+1].locked.SetActive(false);
            accessibleUpgradePanelList[num*4+2].locked.SetActive(false);
            upgradePanelList_UI[num*4].GetComponent<Button>().interactable = false;
            upgradePanelList_UI[num*4+1].GetComponent<Button>().interactable = false;
        }
        else{
            accessibleUpgradePanelList[num*4].level.text = "<color=red>20</color> / "+maxLevel0.ToString() ;
            accessibleUpgradePanelList[num*4+1].level.text = "<color=red>20</color> / "+maxLevel0.ToString() ;
            accessibleUpgradePanelList[num*4+2].level.text = "<color=red>20</color> / "+maxLevel0.ToString() ;
            accessibleUpgradePanelList[num*4+1].locked.SetActive(false);
            accessibleUpgradePanelList[num*4+2].locked.SetActive(false);
            accessibleUpgradePanelList[num*4+3].locked.SetActive(false);
            upgradePanelList_UI[num*4].GetComponent<Button>().interactable = false;
            upgradePanelList_UI[num*4+1].GetComponent<Button>().interactable = false;
            upgradePanelList_UI[num*4+2].GetComponent<Button>().interactable = false;
        }

        //잠금해제 적용.
        ApplyUnlocked();


        //현재 레벨 표시를 어디에 할 것인가.
        if(tempLevel%20 == 0 && tempLevel !=0){ //20, 40, 60 일 때

            accessibleUpgradePanelList[(tempLevel-1)/20+num*4].level.text = "<color=red>"+(tempLevel%20+20).ToString()+"</color> / "+maxLevel0.ToString() ;
        }
        else{
            if(((tempLevel-1)/20)%20==3){   //61일때
                accessibleUpgradePanelList[(tempLevel-1)/20+num*4].level.text = "<color=red>"+(tempLevel%20).ToString()+"</color> / 1";

            }
            else{ // 일반 수 일때 1~19, 21~39 // ex 용접기 렙 17 > 0 + 0*4
                accessibleUpgradePanelList[(tempLevel-1)/20+num*4].level.text = "<color=red>"+(tempLevel%20).ToString()+"</color> / "+maxLevel0.ToString() ;

            }
        }

        //업그레이드 초과시 이전 버튼 비활성화
        

    }
//잠긴 업그레이드 버튼 클릭 시
    public int tempLockedNum;
    public void LockedUpgradeBtn(int num){//0 ~ 15
        tempLockedNum = num;        
        
        switch(num/row){
            case 0:
                tempLevel = PlayerManager.instance.weldingLevel;
                tempDefault = PlayerManager.instance.defaultWeldingSec;
                break;
            case 1:
                tempLevel = PlayerManager.instance.engineLevel;
                tempDefault = PlayerManager.instance.defaultSpeed;
                break;
            case 2:
                tempLevel = PlayerManager.instance.fuelLevel;
                tempDefault = PlayerManager.instance.defaultFuel;
                break;
            case 3:
                tempLevel = PlayerManager.instance.bodyLevel;
                tempDefault = PlayerManager.instance.defaultCapacity;
                break;
        }


        // if(num%4 == 1){//서사 해제
        //     unlockRequirementText.text = rpRequirement[0].ToString();
        //     if(tempLevel == 20 && PlayerManager.instance.curRP >= rpRequirement[0]){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
        //         unlockCover.SetActive(true);
        //     }
        // }
        // else if(num%4 == 2){

        //     unlockRequirementText.text = rpRequirement[1].ToString();
        //     if(tempLevel == 40 && PlayerManager.instance.curRP >= rpRequirement[1]){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
        //         unlockCover.SetActive(true);
        //     }
        // }
        // else if(num%4 == 3){

        //     unlockRequirementText.text = rpRequirement[2].ToString();
        //     if(tempLevel == 60 && PlayerManager.instance.curRP >= rpRequirement[2]){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
        //         unlockCover.SetActive(true);
        //     }
        // }

    
        unlockRequirementText.text = string.Format("{0:#,###0}", rpRequirement[(num%4)-1]);//rpRequirement[(num%4)-1].ToString();
        if(tempLevel == (num%4)*20 && PlayerManager.instance.curRP >= rpRequirement[(num%4)-1]){
            unlockCover.SetActive(false);
        }
        else{
            unlockCover.SetActive(true);
        }
    }
    public void UnlockBtn(){
        //if(tempLockedNum%4 == 1){//서사 해제
            if(PlayerManager.instance.curRP >= rpRequirement [tempLockedNum%4-1]){
                PlayerManager.instance.curRP-=rpRequirement [tempLockedNum%4-1];
                accessibleUpgradePanelList[tempLockedNum].locked.SetActive(false);
                unlockedNextUpgrade[tempLockedNum] = true;
            }
            else{

            }
        //     else{
        //     }
        // }
        // else if(tempLockedNum%4 == 2){

        //     if(tempLevel == 40){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
        //         unlockCover.SetActive(true);
        //     }
        // }
        // else if(tempLockedNum%4 == 3){

        //     if(tempLevel == 60){
        //         unlockCover.SetActive(false);
        //     }
        //     else{
        //         unlockCover.SetActive(true);
        //     }
        // }

        //accessibleUpgradePanelList[tempLockedNum].locked.SetActive(false);
    }
    public int AddTotalLevel(){
        int total = 0;
        total = PlayerManager.instance.weldingLevel + PlayerManager.instance.engineLevel + PlayerManager.instance.fuelLevel + PlayerManager.instance.bodyLevel;
        return total;
    }
    public void ApplyUnlocked(){
        for(int i=0;i<unlockedNextUpgrade.Length;i++){
            if(i%4!=0){

                if(unlockedNextUpgrade[i]){
                    accessibleUpgradePanelList[i].locked.SetActive(false);
                }
                else{
                    accessibleUpgradePanelList[i].locked.SetActive(true);
                }
            }
        }
    }
    public void ResetUnlocked(){
        unlockedNextUpgrade = new bool[16];
        for(int i=0;i<unlockedNextUpgrade.Length;i++){
            if(i%4!=0){
                accessibleUpgradePanelList[i].locked.SetActive(true);
                unlockedNextUpgrade[i] = false;
                
            }
        }
    }
}
