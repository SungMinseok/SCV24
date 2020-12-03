using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
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

    [SerializeField] public Slider fuelBar;
    [SerializeField] public Text minText;


    public void ToggleAuto(){
        if(PlayerManager.instance.isAuto) PlayerManager.instance.StopAuto();
        PlayerManager.instance.isAuto = !PlayerManager.instance.isAuto;
    }

    public void ChargeFuel(){
        PlayerManager.instance.HandleFuel(1000f);
    }
}
