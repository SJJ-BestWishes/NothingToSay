using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//注意
//挂载在Camera
//音频放在Resource下Audio中(或者具体BgAudio或者EfcAudio下)
//格式 AudioManager.Instance.PlayBgMusic(AudioDic.attack));
//如需要添加固定声音源，可在TODO后添加
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get { return _instance; }
    }

    private AudioSource efcSource;
    private AudioSource bgSource;
    //TODO 可添加

    private const string bgPath = "Audio/BgAudio/";
    private const string efcPath = "Audio/EfcAudio/";
    //TODO 可添加

    //存储已经加载过的AudioClip
    //存储BgAudioClip
    private static Dictionary<string, AudioClip> bgAudioClipDic = new Dictionary<string, AudioClip>();
    //存储Efc
    private static Dictionary<string, AudioClip> efcAudioClipDic = new Dictionary<string, AudioClip>();
    //TODO 可添加

    void Awake()
    {
        _instance = this;
        efcSource = gameObject.AddComponent<AudioSource>();
        bgSource = gameObject.AddComponent<AudioSource>();
        //TODO 可添加
    }

    /// <summary>
    /// 得到音乐音频
    /// </summary>
    /// <param name="dic">什么类型音乐对应的字典</param>
    /// <param name="clipName">AudioDic 类型</param>
    /// <param name="path">该类型音乐对应的路径const</param>
    /// <returns></returns>
    private static AudioClip GetAudioClip(Dictionary<string, AudioClip> dic, string clipName,string path)
    {
        if (!dic.ContainsKey(clipName))
        {
            dic.Add(clipName, Resources.Load<AudioClip>(path + clipName));
            if (dic[clipName] == null)
            {
                if (dic == bgAudioClipDic)
                    Debug.Log("没有找到背景音乐音频" + clipName);
                else if (dic == efcAudioClipDic)
                    Debug.Log("没有找到特效音乐音频" + clipName);
                //TODO 可添加
                return null;
            }
            else
                return dic[clipName];
        }
        else
            return dic[clipName];
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="clipName">AudioDic 类型</param>
    /// <param name="pitch">速度(0-3)</param>
    /// <param name="volume">音量(0-1)</param>
    public void PlayBgMusic(string clipName, float pitch = 1, float volume = 1)
    {
        AudioClip audioClip = GetAudioClip(bgAudioClipDic,clipName,bgPath);
        if (bgSource.isPlaying)
            bgSource.Stop();
        bgSource.clip = audioClip;
        bgSource.pitch = pitch;
        bgSource.volume = volume;
        //开启循环
        bgSource.loop = true;
        bgSource.Play();
    }
    /// <summary>
    /// 播放特效音乐
    /// </summary>
    /// <param name="clipName">AudioDic 类型</param>
    /// <param name="pitch">速度(0-3)</param>
    /// <param name="volume">音量(0-1)</param>
    public void PlayEfcMusic(string clipName, float pitch = 1, float volume = 1)
    {
        AudioClip audioClip = GetAudioClip(efcAudioClipDic, clipName, efcPath);
        if (efcSource.isPlaying)
            efcSource.Stop();
        efcSource.clip = audioClip;
        efcSource.pitch = pitch;
        efcSource.volume = volume;
        efcSource.Play();
    }

    //TODO 可添加

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="path">Resource下Audio下路径</param>
    /// <param name="audioSource">指定播放源</param>
    /// <param name="pitch">速度(0-3)</param>
    /// <param name="volume">音量(0-1)</param>
    public void PlayMusic(string path, AudioSource audioSource , float pitch = 1, float volume = 1)
    {
        string musicPath = "Audio/" + path;
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.clip = Resources.Load<AudioClip>(musicPath);
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.Play();
    }

    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop()
    {
        efcSource.Stop();
        bgSource.Stop();
        //TODO 可添加
    }
}