using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    public bool isPaused;
    public bool testMode;

    public Vector2 screenSize;
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }

        
        screenSize = new Vector2(Screen.width,Screen.height);
    }
    void Start()
    {
        Screen.sleepTimeout=SleepTimeout.NeverSleep;
        //Debug.Log(screenSize);
    }

    void OnApplicationQuit(){
        
        if(DBManager.instance != null) DBManager.instance.CallSave(0);
    }
    void OnApplicationPause(bool pause)
    {
        if(DBManager.instance != null){
                
            if (pause)
            {
                isPaused = true;
                DBManager.instance.CallSave(0);
                /* 앱이 비활성화 되었을 때 처리 */    
            }
            else{
                if(isPaused){
                    isPaused = false;
                }
            }
        }
    }
}
