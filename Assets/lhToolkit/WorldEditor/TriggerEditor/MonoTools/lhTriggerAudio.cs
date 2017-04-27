using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;
using LaoHan.Battle;

namespace LaoHan.Tools.WorldEditor
{
    public class lhTriggerAudio : lhTriggerBase
    {
        public string[] audioSources;
        void Start()
        {
            //lhSyncManager.playMusicHandler[id] = OnTrigger;
        }
        void OnDestroy()
        {
            //lhSyncManager.playMusicHandler[id] = null;
        }
        //void OnTrigger(PlayMusic protocol)
        //{
        //    foreach (var item in audioSources)
	       // {
        //        lhResources.Load(item, (o) =>
        //        {
        //            //SoundManager.PlayCappedSFX(o as AudioClip,"lhTriggerAudio");
        //        });
	       // }
        //}
    }
}