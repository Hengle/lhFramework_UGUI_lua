using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaoHan.FairyGUI
{
    public interface IUIInterface
    {
        void Initialize(Action onInitialOver);
        void Open(Intent parameter, Action<Intent> onOpenOver);
        void Close(Action<Intent> onCloseOver);
        void Destroy(Action<Intent> onDestoryOver);
    }
}
