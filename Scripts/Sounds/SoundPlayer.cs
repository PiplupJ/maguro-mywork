using UnityEngine;
using System.Collections.Generic;

public enum SoundType
{
    TitleBGM = 0,
    GameBGM = 1,
    EndingBGM = 2,

    SelectSE = 10,
    HoverSE = 11,

    ThrowSE = 21,
    EatSE = 22,
    ExplodeSE = 23,
    KnockOutSE = 24,

    ApplauseSE = 31,
    AwardEntrySE = 32
}
public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance;

    private Dictionary<SoundType, SoundData> dict = new();

    [SerializeField] private SoundDB database;

    private AudioSource bgmSource;
    [SerializeField]float bgmVolume = 0.5f;
    private AudioSource[] seSource;
    [SerializeField]int seSourceCount = 5;
    private int seIndex = 0;

    void Awake()
    {
        if(Instance!=null && Instance!=this){
            Destroy(gameObject);
            return;
        }

        Instance = this;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;

        seSource = new AudioSource[seSourceCount];

        for(int i = 0; i < seSourceCount; i++){
            seSource[i] = gameObject.AddComponent<AudioSource>();
            seSource[i].playOnAwake = false;
        }

        foreach(var s in database.sounds){
            if(dict.ContainsKey(s.type)){
                Debug.LogWarning("SoundPlayer:重複キーがあります"+s.type);
            }

            dict[s.type] = s;
        }

        DontDestroyOnLoad(gameObject);
    }

    //BGMの再生
    public void PlayBGM(SoundType soundType)
    {
        if(!dict.TryGetValue(soundType, out var s)){
            Debug.LogWarning("SoundPlayer:サウンドが存在しません"+soundType);
            return;
        }

        bgmSource.clip = s.clip;
        bgmSource.Play();

    }
    //BGMの停止
    public void StopBGM()
    {
        bgmSource.Stop();
    }
    //SEの再生
    public void PlaySE(SoundType soundType)
    {
        AudioSource source = seSource[seIndex];
        if(!dict.TryGetValue(soundType, out var s)){
            Debug.LogWarning("SoundPlayer:サウンドが存在しません"+soundType);
            return;
        }

        source.pitch = 1f + Random.Range(-s.pitch, s.pitch);
        source.PlayOneShot(s.clip);

        seIndex = (seIndex+1)%seSourceCount;
    }
}
