using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

using LaoHan.Infrastruture;
using LaoHan.Network;
namespace LaoHan.UGUI
{
    public class UITool_Video : lhMonoBehaviour
    {
        public RawImage rawImage;
        public AudioSource audioSource;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        private MovieTexture m_movieTexture;
#endif
        public void Initial(string filePath, UnityAction onInitialOver)
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            lhHttp.Get("file:///" + filePath, (www) =>
            {
                m_movieTexture = www.movie;
                Debug.Log(www.bytes.Length + "/" + m_movieTexture.duration + "/" + m_movieTexture.filterMode + "/" + m_movieTexture.texelSize + "/" + m_movieTexture.width + "/" + m_movieTexture.name);
                audioSource.clip = m_movieTexture.audioClip;
                rawImage.texture = m_movieTexture;
                m_movieTexture.loop = true;
                onInitialOver();
            });
#endif
        }
        public void Play()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            m_movieTexture.Play();
            audioSource.Play();
#endif
        }
        public void Pause()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (!m_movieTexture.isPlaying) return;
            m_movieTexture.Pause();
            audioSource.Pause();
#endif
        }
        public void Dispose()
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            rawImage.texture = null;
            m_movieTexture = null;
            audioSource.clip = null;
            Resources.UnloadUnusedAssets();
#endif

        }
        public void Play(string filePath)
        {
#if UNITY_ANDROID || UNITY_INPHONE
            Handheld.PlayFullScreenMovie("file://" + filePath, Color.black, FullScreenMovieControlMode.Full);
#endif
        }
    }
}