using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Bot{
    public int index;
    public string name;
    public float efficiency;
    public float price;
    // public Bot(int a, string b, float c, float d){
    //     index = a;
    //     name = b;
    //     efficiency =c;
    //     price = d;

    // }
}
public class BotManager : MonoBehaviour
{
    public static BotManager instance;
    public List<Bot> botInfoList;
    public List<int> botSaved;
    public int maxPopulation = 20;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start(){
        //botSaved = new List<int>();
        //Debug.Log(botSaved.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshBotEquip(int num= -1){
        if(num == -1){
                
            for(int i=0;i<transform.childCount;i++){
                var temp =transform.GetChild(i).GetComponent<BotScript>();
                temp.speed = PlayerManager.instance.speed;
                temp.weldingSec = PlayerManager.instance.weldingSec + Random.Range(PlayerManager.instance.weldingSec * -0.01f, PlayerManager.instance.weldingSec * 0.01f);
                temp.capacity = (int)(temp.efficiency * PlayerManager.instance.capacity);
                temp.fuelUsagePerWalk = temp.efficiency * PlayerManager.instance.fuelUsagePerWalk;
            }
        }
        else{
            
            var temp =transform.GetChild(num).GetComponent<BotScript>();
            temp.speed = PlayerManager.instance.speed;
            temp.weldingSec = PlayerManager.instance.weldingSec + Random.Range(PlayerManager.instance.weldingSec * -0.01f, PlayerManager.instance.weldingSec * 0.01f);
            temp.capacity = (int)(temp.efficiency * PlayerManager.instance.capacity);
            temp.fuelUsagePerWalk = temp.efficiency * PlayerManager.instance.fuelUsagePerWalk;
        }
    }
    public void LoadBot(){
        
        Debug.Log(botSaved.Count);
        if(botSaved != null){
            StartCoroutine(LoadBotCoroutine());
        }
    }
    IEnumerator LoadBotCoroutine(){
        for(int i=0; i<botSaved.Count; i++){
            FactoryManager.instance.nowNum = botSaved[i];
            FactoryManager.instance.ProduceByLoad();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyBot(int index){
        botSaved.RemoveAt(index);
        transform.GetChild(index).GetComponent<BotScript>().DestroyBot();
        //Destroy(transform.GetChild(index));
    }
    public void DestroyAllBot(){
        for(int i=0;i<transform.childCount;i++){
            botSaved.Clear();
            transform.GetChild(i).GetComponent<BotScript>().DestroyBot();
            //Destroy(transform.GetChild(i));
        }
    }
}
