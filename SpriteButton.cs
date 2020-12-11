using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum BuildingType{
    Enterable,
}
public class SpriteButton : MonoBehaviour
{
    [SerializeField] BuildingType buildingType;
    string objectName;

    Vector2 defaultScale;
    void Start(){
        defaultScale = transform.localScale;
        objectName = gameObject.name;
    }
    void OnMouseEnter(){
        transform.localScale = new Vector2(defaultScale.x * 1.1f,defaultScale.y * 1.1f);
    }
    void OnMouseExit(){
        
        transform.localScale = defaultScale;
    }
    void OnMouseUpAsButton(){
        //if(!CameraMovement.instance.isMoving){
        if(!UIManager.instance.OnUI()){

            if(buildingType == BuildingType.Enterable){

                UIManager.instance.TogglegoTo(objectName);
            }
        }

        //}
        //Debug.Log("ㅇㅇ");
    }
}
