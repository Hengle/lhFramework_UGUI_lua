using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

using LaoHan.Data;

namespace LaoHan.Infrastruture
{
    public enum ESound2DType
    {
        Sound2D=0,
        BackMusic=2
    }
    public class lhAudioManager {

        private Transform m_defaultParent;
        private Dictionary<string, AudioMixer> mixerDic;
        private Dictionary<ESound2DType, Dictionary<string, AudioSource>> sound2DLibrary;
        public static float sound2DVolume = 1;
        public static float backMusicVolume = 1;
        public static float sound3DVolume = 1;

        private static lhAudioManager m_instance;
        public static lhAudioManager GetInstance()
        {
            if (m_instance != null) return null;
            return m_instance = new lhAudioManager();
        }
        lhAudioManager()
        {

        }
        public void Initialize(Action onInitialOver)
        {
            m_defaultParent = GameObject.FindObjectOfType<AudioListener>().transform;
            UnityEngine.Object.DontDestroyOnLoad(m_defaultParent.gameObject);
            mixerDic = new Dictionary<string, AudioMixer>();
            sound2DLibrary = new Dictionary<ESound2DType, Dictionary<string, AudioSource>>();
            string[] mixerArr = new string[lhConfigData.audioConfig.mixers.Count];
            for (int i = 0; i < lhConfigData.audioConfig.mixers.Count; i++)
            {
                mixerArr[i] = lhConfigData.audioConfig.mixers[i].path;
            }
            lhResources.Load(mixerArr,
                (i, v, o) =>
                {
                    mixerDic.Add(lhConfigData.audioConfig.mixers[i].name, o as AudioMixer);
                },
                () => {
                    bool sound2d = false;
                    bool backmusic = false;
                    Action OnLoadOver = () =>
                    {
                        if (sound2d && backmusic)
                        {
                            onInitialOver();
                        }
                    };
                    LoadSound(ESound2DType.Sound2D, () => { sound2d = true; OnLoadOver(); }, lhConfigData.audioConfig.sound2D.ToArray());
                    LoadSound(ESound2DType.BackMusic, () => { backmusic = true; OnLoadOver(); }, lhConfigData.audioConfig.backMusic.ToArray());
                }
            );
        }
        public void Dispose()
        {
            m_instance = null;
        }
        public static void StoreSound3D(int count,params lhConfigData.AudioConfig.AudioData[] arr)
        {
            string[] sound3DArr = new string[arr.Length];
            ObjectStoreData[] storeArr = new ObjectStoreData[arr.Length];
            for (int j = 0; j < arr.Length; j++)
            {
                var audioData = arr[j];
                string clipPath = audioData.clipPath;
                storeArr[j] = new ObjectStoreData();
                storeArr[j].index = arr[j].name;
                storeArr[j].path = arr[j].prefabPath;
                storeArr[j].count = 1;
                storeArr[j].onCreateHandler = o =>
                {
                    var obj = o as GameObject;
                    AudioSource audioSource = obj.GetComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    lhResources.Load(clipPath, (clip) =>
                    {
                        audioSource.clip = clip as AudioClip;
                    });
                    audioSource.outputAudioMixerGroup = m_instance.mixerDic[audioData.mixerName].FindMatchingGroups(audioData.mixerGroup)[0];
                };
            }
            lhObjectManager.Store(storeArr, () =>
            {

            });
        }
        public static void Play3D(string soundName)
        {
            var soundAudio = lhObjectManager.GetObject(soundName, EPoolType.Sound3D) as AudioSource;
            soundAudio.volume = sound3DVolume;
            soundAudio.Play();
            lhObjectManager.FreeObject(soundName, soundAudio, soundAudio.time, EPoolType.Sound3D);
        }
        public static void Play2D(ESound2DType audioType,string name)
        {
            if (!m_instance.sound2DLibrary.ContainsKey(audioType))
            {
                lhDebug.LogError("LaoHan: dont has this audioType: " + audioType);
                return;
            }
            if (!m_instance.sound2DLibrary[audioType].ContainsKey(name))
            {
                lhDebug.LogError("LaoHan: dont has this name: " + name);
                return;
            }
            var audioSource = m_instance.sound2DLibrary[audioType][name];
            if (audioType==ESound2DType.Sound2D)
                audioSource.volume = sound2DVolume;
            else
                audioSource.volume = backMusicVolume;
            audioSource.Play();
        }
        private void LoadSound(ESound2DType audioType,Action onLoadOver,params lhConfigData.AudioConfig.AudioData[] arr)
        {
            string[] sound2DArr = new string[arr.Length];
            for (int j = 0; j < arr.Length; j++)
            {
                sound2DArr[j] = arr[j].clipPath;
            }
            lhResources.Load(sound2DArr,
                (i, s, o) =>
                {
                    var audioData = arr[i];
                    var audioSource = m_defaultParent.gameObject.AddComponent<AudioSource>();
                    audioSource.playOnAwake = false;
                    audioSource.clip = o as AudioClip;
                    audioSource.outputAudioMixerGroup = mixerDic[audioData.mixerName].FindMatchingGroups(audioData.mixerGroup)[0];
                    if (!sound2DLibrary.ContainsKey(audioType))
                    {
                        var dic = new Dictionary<string, AudioSource>();
                        dic.Add(audioData.name, audioSource);
                        sound2DLibrary.Add(audioType, dic);
                    }
                    else
                        sound2DLibrary[audioType].Add(audioData.name, audioSource);
                }, onLoadOver);
        }
    }
}