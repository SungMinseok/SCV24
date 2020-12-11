using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    Vector2 defaultScale;
    void Start(){
        defaultScale = transform.localScale;
    }
    void OnMouseEnter(){
        transform.localScale = new Vector2(defaultScale.x * 1.2f,defaultScale.y * 1.2f);
    }
    void OnMouseExit(){
        
        transform.localScale = defaultScale;
    }
    void OnMouseUpAsButton(){
        UIManager.instance.TogglegoTo("Center");
        //Debug.Log("ㅇㅇ");
    }
}
