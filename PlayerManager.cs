using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum UnitType{
    unit,
    shadow,
}
public enum ShadowType{
    normal,
    booster,
}
public enum PackageType{
    none,
    normal,
}
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;    
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
    [SerializeField] public UnitType type;
    [SerializeField] public ShadowType shadowType;
    [SerializeField] public PackageType packageType = PackageType.none;
    public int curMineral;
    Slider fuelBar;
    Text mineralBar;

    //public float runSpeed = 4f;
    //private float defaultSpeed;

    public float curFuel;
    private float calculatedFuel;

    public float fuelUsagePerWalk = 1f;
    public float fuelUsagePerRun = 2f;

    //public float miningSpeed = 5f;
    
    [Header("장비 초기값")]
    public float defaultWeldingSec;
    public float defaultSpeed;
    public float defaultFuel;
    public int defaultCapacity;

    [Header("장비 단계")]
    public int weldingLevel;
    public float weldingSec = 2f;
    public int engineLevel;
    public float speed = 2f;
    public int fuelLevel;
    public float maxFuel;
    public int bodyLevel;
    public int capacity = 8;//적재량


    [HideInInspector]    public Rigidbody2D rb;
    [HideInInspector]    public SpriteRenderer sr;
    
    [HideInInspector] public Animator animator;
    Animator animatorMother;
    Animator animatorChild;
    GameObject booster;
    RaycastHit2D hitTemp;
    private SpriteRenderer[] boosters;
     [SerializeField]private Vector2 defaultSide;
        [HideInInspector]public Vector2 movement;
        [HideInInspector]public Vector2 movementDirection;
        [HideInInspector]public bool canMove;
        [HideInInspector]public bool isAlive;
        [HideInInspector]public bool isHolding;//뭔가 들고 있을 때
        [HideInInspector]public bool isMining;//채취 중일 때
    private bool miningFlag;
    [Header("이동 관련")]//auto 버튼 눌렀을 때
        [HideInInspector]public bool isAuto;
       [HideInInspector] public bool autoGathering;  //아무것도 들고 있지 않은 상태 > 미네랄로 감.
        [HideInInspector]public bool autoReturnCargo;//미네랄 들고 있는 상태 > 센터로 감.
        [HideInInspector]public bool isMoving;
        [HideInInspector]public bool gotMine;//미네랄 발견
        [HideInInspector]public bool gotCenter;//센터 발견
        [HideInInspector]public bool placeFlag;
        [HideInInspector]public SpriteRenderer mineral;
        [HideInInspector]public GameObject workLight;
        [HideInInspector]public Transform destination;
    IEnumerator miningCoroutine;
    bool onX;
    bool onY;
    [Header("센터 이동")]//auto 버튼 눌렀을 때
        [HideInInspector]public bool goToCenter;
    void Start()
    {
        if(type==UnitType.unit){
            isAlive = true;
            canMove = true;
            rb = GetComponent<Rigidbody2D>();
            if (GameObject.Find("Shadow") != null){
                
                animatorChild = transform.Find("Shadow").GetComponent<Animator>();
            }
        }
        else if(type==UnitType.shadow){
            transform.localPosition = new Vector2(0.015f,-0.015f);
            animatorMother = transform.parent.GetComponent<Animator>();
            //getco
        }
        
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
        
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        //defaultSpeed = speed;

        //초기설정
        weldingSec = defaultWeldingSec;
        speed = defaultSpeed;
        maxFuel = defaultFuel;
        capacity = defaultCapacity;

        //체력
        fuelBar = UIManager.instance.fuelBar;
        if(fuelBar != null) fuelBar.value = (float) curFuel / (float) maxFuel;

        //미네랄
        mineralBar = UIManager.instance.minText;

        miningCoroutine = MiningCoroutine();
    }

    // Update is called once per frame
    void Update()
    {
#region PC 이동
        // if (type == UnitType.unit)
        // {
        //     if (canMove && !animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Att"))
        
        //     {
                

        //         if (Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Horizontal") == 0)
        //         {//상하 먼저 입력 
        //             onY = true;
        //             onX = false;
        //         }
        //         else if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") == 0)
        //         {//좌우 먼저 입력
        //             onY = false;
        //             onX = true;
        //         }
        //         else if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        //         {
        //             onX = false;
        //             onY = false;
        //         }
        //         if (onX)
        //         {//좌우 누른 상태에서 상하 가능

        //             if (Input.GetAxisRaw("Vertical") != 0)
        //             {
        //                 ////Debug.Log("상하");
        //                 movement.x = 0;
        //                 movement.y = Input.GetAxisRaw("Vertical");

        //                 sr.flipX = false;

                     
        //                 SetBooster("UPDOWN");       
        //             }
        //             else if (Input.GetAxisRaw("Horizontal") != 0)
        //             {
        //                 ////Debug.Log("좌우");
        //                 movement.x = Input.GetAxisRaw("Horizontal");
        //                 movement.y = 0;
        //                 sr.flipX = animator.GetFloat("Horizontal") < 0 ? true : false;
                        
                     
        //                 SetBooster("LEFTRIGHT"); 

        //             }
        //         }
        //         else if (onY)
        //         {//상하 누른 상태에서 좌우 가능
        //             if (Input.GetAxisRaw("Horizontal") != 0)
        //             {
        //                 //Debug.Log("좌우");
        //                 movement.x = Input.GetAxisRaw("Horizontal");
        //                 movement.y = 0;

        //                 sr.flipX = animator.GetFloat("Horizontal") < 0 ? true : false;

        //                 SetBooster("LEFTRIGHT"); 
        //             }
        //             else if (Input.GetAxisRaw("Vertical") != 0)
        //             {
        //                 //Debug.Log("상하");
        //                 movement.x = 0;
        //                 movement.y = Input.GetAxisRaw("Vertical");

        //                 sr.flipX = false;

        //                 SetBooster("UPDOWN");                        

        //             }
        //         }


        //         if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        //         {
        //             movement.x = 0;
        //             movement.y = 0;
        //         }
        //         if (Input.GetKey(KeyCode.LeftShift) && movement != Vector2.zero)
        //         {
        //             rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        //             animator.SetFloat("Speed", speed / defaultSpeed / 2);
        //             //curFuel -= fuelUsagePerRun;
        //             HandleFuel(-fuelUsagePerRun);
        //         }
        //         else if (movement != Vector2.zero)
        //         {
        //             rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        //             animator.SetFloat("Speed", speed / defaultSpeed / 2);
        //            // curFuel -= fuelUsagePerWalk;
        //             HandleFuel(-fuelUsagePerWalk);
        //         }
        //         else if (movement == Vector2.zero)
        //         {
        //             animator.SetFloat("Speed", 0f);
        //         }


        //     }
        // }
        // else if(type == UnitType.shadow){
        //     animator.SetFloat("Horizontal",animatorMother.GetFloat("Horizontal"));
        //     animator.SetFloat("Vertical",animatorMother.GetFloat("Vertical"));
            
        //     sr.flipX = animator.GetFloat("Horizontal") < 0 ? true : false;
            
        //     animator.SetFloat("Speed", animatorMother.GetFloat("Speed"));

        //     //(animator.GetFloat("Horizontal"),animator.GetFloat("Vertical"));
        // }

        
#endregion

#region PC 공격
        if (type == UnitType.unit)
        {
            if (canMove)
            {
                
                bool shot = Input.GetKey(KeyCode.Space);
                if(shot && !animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Att")){
                    animator.SetTrigger("Att");
                    if(animatorChild!=null)
                        animatorChild.SetTrigger("Att");

                }
            }
        }
        //animator.SetFloat("Speed", movement.sqrMagnitude);
        
#endregion
#region Fuel
        //HandleFuel();
#endregion
#region Auto Mode
        
        if (type == UnitType.unit){
            if(isAuto){
                UIManager.instance.DisableColliders();
                //goToCenter = false;

                //if(goToCenter || MobileControl.instance.isTouch|| Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
                if(goToCenter || MobileControl.instance.isTouch){
                    isAuto = false;  //오토모드중 움직이면 오토 중지.
                Debug.Log("11");
                    StopAuto();
                       //Debug.Log("22");
                }
                
                    
                    
                    
                    //미네랄로 이동
                if(!isHolding&&!isMining){
                    //Debug.Log("미네랄로 이동");
                    destination = GameObject.FindWithTag("Mineral Field").transform;
                    
                    if(gotMine){
                        gotMine = false;
                        isMining = true;
                        destination = null;

                    }
                    else if(Mathf.Abs(transform.position.y - destination.position.y)>=0.1f){
                    // else if(Mathf.Abs(transform.position.y - destination.position.y)>=
                    // destination.GetComponent<BoxCollider2D>().bounds.max.y - destination.GetComponent<BoxCollider2D>().bounds.min.y
                    // ){
                        
                        
                        // //Debug.Log("1 :" +destination.GetComponent<BoxCollider2D>().bounds.max.y);
                        // //Debug.Log("2 :" +destination.GetComponent<BoxCollider2D>().bounds.min.y);
                        
                        //gotMine = false;
                        ////Debug.Log(Mathf.Abs(transform.position.y - destination.position.y));
                            //transform.position = Vector2.MoveTowards(transform.position,destination.position,Time.deltaTime);
                        if(transform.position.y > destination.position.y){
                            //transform.Translate(Vector3.down*speed* Time.deltaTime);
                            movement = new Vector2(0,-1);
                            //curFuel -= fuelUsagePerRun;
                        }
                        else{
                            //transform.Translate(Vector3.up*speed* Time.deltaTime);
                            movement = new Vector2(0,1);

                        }
                        sr.flipX = false;
                        SetBooster("UPDOWN");
                        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                        animator.SetFloat("Speed", speed / defaultSpeed / 2);
                        HandleFuel(-fuelUsagePerRun);
                        
                    }
                    else if(Mathf.Abs(transform.position.x - destination.position.x)>=0.1f){
                        //gotMine = false;
                        if(transform.position.x > destination.position.x){
                            movement = new Vector2(-1,0);
                            //transform.Translate(Vector3.left*speed* Time.deltaTime);
                        }
                        else{
                            //transform.Translate(Vector3.right*speed* Time.deltaTime);
                            movement = new Vector2(1,0);

                        }
                        sr.flipX = animator.GetFloat("Horizontal") < 0 ? true : false;
                        SetBooster("LEFTRIGHT"); 
                        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                        animator.SetFloat("Speed", speed / defaultSpeed / 2);
                        HandleFuel(-fuelUsagePerRun);
                    }
                }
                else if(isMining){
                    //Debug.Log("미네랄 채취 준비");
                    animator.SetBool("Stop", true);
                    workLight.SetActive(true);
                    if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Att")){
                        
                        animator.SetTrigger("Att");
                    }
                    if(!miningFlag){
                    ////Debug.Log("1");
                        miningFlag = true;
                        StartCoroutine(MiningCoroutine());
                    ////Debug.Log("2");
                    } 
                }
                else if(isHolding&&!isMining){
                    //StopCoroutine(miningCoroutine);
                    //Debug.Log("센터로 이동");
                    
                    destination = GameObject.FindWithTag("Center").transform;
                                        
                    if(gotCenter){
                        HandleMineral();
                        gotCenter = false;
                        isHolding = false;
                        destination = null;
                        mineral.gameObject.SetActive(false);
                        packageType = PackageType.none;
                        //Debug.Log("미네랄 저장");
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
                        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                        animator.SetFloat("Speed", speed / defaultSpeed / 2);
                        HandleFuel(-fuelUsagePerRun);
                        
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
                        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                        animator.SetFloat("Speed", speed / defaultSpeed / 2);
                        HandleFuel(-fuelUsagePerRun);
                    }
                }
            }
            if(goToCenter){
                //StopAuto();
                UIManager.instance.DisableColliders();
                //canMove = false;
                //isAuto = false;
                
                if(isAuto || MobileControl.instance.isTouch){
                    goToCenter = false;  //오토모드중 움직이면 오토 중지.
                Debug.Log("22");
                    StopAuto();
                }
                destination = GameObject.FindWithTag("Center").transform;
                                    
                if(gotCenter){
                    //HandleMineral();
                    goToCenter = false;
                    gotCenter = false;
                    //isHolding = false;
                    destination = null;
                    //mineral.gameObject.SetActive(false);
                    //packageType = PackageType.none;
                    ////Debug.Log("미네랄 저장");
                    EnterCenter();
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
                    rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                    animator.SetFloat("Speed", speed / defaultSpeed / 2);
                    HandleFuel(-fuelUsagePerRun);
                    
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
                    rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                    animator.SetFloat("Speed", speed / defaultSpeed / 2);
                    HandleFuel(-fuelUsagePerRun);
                }
            }
        }
#endregion

#region SetMineral
        if(int.Parse(mineralBar.text)<curMineral){
            ////Debug.Log("미네랄 업");
            
            mineralBar.text = (int.Parse(mineralBar.text) + 1).ToString();
            //mineralBar.text = ((int)Mathf.Lerp(int.Parse(mineralBar.text), curMineral, Time.deltaTime *speed )).ToString();
        }
        // else{
        //     mineralBar.text = curMineral.ToString();
        // }
#endregion

        if (shadowType == ShadowType.booster){
            if(animator.GetFloat("Speed")!=0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Att")){
                booster.SetActive(true);
            }
            else{
                booster.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            animator.SetBool("Stop", true);
        }
        else if(Input.GetKeyUp(KeyCode.Space)){
            animator.SetBool("Stop", false);
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

    public void Death(){
        if(isAlive){   
            if (type == UnitType.unit)
            {

                isAlive = false;
                canMove = false;
                animator.SetTrigger("Dth");   
                animatorChild.SetTrigger("Dth");  
                   
                StartCoroutine(DeathCoroutine());
            }
        }
    }

    public IEnumerator DeathCoroutine(){
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(()=>animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Destroy(gameObject);
    }
//부스터랑 미네랄 위치 세팅
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

    public void HandleFuel(float amount){
        if(curFuel>=0 && curFuel<=maxFuel){
            curFuel += amount;
        }
        else if(curFuel<0){
            curFuel = 0f;
            fuelBar.value = 0;
            canMove = false;
            animator.SetFloat("Speed", 0f);
            //Debug.Log("앵꼬");
        }
        else if(curFuel>maxFuel){
            curFuel = maxFuel;
            //Debug.Log("풀차지");
        }
        fuelBar.value = Mathf.Lerp(fuelBar.value,(float) curFuel / (float) maxFuel, Time.deltaTime*10 );
        
    }

    void HandleMineral(){
        //int temp0 = curMineral;
        int preMineral = curMineral;
        switch(packageType){
            case PackageType.normal :
                curMineral += capacity;
                break;
            default :
                break;
        }

        // float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 

        // float offset = (curMineral - preMineral) / duration;



        // while (preMineral < curMineral)

        // {

        //     preMineral += (int)(offset * Time.deltaTime);

        //     mineralBar.text = ((int)preMineral).ToString();


        // }



        // preMineral = curMineral;

        // mineralBar.text = ((int)preMineral).ToString();
        // while(int.Parse(mineralBar.text)!=curMineral){

        //     mineralBar.text = ((int)Mathf.Lerp(int.Parse(mineralBar.text), curMineral, Time.deltaTime*10 )).ToString();
        // }
        //StartCoroutine(HandleMineralCoroutine());
        //mineralBar.text = (int)Mathf.Lerp(mineralBar.text.ToString(), curMineral, Time.deltaTime*10 );
        //mineralBar.text = temp.ToString();

        ////Debug.Log(temp0,temp,curMineral);
        // switch(packageType){
        //     case PackageType.normal :
        //         curMineral += 8;
        //         break;
        //     default :
        //         break;
        // }
        // mineralBar.text = curMineral.ToString();
    }
    // IEnumerator HandleMineralCoroutine(){
    //     yield return 
    //     while(int.Parse(mineralBar.text)==curMineral){

    //         mineralBar.text = (int)Mathf.Lerp(mineralBar.text.ToString(), curMineral, Time.deltaTime*10 );
    //     }
    //     //yield return new WaitUntil(()=> int.Parse(mineralBar.text)==curMineral);
    // }
    // public void GatherAuto(){
    //     autoGathering = true;
    //     animator.SetTrigger("Att");
    // }
    // IEnumerator GatherAutoCoroutine(){
    //     animator.SetTrigger("Att");
    //     for(int i=0;i<5;i++){
    //         yield return new WaitForSeconds(1f);
    //     }
    // }

    public void Gather(Transform destination){
        
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
            
            else if(collision.tag == "Center"){
                //if(isHolding){
                    gotCenter = true;
                //}
                //Debug.Log("센터 발견");
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
                    gotCenter = false;
            }
        //}
    
    }


    // public void RayCheck(){
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, )
    // }

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
        packageType = PackageType.normal;
    }

    public void StopAuto(){
 Debug.Log("STOP AUTO");
        UIManager.instance.EnableColliders();
        isMining = false;
        StopAllCoroutines();
        animator.SetBool("Stop", false);
        miningFlag = false;
        destination=null;
        workLight.SetActive(false);
        isMining = false;
    }
    // public void GoToCenter(){
    //     goToCenter = !goToCenter;
    // }
    public void EnterCenter(){    
        StopAuto();
        UIManager.instance.centerUI.SetActive(true);
        canMove = false;
    }
    public void ExitCenter(){
        UIManager.instance.centerUI.SetActive(false);
        canMove = true;
    }

    public void RefreshEquip(){
        weldingSec = defaultWeldingSec + weldingLevel * UpgradeManager.instance.upgradeList[0].upgradeDelta;
        speed = defaultSpeed + engineLevel * UpgradeManager.instance.upgradeList[1].upgradeDelta;
        maxFuel = defaultFuel + fuelLevel  * UpgradeManager.instance.upgradeList[2].upgradeDelta;
        capacity = bodyLevel * (int)UpgradeManager.instance.upgradeList[3].upgradeDelta;
    }
}
