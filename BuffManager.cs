using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Buff{
    public string name;
    public float time;

    public Buff(string _name, float _time){
        name = _name;
        time = _time;
    }

    
}
public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public int boxCount;
    public Text boxCountText;
    public List<Buff> buffs = new List<Buff>();
    // Start is called before the first frame update
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
    void Awake(){
        instance = this;
    }
    void Start()
    {
        //CreateDropship();
        boxCountText.text = "x " + boxCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateBuff(string name){
        for(int i=0;i<buffs.Count;i++){
            if(buffs[i].name == name){
                StartCoroutine(BuffCoroutine(buffs[i]));
            }
        }
    }

    IEnumerator BuffCoroutine(Buff buff){
        centerBuffBtn.interactable = false;
        centerBuffImg.gameObject.SetActive(true);
        centerBuffText.text = buff.time.ToString();
        float tempTime = buff.time;


        while(tempTime>0f){  
            
            // buff.time--;

            
            // centerBuffText.text = buff.time.ToString();
            // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
            // Debug.Log(centerBuffImg.fillAmount);
            // yield return new WaitForSeconds(1f);

            tempTime -= Time.deltaTime;
            
            centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
            centerBuffImg.fillAmount = tempTime / buff.time ;
            yield return new WaitForFixedUpdate();
        }

        centerBuffBtn.interactable = true;
        centerBuffImg.gameObject.SetActive(false);


    }
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

}
