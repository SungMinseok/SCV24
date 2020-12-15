using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    [SerializeField]private Camera cam;
    public BoxCollider2D map;
    private float mapMinX, mapMaxX, mapMinY, mapMaxY;

    Vector3 dragOrigin;
    public bool onMouse;
    public bool isMoving;
    void Awake(){
        instance = this;

        mapMinX = map.transform.position.x - map.bounds.size.x / 2f;
        mapMaxX = map.transform.position.x + map.bounds.size.x / 2f;
        mapMinY = map.transform.position.y - map.bounds.size.y / 2f;
        mapMaxY = map.transform.position.y + map.bounds.size.y / 2f;
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
            //cam.transform.position += difference;
            cam.transform.position = ClampCamera(cam.transform.position + difference);
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
    Vector3 ClampCamera(Vector3 targetPosition){
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
