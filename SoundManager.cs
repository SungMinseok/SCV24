using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound{
    public string soundName;
    public AudioClip clip;
    private AudioSource source;     //사운드 플레이어(볼륨조절등)
    public float volume = 1f;
    public bool loop;
    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
    }
    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void SetLoop()
    {
        source.loop = true;
    }

    public void SetLoopCancel()
    {
        source.loop = false;
    }

    public void SetVolume()
    {
        source.volume = volume;
    }
    public bool isPlaying(){
        if(source.isPlaying){
            return true;
        }
        return false;
    }
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

    void Awake()
    {
        instance =this;
        bgmPlayer.clip = bgmSounds[0].clip;
        bgmPlayer.Play();        
        
        for(int i = 0; i<sfxSounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드파일:" + i + "=" + sfxSounds[i].soundName);
            sfxSounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }
    public void BGMPlay(int num){
        
        bgmPlayer.clip = bgmSounds[num].clip;
        bgmPlayer.Play();   
    }

    public void Play(string _soundName){
        // for(int i=0; i<sfxSounds.Length; i++){
        //     if(_soundName == sfxSounds[i].soundName){
        //         for(int j=0;j<sfxPlayer.Length; j++){
        //             if(!sfxPlayer[j].isPlaying){
        //                 sfxPlayer[j].clip=sfxSounds[i].clip;
        //                 sfxPlayer[j].Play();
        //                 return;
        //             }
        //         }
        //         return;
        //     }
        // }
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if(_soundName == sfxSounds[i].soundName)
            {
                sfxSounds[i].Play();
                return;
            }
        }
    }    
    public void Stop(string _soundName){
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if(_soundName == sfxSounds[i].soundName)
            {
                sfxSounds[i].Stop();
                return;
            }
        }
    }
    public bool IsPlaying(string _soundName){
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if(_soundName == sfxSounds[i].soundName)
            {
                if(sfxSounds[i].isPlaying()){
                    return true;
                }
            }
        }
        return false;
    }

    public void ClickSound(){
        Play("btn0");
    }
    public void ClickSoundInGame(){
        Play("btn1");
    }

    public void ReadySound(){
        Play("ready");
    } 


}
