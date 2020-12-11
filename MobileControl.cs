using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum ControllerType
{
    Control,
    JoyPad,

}

//조이스틱 : 조이패드 + 쉬프트 (비활성화 : 퍼즐, 게임, 책, 게임오버 // 반투명 : 대화)
//상호작용 : 스페이스 (반투명 : 퍼즐, 게임)
#if UNITY_ANDROID || UNITY_IOS
public class MobileControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{
    public static MobileControl instance;
    
    [Header("JoyPad")]
    [SerializeField] private ControllerType type;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform js;
    public GameObject shiftBtn;
    public GameObject spaceBtn;
    public bool isTouch;
    //public bool isDragging;
    private float r;
    private float h;
    public PlayerManager thePlayer;
    public MobileControl mobileControl;
    public Camera theCamera;
    Vector2 originPos;
    public bool isFixed;
    public bool isActivated;

    [Header("Control")]
    public GameObject fixedJoy;
    public GameObject unfixedJoy;
    void Start()
    {
        instance = this;
        //theCamera = GameObject.Find("Camera").GetComponent<Camera>();


        if(type == ControllerType.JoyPad){      

        theCamera = GameObject.Find("Camera").GetComponent<Camera>();
            originPos = bg.transform.localPosition;
            r = bg.rect.width * 0.5f;
            //h = GetComponent<RectTransform>().rect.height *0.5f;
            mobileControl = GameObject.Find("MobileControl").GetComponent<MobileControl>();
        }
        // if (type != ControllerType.TouchPad)
        // {
        // }

        //rect= GetComponent<RectTransform>();
        //unfixedJoyRect = GetComponent<RectTransform>();

    }

    void Update()
    {
        if (type == ControllerType.JoyPad)
        {
            if (thePlayer.canMove)
            {
                if (isTouch)
                {
                    if (thePlayer.movement == Vector2.zero)
                    {
                        thePlayer.movement = new Vector2(thePlayer.animator.GetFloat("Horizontal"), thePlayer.animator.GetFloat("Vertical"));
                    }

                    thePlayer.rb.MovePosition(thePlayer.rb.position + thePlayer.movement * thePlayer.curSpeed * Time.fixedDeltaTime);
                    thePlayer.animator.SetFloat("Speed", 1f);
                    thePlayer.HandleFuel(-thePlayer.fuelUsagePerWalk);
                }
                else
                {
                    
                    js.localPosition = Vector3.zero;
                    bg.gameObject.transform.localPosition = originPos;
                    if (js.localPosition == Vector3.zero)
                    {
                        thePlayer.animator.SetFloat("Speed", 0f);
                    }

                }
            }
            else if (!thePlayer.canMove)
            {

            }
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
            //Debug.Log("OnDrag");
        if (type == ControllerType.JoyPad)
        {
            if (thePlayer.canMove)
            {
                Vector3 temp = theCamera.ScreenToWorldPoint(eventData.position);
                js.gameObject.transform.position = new Vector3(temp.x, temp.y, 0);
                js.anchoredPosition = Vector2.ClampMagnitude(js.anchoredPosition, r);
                //GetComponent<RectTransform>().anchoredPosition = Vector2.ClampMagnitude(GetComponent<RectTransform>().anchoredPosition, h);
                Vector3 v = js.anchoredPosition.normalized;

                if (v.y < 0.7 && v.y > 0)
                {
                    if (js.localPosition.x > 0)
                    {
                        thePlayer.movement.x = 1f;
                        thePlayer.movement.y = 0;
                    }
                    else if (js.localPosition.x < 0)
                    {
                        thePlayer.movement.x = -1f;
                        thePlayer.movement.y = 0;
                    }
                    PlayerManager.instance.SetBooster("LEFTRIGHT");
                    PlayerManager.instance.sr.flipX = PlayerManager.instance.animator.GetFloat("Horizontal") < 0 ? true : false;
                }
                else if (v.y >= 0.7)
                {

                    thePlayer.movement.x = 0;
                    thePlayer.movement.y = 1f;
                    PlayerManager.instance.SetBooster("UPDOWN");
                    PlayerManager.instance.sr.flipX = false;
                }

                else if (v.y > -0.7 && v.y < 0)
                {
                    if (js.localPosition.x > 0)
                    {
                        thePlayer.movement.x = 1f;
                        thePlayer.movement.y = 0;
                    }
                    else if (js.localPosition.x < 0)
                    {
                        thePlayer.movement.x = -1f;
                        thePlayer.movement.y = 0;
                    }
                    PlayerManager.instance.SetBooster("LEFTRIGHT");
                    PlayerManager.instance.sr.flipX = PlayerManager.instance.animator.GetFloat("Horizontal") < 0 ? true : false;
                }
                else if (v.y <= -0.7)
                {

                    thePlayer.movement.x = 0;
                    thePlayer.movement.y = -1f;
                    PlayerManager.instance.SetBooster("UPDOWN");
                    PlayerManager.instance.sr.flipX = false;
                }

                thePlayer.animator.SetFloat("Horizontal", thePlayer.movement.x);
                thePlayer.animator.SetFloat("Vertical", thePlayer.movement.y);

            }
        }


    }
    public void OnPointerDown(PointerEventData eventData)
    {
            //Debug.Log("OnPointerDown");
        if (type == ControllerType.JoyPad)
        {
            Vector3 temp = theCamera.ScreenToWorldPoint(eventData.position);
            js.gameObject.transform.position = new Vector3(temp.x, temp.y, 0);
            if(!mobileControl.isFixed){
                
                bg.gameObject.transform.position = new Vector3(temp.x, temp.y, 0);
            }
            isTouch = true;
            OnDrag(eventData);
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
            //Debug.Log("OnPointerUp");
        if (type == ControllerType.JoyPad)
        {
            isTouch = false;
            js.localPosition = Vector3.zero;
            bg.gameObject.transform.localPosition = originPos;
            thePlayer.animator.SetFloat("Speed", 0f);
        }


    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void ToggleFix(){
        isFixed = !isFixed;
        if(isFixed){
            fixedJoy.SetActive(true);
            unfixedJoy.SetActive(false);
        }
        else{
            
            fixedJoy.SetActive(false);
            unfixedJoy.SetActive(true);
        }
    }

}
#endif
