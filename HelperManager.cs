using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    public static HelperManager instance;

    public bool flag;
    public GameObject bundle;
    public GameObject[] helpers; //미네랄, 센터, 상단UI
    public GameObject[] arrows; //미네랄, 센터, 상단UI
    public GameObject finalDes;
    void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void HelperOn(){
        StartCoroutine(HelperCoroutine());
    }
    public void HelperOk(){
        flag = false;
    }
    IEnumerator HelperCoroutine(){
        bundle.SetActive(true);
        SoundManager.instance.Play("transmission");
        helpers[0].SetActive(true);
        //arrows[0].SetActive(true);
        flag = true;
        yield return new WaitUntil(()=>!flag);
        helpers[0].SetActive(false);
        //arrows[0].SetActive(false);

        for(int i=1;i<=4;i++){
            
        SoundManager.instance.Play("transmission");
            helpers[i].SetActive(true);//하단 0번
            arrows[i].SetActive(true);
            flag = true;
            yield return new WaitUntil(()=>!flag);
            helpers[i].SetActive(false);
            arrows[i].SetActive(false);

        }

        
        SoundManager.instance.Play("transmission");

        finalDes.SetActive(true);
        flag = true;
        yield return new WaitUntil(()=>!flag);
        finalDes.SetActive(false);

        PlayerManager.instance.GameStart();
        // helpers[1].SetActive(true);//하단 0번
        // arrows[1].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[1].SetActive(false);
        // arrows[1].SetActive(false);

        // helpers[2].SetActive(true);
        // arrows[2].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[2].SetActive(false);
        // arrows[2].SetActive(true);
        
        // helpers[3].SetActive(true);
        // arrows[3].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[3].SetActive(false);
        // arrows[3].SetActive(true);

        // helpers[4].SetActive(true);
        // arrows[4].SetActive(true);
        // flag = true;
        // yield return new WaitUntil(()=>!flag);
        // helpers[4].SetActive(false);
        // arrows[4].SetActive(true);



        bundle.SetActive(false);
    }
}
