using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotScript : MonoBehaviour
{
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public SpriteRenderer sr;
    private Vector2 defaultSide;
    [HideInInspector]public Vector2 movement;
    [HideInInspector]public Vector2 movementDirection;
    [HideInInspector]public bool canMove;
    [HideInInspector]public bool isAlive;
    [HideInInspector]public bool isHolding;//뭔가 들고 있을 때
    [HideInInspector]public bool isMining;//채취 중일 때
    [SerializeField] public UnitType type;
    [SerializeField] public ShadowType shadowType;
    [SerializeField] public PackageType packageType = PackageType.none;
    private Animator animator;
    public bool gotMine;//미네랄 발견
    public bool gotDestination;//센터 발견
    public Transform destination;
    public float curSpeed = 2f;
    public float weldingSec = 2f;
    public GameObject workLight;
    public GameObject mineral;
    GameObject booster;
    private SpriteRenderer[] boosters;
    bool miningFlag;
    public GameObject floatingText;
    public GameObject floatingCanvas;
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();        
        
        if(shadowType==ShadowType.booster){
            
            booster = transform.Find("Booster").gameObject;
            booster.SetActive(false);
            boosters = new SpriteRenderer[3];
            for(int i=0;i<3;i++){
                //Debug.Log(booster.transform.childCount);
                //Debug.Log(booster.transform.GetChild(i).name);
                boosters[i] = booster.transform.GetChild(i).GetComponent<SpriteRenderer>();
            }
            defaultSide = boosters[1].GetComponent<RectTransform>().localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
            if(!isHolding&&!isMining){
                destination = GameObject.FindWithTag("Mineral Field").transform;
                
                if(gotMine){
                    gotMine = false;
                    isMining = true;
                    destination = null;

                }
                else if(Mathf.Abs(transform.position.y - destination.position.y)>=0.1f){
                    if(transform.position.y > destination.position.y){
                        movement = new Vector2(0,-1);
                    }
                    else{
                        movement = new Vector2(0,1);

                    }
                    sr.flipX = false;
                        SetBooster("UPDOWN");
                    rb.MovePosition(rb.position + movement * curSpeed * Time.fixedDeltaTime);

                    
                }
                else if(Mathf.Abs(transform.position.x - destination.position.x)>=0.1f){

                    if(transform.position.x > destination.position.x){
                        movement = new Vector2(-1,0);
                    }
                    else{
                        movement = new Vector2(1,0);

                    }
                    sr.flipX = animator.GetFloat("Horizontal") < 0 ? true : false;

                        SetBooster("LEFTRIGHT"); 
                    rb.MovePosition(rb.position + movement * curSpeed * Time.fixedDeltaTime);

                }
            }
            else if(isMining){
                animator.SetBool("Stop", true);
                workLight.SetActive(true);                    
                if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Att")){
                        
                        animator.SetTrigger("Att");
                    }
                if(!miningFlag){
                    miningFlag = true;
                    StartCoroutine(MiningCoroutine());
                } 
            }
            else if(isHolding&&!isMining){
                destination = GameObject.FindWithTag("Center").transform;
                if(gotDestination){
                        HandleMineral();
                    gotDestination = false;
                    isHolding = false;
                    destination = null;
                    mineral.gameObject.SetActive(false);
                    //packageType = PackageType.none;
                }
                else if(Mathf.Abs(transform.position.y - destination.position.y)>=
                destination.GetComponent<BoxCollider2D>().size.y * destination.localScale.y /2){                    
                    if(transform.position.y > destination.position.y){
                        movement = new Vector2(0,-1);
                    }
                    else{
                        movement = new Vector2(0,1);

                    }
                    sr.flipX = false;
                        SetBooster("UPDOWN");
                    rb.MovePosition(rb.position + movement * curSpeed * Time.fixedDeltaTime);

                    
                }
                else if(Mathf.Abs(transform.position.x - destination.position.x)>=0.1f){
                    if(transform.position.x > destination.position.x){
                        movement = new Vector2(-1,0);
                    }
                    else{
                        movement = new Vector2(1,0);

                    }
                    sr.flipX = animator.GetFloat("Horizontal") < 0 ? true : false;

                        SetBooster("LEFTRIGHT"); 
                    rb.MovePosition(rb.position + movement * curSpeed * Time.fixedDeltaTime);

                }
            }
        }

    void FixedUpdate(){
        //rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        //movement = new Vector2(animator.GetFloat("Horizontal"),animator.GetFloat("Vertical"));

        
        movementDirection = new Vector2(movement.x, movement.y);
        if (movementDirection != Vector2.zero){
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }

    }


    IEnumerator MiningCoroutine(){
                    //Debug.Log("미네랄 채취 시작");
        yield return new WaitForSeconds(weldingSec);
                    //Debug.Log("미네랄 채취 완료");
                    workLight.SetActive(false);
        animator.SetBool("Stop", false);
        isHolding = true;
        isMining = false;                        
        miningFlag = false;
        mineral.gameObject.SetActive(true);


        //미네랄 종류 선택()
        //packageType = PackageType.normal;
    }

    private void OnTriggerStay2D(Collider2D collision){
        // if(!placeFlag){
        //     placeFlag = true;
            if(collision.tag == "Mineral Field"){
                if(!isHolding){
                    gotMine = true;
                }
                //Debug.Log("미네랄 발견");
            }        
            
            else if(destination != null){
                    
                if(collision.name == destination.name){
                    //if(isHolding){
                        gotDestination = true;
                    //}
                    //Debug.Log("센터 발견");
                }
            }
        //}
    
    }
        private void OnTriggerExit2D(Collider2D collision){
        // if(!placeFlag){
        //     placeFlag = true;
            if(collision.tag == "Mineral Field"){
                    gotMine = false;
        isMining = false;
            }        
            
            else if(collision.tag == "Center"){
                    gotDestination = false;
            }
        //}
    
    }
        public void SetBooster(string dir){
        if(dir=="UPDOWN"){
            if (shadowType == ShadowType.booster){
                if(animator.GetFloat("Vertical") < 0){//하
                    boosters[0].gameObject.SetActive(false);
                    boosters[1].gameObject.SetActive(false);
                    boosters[2].gameObject.SetActive(true);
                    mineral.transform.localPosition = new Vector2(0,-0.05f);
                    workLight.transform.localPosition = new Vector2(0,-0.2f);
                }
                else{//상
                    boosters[0].gameObject.SetActive(true);
                    boosters[1].gameObject.SetActive(false);
                    boosters[2].gameObject.SetActive(false);
                    mineral.transform.localPosition = new Vector2(0,0.11f);
                    workLight.transform.localPosition = new Vector2(0.1f,0.2f);
                }
            }
        }
        else{
            if (shadowType == ShadowType.booster){
                if(animator.GetFloat("Horizontal") > 0){//우
                    boosters[1].flipX = false;
                    boosters[1].transform.localPosition = new Vector2(defaultSide.x,defaultSide.y);
                    boosters[0].gameObject.SetActive(false);
                    boosters[1].gameObject.SetActive(true);
                    boosters[2].gameObject.SetActive(false);
                    mineral.transform.localPosition = new Vector2(0.084f,0.034f);
                    workLight.transform.localPosition = new Vector2(0.25f,0);
                }
                else{//좌
                    boosters[1].flipX = true;
                    boosters[1].transform.localPosition = new Vector2(-defaultSide.x,defaultSide.y);
                    boosters[0].gameObject.SetActive(false);
                    boosters[1].gameObject.SetActive(true);
                    boosters[2].gameObject.SetActive(false);
                    mineral.transform.localPosition = new Vector2(-0.084f,0.034f);
                    workLight.transform.localPosition = new Vector2(-0.25f,0);
                }
            }
        }
    }

    public void HandleMineral(int amount = 0,bool floating = true){
        //int temp0 = curMineral;
        //int preMineral = curMineral;
        if(amount==0){
                
            switch(packageType){
                case PackageType.normal :
                    PlayerManager.instance.curMineral += PlayerManager.instance.capacity;
                    break;
                default :
                    break;
            }
        }
        else{
            PlayerManager.instance.curMineral += amount;
        }

        if(floating) PrintFloating("+ "+PlayerManager.instance.capacity.ToString());

    }    
    public void PrintFloating(string text, Sprite sprite = null)
    {

        if (text != "")
        {
            var clone = Instantiate(floatingText, floatingCanvas.transform.position, Quaternion.identity);
            clone.GetComponent<Text>().text = text;
            clone.transform.SetParent(floatingCanvas.transform);
        }
    }
    public void RepSound(){
        SoundManager.instance.Play("rep"+Random.Range(0,5));
    }    
}
