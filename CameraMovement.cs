using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    [SerializeField]private Camera cam;

    Vector3 dragOrigin;
    public bool onMouse;
    public bool isMoving;
    void Awake(){
        instance = this;
    }
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        if(!UIManager.instance.OnUI()){
            PanCamera();
        }
    }

    void PanCamera(){
        if(Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButton(0)){
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
            //isMoving = true;
        }
        else{
            //isMoving = false;
        }
    }

    // void OnMouseEnter(){
    //     onMouse = true;
    // }
    // void OnMouseExit(){
    //     onMouse = false;
    // }
    // void OnMouseEnter(){
        
    //     Debug.Log("aa");
    // }    
    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     dragOrigin = cam.ScreenToWorldPoint(eventData.position);

    // }
    // public void OnDrag(PointerEventData eventData)
    // {Debug.Log("aa");
    //     Vector3 dragPos = cam.ScreenToWorldPoint(eventData.position);

    //     Vector3 difference = dragPos - dragOrigin;
    //     cam.transform.position = difference.normalized;
    //             // dragOrigin = cam.ScreenToWorldPoint(eventData.position);
    //             // Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(eventData.position);
    //             Debug.Log("dragOrigin" + dragOrigin);
    //             Debug.Log("difference" + difference);
    //             // //Debug.Log(difference);
    //             // //GetComponent<RectTransform>().anchoredPosition = Vector2.ClampMagnitude(GetComponent<RectTransform>().anchoredPosition, h);
    //             // cam.transform.position += difference;
    // }
}
