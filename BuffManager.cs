using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Buff{
    public string name;
    public float coolTime;
    public float duration;
    //public int count;//DB저장
    public Transform btn;
    public Transform buffImage;//상단UI표시
    public float remainingCoolTime;
    public float remainingDuration;
    public int count;//DB저장
    //public int remainingCount;
    
}
public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public int boxCount;
    public Text boxCountText;
    public Transform botManager;
    [SerializeField]
    public List<Buff> buffs = new List<Buff>();
    // Start is called before the first frame update
    //public List<float> coolTimeCounter = new List<float>();
    //public List<float> durationTimeCounter = new List<float>();
    // public float[] coolTimeCounter = new float[1];
    // public float[] durationTimeCounter = new float[1];
    // public int[] countCounter = new int[1];
    public Image centerBuffImg;
    public Text centerBuffText;
    public Button centerBuffBtn;
    [Header("랜덤박스 UI")]
    public GameObject randomBoxPanel;
    public Text randomAmountText;
    public GameObject randomOk;
    public Image randomImage;
    public Sprite mineral;
    public Sprite rp;
    public GameObject recallEffect;
    [Header("드랍쉽")]
    public GameObject dropship;
    public GameObject dropbox;
    public BoxCollider2D mapBound;
    public float dropshipSpeed = 5f;
    [Header("RP")]
    public BoxCollider2D[] createArea;
    public float rpCoolTime;
    public int maxRP;
    public GameObject rpBox;
    public Transform rpParent;
    
    [Header("오토모드")]
    public bool autoChargeFuel;
    private bool autoCharging;
    void Awake(){
        instance = this;
    }
    void Start()
    {
        //CreateDropship();
        boxCountText.text = "x " + boxCount.ToString();

        for(int i=0; i< buffs.Count;i++){
            var temp = buffs[i].btn.GetChild(buffs[i].btn.childCount-1);
            if(temp.gameObject.name == "LeftCount"){//카운트가 있는 경우(스팀팩)
                temp.GetComponent<Text>().text = buffs[i].count.ToString();
                    
                    //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).GetComponent<Text>().text
            }
        }

        StartCoroutine(CreateRandomRPCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateBuff(string name){
        for(int i=0;i<buffs.Count;i++){
            if(buffs[i].name == name){
                //var temp = buffs[i].btn.GetChild(buffs[i].btn.childCount-1);
                //if(temp.gameObject.name == "LeftCount"){//카운트가 있는 경우(스팀팩)
                if(buffs[i].count > 0){

                    buffs[i].btn.GetChild(buffs[i].btn.childCount-1).GetComponent<Text>().text = (--buffs[i].count).ToString();
                    StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
                    StartCoroutine(BuffCoroutine(buffs[i]));
                }
                else{

                }
                    
                        //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).GetComponent<Text>().text
                // }
                // else{

                //     StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
                // }
            }
        }
    }

    IEnumerator BuffCoolTimeCoroutine(Buff buff){
        buff.btn.GetComponent<Button>().interactable = false;
        var coolTimeImage = buff.btn.GetChild(buff.btn.childCount-2).GetComponent<Image>();
        var coolTimeText = buff.btn.GetChild(buff.btn.childCount-2).transform.GetChild(0).GetComponent<Text>();
        //buff.btn.GetComponent<Button>().interactable = false;//centerBuffBtn.interactable = false;
        coolTimeImage.gameObject.SetActive(true);//centerBuffImg.gameObject.SetActive(true);
        coolTimeText.text = buff.coolTime.ToString();//centerBuffText.text = buff.time.ToString();
        
        

        while(buff.remainingCoolTime>0f){  
            
            // buff.time--;

           // Debug.Log("TempTime : "+ tempTime);
            // centerBuffText.text = buff.time.ToString();
            // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
            // Debug.Log(centerBuffImg.fillAmount);
            // yield return new WaitForSeconds(1f);

            buff.remainingCoolTime -= Time.deltaTime;
            
            coolTimeText.text = (Mathf.CeilToInt(buff.remainingCoolTime)).ToString();//centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
            coolTimeImage.fillAmount = buff.remainingCoolTime / buff.coolTime ;
            yield return new WaitForFixedUpdate();
        }

        //centerBuffBtn.interactable = true;
        buff.btn.GetComponent<Button>().interactable = true;
        coolTimeImage.gameObject.SetActive(false);//centerBuffImg.gameObject.SetActive(false);

        buff.remainingCoolTime = buff.coolTime;
    }    
    IEnumerator BuffCoroutine(Buff buff){
        buff.buffImage.gameObject.SetActive(true);
        // buff.btn.GetComponent<Button>().interactable = false;
        var coolTimeImage = buff.buffImage.GetChild(0).GetComponent<Image>();
        // var coolTimeText = buff.btn.GetChild(buff.btn.childCount-2).transform.GetChild(0).GetComponent<Text>();
        // //buff.btn.GetComponent<Button>().interactable = false;//centerBuffBtn.interactable = false;
        // coolTimeImage.gameObject.SetActive(true);//centerBuffImg.gameObject.SetActive(true);
        // coolTimeText.text = buff.coolTime.ToString();//centerBuffText.text = buff.time.ToString();
        // float tempTime = buff.coolTime;
        Debug.Log("버프코루틴 시작");
        switch(buff.name){
            case "Stimpack0" : 
                SetBoosterColor("red");
                SetSCVSpeed(1.5f);
                break;
            case "Stimpack1" : 
                SetMineralSize(1.5f);
                break;

            default :
                break;
        }







        
        while(buff.remainingDuration>0f){  
            
            // buff.time--;

            //Debug.Log("TempTime : "+ tempTime);
            // centerBuffText.text = buff.time.ToString();
            // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
            // Debug.Log(centerBuffImg.fillAmount);
            // yield return new WaitForSeconds(1f);

            buff.remainingDuration -= Time.deltaTime;
            
            //coolTimeText.text = (Mathf.CeilToInt(tempTime)).ToString();//centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
            coolTimeImage.fillAmount = (buff.duration-buff.remainingDuration) / buff.duration ;
            yield return new WaitForFixedUpdate();
        }

        //centerBuffBtn.interactable = true;
        //buff.btn.GetComponent<Button>().interactable = true;
        buff.buffImage.gameObject.SetActive(false);//centerBuffImg.gameObject.SetActive(false);

        buff.remainingDuration = buff.duration;


        switch(buff.name){
            case "Stimpack0" : 
                SetBoosterColor();
                SetSCVSpeed();
                break;
            case "Stimpack1" : 
                SetMineralSize();
                break;

            default :
                break;
        }


    }
    // IEnumerator NewBuffCoroutine(Buff buff){ // 버프 중첩용.
    //     buff.buffImage.gameObject.SetActive(true);
    //     var coolTimeImage = buff.buffImage.GetChild(0).GetComponent<Image>();
    //     Debug.Log("버프코루틴 시작");
    //     switch(buff.name){
    //         case "Stimpack" : 
    //             SetBoosterColor("red");
    //             SetSCVSpeed(1.5f);
    //             break;
    //         default :
    //             break;
    //     }
    //     while(buff.remainingDuration>0f){  
    //         buff.remainingDuration -= Time.deltaTime;
    //         coolTimeImage.fillAmount = (buff.duration-buff.remainingDuration) / buff.duration ;
    //         yield return new WaitForFixedUpdate();
    //     }
    //     buff.buffImage.gameObject.SetActive(false);

    //     buff.remainingDuration = buff.duration;


    //     switch(buff.name){
    //         case "Stimpack" : 
    //             SetBoosterColor();
    //             SetSCVSpeed();
    //             break;

    //         default :
    //             break;
    //     }


    // }
    int ranType;
    int ranNum;
    public void SetDailyRandomBox(){    // 일일 랜덤상자 클릭
        ActivateBuff("Center");
        recallEffect.SetActive(true);
        randomBoxPanel.SetActive(true);
        SoundManager.instance.Play("recall");
        ranType = Random.Range(0,2);//0,1
        
        if(ranType == 0){ //미네랄 50000~100000
            
            ranNum = Random.Range(50,100);
            randomImage.sprite = mineral;
            randomAmountText.text = (ranNum * 1000).ToString();
        }
        else if(ranType == 1){//연구점수 200~2000

            ranNum = Random.Range(10,100); 
            randomImage.sprite = rp;
            randomAmountText.text = (ranNum * 20).ToString();
        }
        

        Invoke("DelayOkBtn",0.7f);
    }    
    public void SetRandomBox(){ //획득 가능 랜덤 상자 클릭
        if(boxCount >0){
            //ActivateBuff("Center");
        recallEffect.SetActive(true);
        randomBoxPanel.SetActive(true);
            //boxCount --;
            boxCountText.text = "x "+(--boxCount).ToString();
                
            SoundManager.instance.Play("recall");
            ranType = Random.Range(0,2);//0,1
            
            if(ranType == 0){ //미네랄 50000~100000
                
                ranNum = Random.Range(50,100);
                randomImage.sprite = mineral;
                randomAmountText.text = (ranNum * 1000).ToString();
            }
            else if(ranType == 1){//연구점수 200~2000

                ranNum = Random.Range(10,100); 
                randomImage.sprite = rp;
                randomAmountText.text = (ranNum * 20).ToString();
            }
            

            Invoke("DelayOkBtn",0.7f);
        }
    }
    public void DelayOkBtn(){
        SoundManager.instance.Play("rescue");
        randomOk.SetActive(true);
    }
    public void GetRandomBox(){ //확인 클릭
        if(ranType ==0 ){

            PlayerManager.instance.HandleMineral(int.Parse(randomAmountText.text));
        }
        else if(ranType ==1){
            PlayerManager.instance.HandleRP(int.Parse(randomAmountText.text));

        }
    }

    public void CreateDropship(){
        StartCoroutine(DropshipMovementCoroutine());
    }

    IEnumerator DropshipMovementCoroutine(){
        SoundManager.instance.Play("dropship");

        dropship.SetActive(true);
        bool itemFlag = false;

        //var pos = SettingManager.instance.screenSize/2;
        Vector2 mainPos = CameraMovement.instance.transform.position;
        
        Vector2 mapBoundPos = new Vector2(mapBound.bounds.size.x/2f + 2f,Random.Range(mapBound.bounds.min.y + 2f,mapBound.bounds.max.y - 2f));
        
       //Debug.Log(SettingManager.instance.screenSize);
        Vector2 tempPos = Vector2.zero;
        //드랍쉽 생성 위치
        //Vector2 dropshipPos = new Vector2(10f,Random.Range(-2f,2f));
        //박스 생성 위치
        float boxPosX =  Random.Range(mapBound.bounds.min.x +2f, mapBound.bounds.max.x -2f); //new Vector2(mainPos.x + Random.Range(-4f,4f), mainPos.y + Random.Range(-1.5f,1.5f));
        //
        Debug.Log(boxPosX);
        int ranNum0 = Random.Range(0,2);
        if(ranNum0==0){ //왼쪽등장
            dropship.GetComponent<SpriteRenderer>().flipX = false;
            dropship.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
            dropship.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = false;
            //dropship.transform.GetChild(1).transform.local = false;
            dropship.transform.position = -mapBoundPos;//new Vector2(pos.x-10,Random.Range(pos.y-2,pos.y+2));
            tempPos = dropship.transform.position;
            //destination.transform.position = new Vector2(-pos.x-300,Random.Range(-pos.y,+pos.y));
        }
        else{
            dropship.GetComponent<SpriteRenderer>().flipX = true;
            dropship.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
            dropship.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
            dropship.transform.position = mapBoundPos;
            //dropship.transform.position = new Vector2(pos.x+10,Random.Range(pos.y-2,pos.y+2));
            tempPos = dropship.transform.position;
        }

        Debug.Log("드랍쉽생성성공");
        //Debug.Log("드랍쉽 위치 : "+dropship.transform.position.x+"목적지 위치 : "+ (-tempPos.x));






        while((int)dropship.transform.position.x != (int)(-tempPos.x)){
        Debug.Log("이동중");
            dropship.transform.Translate((ranNum0 == 0 ? Vector3.right : Vector3.left) * Time.deltaTime * dropshipSpeed);

            if(!itemFlag&&(int)dropship.transform.position.x == (int)boxPosX){
                Debug.Log("아이템 드랍");
                
        SoundManager.instance.Play("drop");
                var clone = Instantiate(dropbox, dropship.transform.position , Quaternion.identity);

                itemFlag= true;
            }
            
                        yield return new WaitForFixedUpdate();

        }
        Debug.Log("이동완료후 제거");

        dropship.SetActive(false);
        itemFlag= false;


        //yield return null;
    }

    public void GetDropBox(GameObject box){
        //box.GetComponent<SpriteButton>().DestroySprite();
        Debug.Log("상자 삭제");
        SoundManager.instance.Play("rescue");

        boxCountText.text = "x "+(++boxCount).ToString();

        Destroy(box.transform.parent.gameObject);
    }

    //버프 남은 시간 세이브/로드
    public void CheckBuffState(){
        UIManager.instance.ActivateLowerUIPanel(3);
        UIManager.instance.ActivateLowerUIPanel(4);

        for(int i=0;i<buffs.Count;i++){
            if(buffs[i].remainingCoolTime!=buffs[i].coolTime){
                StartCoroutine(BuffCoolTimeCoroutine(buffs[i]));
            }
            else{
                buffs[i].btn.GetChild(buffs[i].btn.childCount-2).gameObject.SetActive(false);
                //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).GetComponent<Image>().fillAmount = 0;
                //buffs[i].btn.GetChild(buffs[i].btn.childCount-2).transform.GetChild(0).GetComponent<Text>().text ;
            }
            if(buffs[i].remainingDuration!=buffs[i].duration){
                StartCoroutine(BuffCoroutine(buffs[i]));
            }
            else{
                
                buffs[i].buffImage.gameObject.SetActive(false);
            }
        }
    }
    public void SetBoosterColor(string _color = "default"){
        //if(buffs[i].remainingDuration!=buffs[i].duration){
            Debug.Log("부스터 색 변경 > "+_color);
        for(int i=0;i<PlayerManager.instance.boosters.Length;i++){
            switch(_color){
                case "red" : 
            //Debug.Log("f");
                    PlayerManager.instance.boosters[i].color = Color.red;
                    break;
                default :
                    PlayerManager.instance.boosters[i].color = new Color(1,1,1,1);
                    break;
            }
        }
        
        Color newColor = PlayerManager.instance.boosters[0].color;

        if(BotManager.instance.botSaved.Count!=0){
            for(int i=0;i<BotManager.instance.botSaved.Count;i++){
                //Debug.Log(BotManager.instance.botSaved)
                var temp = botManager.GetChild(i).GetComponent<BotScript>();
                for(int j=0;j<3;j++){
                    Debug.Log(i+"번 봇 "+j+"번 째 색변경 성공");
                    //botManager.GetChild(i).GetComponent<BotScript>().booster.gameObject.SetActive(true);
                    temp.boosters[j].color = newColor;
                    //botManager.GetChild(i).GetComponent<BotScript>().booster.gameObject.SetActive(false);
                    if(_color != "default"){

                        temp.booster.gameObject.SetActive(true);
                    }
                    else{
                        temp.booster.gameObject.SetActive(false);
                        
                    }
                    
                    // switch(_color){
                    //     case "red" : 
                    //         PlayerManager.instance.boosters[i].color = Color.red;
                    //         break;
                    //     default :
                    //         PlayerManager.instance.boosters[i].color = new Color(1,1,1,1);
                    //         break;
                    // }
                    
                }
                // switch(_color){
                //     case "red" : 
                //         PlayerManager.instance.boosters[i].color = Color.red;
                //         break;
                //     default :
                //         PlayerManager.instance.boosters[i].color = new Color(1,1,1,1);
                //         break;
                // }
            }
        }
    }
    public void SetSCVSpeed(float amount = 1){
        PlayerManager.instance.bonusSpeed = amount;
    }

    public IEnumerator CreateRandomRPCoroutine(){
        yield return new WaitForSeconds(rpCoolTime);
        //var temp = mapBound.bounds;
        if(rpParent.childCount < maxRP){

            var temp = createArea[Random.Range(0,createArea.Length)].bounds;
            Vector3 ranPos =  new Vector3( Random.Range(temp.min.x, temp.max.x ),Random.Range(temp.min.y , temp.max.y ),0);
        
            var clone = Instantiate(rpBox, ranPos , Quaternion.identity);
            clone.transform.SetParent(rpParent);
            Debug.Log("RP랜덤생성");
        }
        

        StartCoroutine(CreateRandomRPCoroutine());

    }    
    public void SetMineralSize(float amount = 1){
        PlayerManager.instance.mineral.transform.localScale = new Vector2(amount,amount);
        PlayerManager.instance.bonusCapacity = amount;
        if(BotManager.instance.botSaved.Count!=0){
            for(int i=0;i<BotManager.instance.botSaved.Count;i++){
                var temp = botManager.GetChild(i).GetComponent<BotScript>();
                for(int j=0;j<3;j++){
                    temp.mineral.transform.localScale =  new Vector2(amount,amount);
                }
            }
        }
    }

    public void AutoChargeFuel(){
        if(!autoCharging){
            autoCharging = true;
            StartCoroutine(AutoChargeFuelCoroutine());
        }
    }

    IEnumerator AutoChargeFuelCoroutine(){
        Debug.Log("오토차징");
        while(PlayerManager.instance.curFuel/PlayerManager.instance.maxFuel < 0.99f){

            PlayerManager.instance.HandleFuel(PlayerManager.instance.maxFuel * 0.075f);

            yield return new WaitForFixedUpdate();
        }
        autoCharging = false;
    }
}
