using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class QuestInfo{
    [Header("퀘스트 이름")]
    public string questMain;
    [Header("퀘스트 요약")]
    [TextArea(1,2)]
    public string questSum;
    [Header("퀘스트 맵화살표")]
    public GameObject questArrow;
}
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public QuestInfo[] questList_GET;//받아오기용
    public QuestInfo[] questList;//순서대로 저장용
    [Header("퀘스트 순서")]
    public int[] questOrder;
    [Header("퀘스트 UI")]
    public GameObject questPanel;
    public Text questText;
    [Header("DB 저장")]
    public int nowPhase;
    public List<int> questOverList = new List<int>();

    void Awake(){
        instance = this;
        //questList_GET = new QuestInfo[questOrder.Length];
        questList = new QuestInfo[questOrder.Length];
        for(int i=0; i<questOrder.Length; i++){
            questList[i]=questList_GET[questOrder[i]];
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetQuest(nowPhase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitTutorial(){
        StopAllCoroutines();
        UIManager.instance.alertPopup.SetActive(false);
        questPanel.SetActive(false);
    }
    public void SetQuest(int phase){
        StartCoroutine(QuestCoroutine(phase));
    }
    IEnumerator QuestCoroutine(int phase){
        yield return null;
        nowPhase = phase;
        SoundManager.instance.Play("rescue");

        questText.text = questList[phase].questMain;
        UIManager.instance.SetPopUp(questList[phase].questSum,"none");
        if(questList[phase].questArrow!=null) questList[phase].questArrow.SetActive(true);

        questPanel.SetActive(true);
        UIManager.instance.alertPopup.GetComponent<Animator>().speed = 0f;
        //화살표, 알림창 삭제 조건
        switch(phase){
            case 0 : 
                yield return new WaitUntil(()=> CameraMovement.instance.isMoving );
                //yield return new WaitForSeconds(2f);
                break;
            case 1 : 
                yield return new WaitUntil(()=> PlayerManager.instance.isAuto );
                break;
            case 2 : 
                yield return new WaitUntil(()=> PlayerManager.instance.curFuel >=500 );
                break;
            case 3 : 
                yield return new WaitUntil(()=> PlayerManager.instance.curMineral >=24 );
                break;
            case 4 : //보급고 건설 버튼 클릭
                yield return new WaitUntil(()=> PlayerManager.instance.orderType == OrderType.Build );
                break;
            case 6 :

            // case 6 : //
            //     yield return new WaitUntil(()=> PlayerManager.instance.curMineral >=24);
            //     break;
            // case 7 : //보급고 건설 버튼 클릭
            //     yield return new WaitUntil(()=> PlayerManager.instance.orderType == OrderType.Build );
            //     break;                
            default : 
                break;
        }

        if(questList[phase].questArrow!=null) questList[phase].questArrow.SetActive(false);
        UIManager.instance.alertPopup.GetComponent<Animator>().speed = 1f;

        //퀘스트 완료 조건
        switch(phase){
            case 0 : 
                //yield return new WaitUntil(()=> CameraMovement.instance.isMoving );
                yield return new WaitForSeconds(2f);
                break;
            case 1 : 
                yield return new WaitUntil(()=> PlayerManager.instance.curMineral ==8 );
                break;

            case 2 : 
                yield return new WaitUntil(()=> !UIManager.instance.OnUI() );//센터나감
                break;

            case 3 : 
                yield return new WaitUntil(()=> PlayerManager.instance.curMineral >=100 );
                break;//SetActive(true);            
            case 4 : //보급고 w중
                //yield return new WaitUntil(()=> BuildingManager.instance.buildingsInMap[0].transform.GetChild(1).gameObject.activeSelf);
                yield return new WaitUntil(()=> BuildingManager.instance.isConstructing[0]);
                break;            
            case 5 : //보급고 w중
                yield return new WaitUntil(()=> BuildingManager.instance.buildingsInMap[0].transform.GetChild(1).gameObject.activeSelf);
                //yield return new WaitUntil(()=> BuildingManager.instance.isConstructing[0]);
                break;            
            // case 6 : //보급고 w중
            //     yield return new WaitUntil(()=> PlayerManager.instance.curMineral >=200 );
            //     //yield return new WaitUntil(()=> BuildingManager.instance.isConstructing[0]);
            //     break;
            // case 7 : //보급고 w중
            //     //yield return new WaitUntil(()=> BuildingManager.instance.buildingsInMap[0].transform.GetChild(1).gameObject.activeSelf);
            //     yield return new WaitUntil(()=> BuildingManager.instance.isConstructing[0]);
            //     break;            
            default : 
                break;
        }

        FinishQuest(phase);

        //퀘스트 완료 후
        switch(phase){
            case 0 : 
                SetQuest(1);
                break;
            case 1 : 
                SetQuest(2);
                break;
            case 2 : 
                SetQuest(3);
                break;
            case 3 : 
                SetQuest(4);
                break;
            case 4 : 
                SetQuest(5);
                break;
            case 5 : 
                FactoryManager.instance.ProduceInTuto();
                UIManager.instance.alertPopup.GetComponent<Animator>().speed = 0f;
                UIManager.instance.SetPopUp("보급고 건설 보너스로 꼬마 scv를 드립니다.\n꼬마 SCV의 효율은 본체 SCV의 효율에 비례합니다.");
                yield return new WaitForSeconds(5f);
                UIManager.instance.alertPopup.GetComponent<Animator>().speed = 1f;
                SetQuest(6);
                break;
                
            // case 6 : 
            //     SetQuest(7);
            //     break;
            default : 
                
                SoundManager.instance.Play("rescue");
                break;
        }
    }
    public void FinishQuest(int phase){
        questPanel.SetActive(false);
        if(!questOverList.Contains(phase)) questOverList.Add(phase);

        
        nowPhase ++;
    }
    // public void Quest(){
    //     if()
    // }
}
