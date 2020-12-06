using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickManager : MonoBehaviour
{
    public static JoystickManager instance;    
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

    public GameObject stick;
    public GameObject bg;
    Vector2 stickFirstPosition;
    public Vector2 joyVec;
    float stickRadius;

    // Start is called before the first frame update
    void Start()
    {
        stickRadius = bg.GetComponent<RectTransform>().sizeDelta.y /2;
    }

    public void PointDown(){
        bg.transform.position = Input.mousePosition;
        stick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;
    }
    public void Drag(BaseEventData baseEventData){
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPosition = pointerEventData.position;
        joyVec = ( dragPosition - stickFirstPosition ).normalized;

        float stickDistance = Vector2.Distance( dragPosition, stickFirstPosition );

        if( stickDistance < stickRadius ){
            stick.transform.position = stickFirstPosition + joyVec * stickDistance ;
        }
        else{
            stick.transform.position = stickFirstPosition + joyVec * stickRadius ;

        }
    }
    public void Drop(){
        joyVec = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
