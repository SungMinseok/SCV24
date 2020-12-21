using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum BuildingType{
    Enterable,
    Resource,
    Drop,
    Item,
}
[RequireComponent (typeof (BoxCollider2D))]
public class SpriteButton : MonoBehaviour
{
    [SerializeField] public BuildingType buildingType;
    public string objectName;

    Vector2 defaultScale;
    void Start(){
        defaultScale = transform.localScale;
        if(objectName == "" )objectName = gameObject.name;
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
                PlayerManager.instance.Order(transform,OrderType.Enter);
                //UIManager.instance.TogglegoTo(objectName);
            }
            else if(buildingType == BuildingType.Resource){

                PlayerManager.instance.selectedMineral = this.transform;
                UIManager.instance.ToggleAuto();
            }
            else if(buildingType == BuildingType.Drop || buildingType == BuildingType.Item ){
                //PlayerManager.instance.destination = this.transform;
                // Debug.Log(transform.position);
                // Debug.Log(PlayerManager.instance. transform.position);
                PlayerManager.instance.Order(transform,OrderType.Get);
                
            }
        }

        //}
        //Debug.Log("ㅇㅇ");
    }

    public void DestroySprite(){
        Destroy(this);
    }
}
