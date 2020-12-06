using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("상점")]
    public GameObject upgradePanel;
    public Text mainText;   //몸통강화 한번에 더많은 광물 운반합니다
    public Text descriptText;   //적재량
    public Text phaseText;  //단계
    public Text upgradeText;    //8 > 16
    public Text priceText;
    // Start is called before the first frame update

    public int nowPage;

   // [SerializeField]
    public List<Upgrade> upgradeList = new List<Upgrade>();
    public Upgrade nowUpgradePanel;
    void Start()
    {
        
        // upgradeList.Add(new Upgrade("-용접기 강화-\n채굴 속도가 증가합니다","채굴속도", 0.1f, 100));
        // upgradeList.Add(new Upgrade("-엔진 강화-\n이동 속도가 증가합니다","이동속도", 0.1f, 100));
        // upgradeList.Add(new Upgrade("-연료통 강화-\n연료통 용량이 증가합니다","용량", 1000, 100));
        // upgradeList.Add(new Upgrade("-몸통 강화-\n한 번에 더 많은 광물을 운반합니다","적재량", 8, 100));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUpgradePanel(int num){
        nowPage = num;
        int tempLevel = 0;
        float tempDefault = 0;
        switch(num){
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
        
        nowUpgradePanel = upgradeList[num];

        if(tempLevel<nowUpgradePanel.maxLevel){
                
            mainText.text = nowUpgradePanel.mainText;
            descriptText.text = nowUpgradePanel.desText;
            phaseText.text = tempLevel.ToString() + " > " + (tempLevel+1).ToString() ;

            upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempDefault+tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;
            //if(nowPage==0) upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempDefault+tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;
            // else if(nowPage==1) upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempDefault+tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;
            // else if(nowPage==2) upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempDefault+tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;
            // else if(nowPage==3) upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString() + " >> " + (tempLevel*nowUpgradePanel.upgradeDelta).ToString() ;
            
            priceText.text = (tempLevel*nowUpgradePanel.priceDelta).ToString() ;

        }
        else{
            
            mainText.text = nowUpgradePanel.mainText;
            descriptText.text = nowUpgradePanel.desText;
            phaseText.text = tempLevel.ToString();

            upgradeText.text = (tempDefault+(tempLevel-1)*nowUpgradePanel.upgradeDelta).ToString();
            
            priceText.text = "";
        }

        upgradePanel.SetActive(true);
    }
    public void UpgradeBtn(){
        switch(nowPage){
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
}
