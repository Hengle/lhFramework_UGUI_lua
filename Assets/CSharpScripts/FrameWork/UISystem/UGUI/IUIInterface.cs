using UnityEngine;
using System;
using System.Collections.Generic;

namespace LaoHan.UGUI
{
    public interface IUIInterface
    {
        void Initialize(Action onInitialOver);
        void Open(Intent parameter,Action<Intent> onOpenOver);
        void Close(Action<Intent> onCloseOver);
        void Destroy(Action<Intent> onDestoryOver);
    }
    public interface IUtilityInterface
    {
        void Initialize(Action onInitialOver);
        void Open(Intent parameter, Action<Intent> onOpenOver);
        void Close(Action<Intent> onCloseOver);
        void Destroy(Action<Intent> onDestroyOver);
    }
    public interface IGuideInterface
    {
        void Initialize(List<string> process, List<string> maskList);
    }
    public enum EUIState
    {
        None = 0,
        Initialize = 1,
        Open = 2,
        Close = 3,
        Destroy = 4
    }
    public enum EAudioType
    {
        None,
        ButtonDown,
        ButtonUp,
        ButtonPress,
        ButtonDrag,
        UIOpen,
        UIClose
    }
    public enum EAudioGroup
    {
        None,
        BackGroundMusic,
        ActionAudio,
        SpecialEffectAudio,
        TriggerAudio,
        UIAudio
    }
    public enum GuideTrigger
    {
        None,
        UIInitialize,
        UIOpen,
        UIClose,
        UIDestroy,
        OnClick,
        OnDoubleClick,
        OnPress
    }
}