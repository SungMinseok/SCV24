using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound{
    public string soundName;
    public AudioClip clip;
    public float volume = 1f;
}
public class SoundManager : MonoBehaviour

{
    public static SoundManager instance;
    [Header("사운드 등록")]
    [SerializeField] Sound[] bgmSounds;
    [SerializeField] Sound[] sfxSounds;

    
    [Header("브금 플레이어")]
    [SerializeField] AudioSource bgmPlayer;
    [Header("효과음 플레이어")]
    [SerializeField] AudioSource[] sfxPlayer;

    void Start()
    {
        instance =this;
        bgmPlayer.clip = bgmSounds[0].clip;
        bgmPlayer.Play();
    }

    public void PlaySE(string _soundName){
        for(int i=0; i<sfxSounds.Length; i++){
            if(_soundName == sfxSounds[i].soundName){
                for(int j=0;j<sfxPlayer.Length; j++){
                    if(!sfxPlayer[j].isPlaying){
                        sfxPlayer[j].clip=sfxSounds[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                return;
            }
        }
    }

    public void ClickSound(){
        PlaySE("click");
    }

}
