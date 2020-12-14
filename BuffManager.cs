using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Buff{
    public string name;
    public float time;

    public Buff(string _name, float _time){
        name = _name;
        time = _time;
    }

    
}
public class BuffManager : MonoBehaviour
{
    public List<Buff> buffs = new List<Buff>();
    // Start is called before the first frame update
    public Image centerBuffImg;
    public Text centerBuffText;
    public Button centerBuffBtn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateBuff(string name){
        for(int i=0;i<buffs.Count;i++){
            if(buffs[i].name == name){
                StartCoroutine(BuffCoroutine(buffs[i]));
            }
        }
    }

    IEnumerator BuffCoroutine(Buff buff){
        centerBuffBtn.interactable = false;
        centerBuffImg.gameObject.SetActive(true);
        centerBuffText.text = buff.time.ToString();
        float tempTime = buff.time;


        while(tempTime>0f){  
            
            // buff.time--;

            
            // centerBuffText.text = buff.time.ToString();
            // centerBuffImg.fillAmount = (float)buff.time / (float)full ;
            // Debug.Log(centerBuffImg.fillAmount);
            // yield return new WaitForSeconds(1f);

            tempTime -= Time.deltaTime;
            
            centerBuffText.text = (Mathf.CeilToInt(tempTime)).ToString();
            centerBuffImg.fillAmount = tempTime / buff.time ;
            yield return new WaitForFixedUpdate();
        }

        centerBuffBtn.interactable = true;
        centerBuffImg.gameObject.SetActive(false);


    }
}
