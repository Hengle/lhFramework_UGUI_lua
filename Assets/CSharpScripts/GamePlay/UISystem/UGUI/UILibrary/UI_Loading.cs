using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LaoHan.UGUI;
using System;
using LaoHan.Battle;
using UnityEngine.UI;

public class UI_Loading : lhUIBase 
{
    public Mask loadingMask;
    public RawImage loadingForeImage;

    private float finalWaitSecs = 1.0f;
    private float progress;

    #region lhUIBase
    public override void Initialize(Action onInitialOver)
    {
        base.Initialize(onInitialOver);
        onInitialOver();
    }
    public override void Open(Intent parameter, Action<Intent> onOpenOver)
    {
        base.Open(parameter, onOpenOver);
        gameObject.SetActive(true);

        if (onOpenOver != null)
            onOpenOver(null);
    }
    public override void Close(Action<Intent> onCloseOver)
    {
        gameObject.SetActive(false);
        base.Close(onCloseOver);
        if (onCloseOver != null)
            onCloseOver(null);
    }
    public override void Destroy(Action<Intent> onDestoryOver)
    {
        base.Destroy(onDestoryOver);
        if (onDestoryOver != null)
            onDestoryOver(null);
    }
    #endregion

    #region unity method
    void Update()
    {
    }
    #endregion

    #region private method
    #endregion
}
