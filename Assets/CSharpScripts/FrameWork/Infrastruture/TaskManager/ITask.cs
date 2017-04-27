using UnityEngine;
using System;
using System.Collections.Generic;

namespace LaoHan.Infrastruture
{
    public interface ITask
    {
        float GetPriority();
        void OnAwake();
        void OnTaskComplete();
        void OnTaskRestart();
        void OnEnd();
        void OnPause(bool paused);
        void OnReset();
        void OnStart();
        ETaskStatus OnUpdate();
    }
}
