using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using LaoHan.UGUI;
using LaoHan.Network;
using LaoHan.Data;
using System.Net;
using LaoHan.Control;
using LaoHan.Infrastruture;
using System.Reflection;

namespace LaoHan.UGUI
{
    public class UI_Login : lhUIBase
    {
        #region public member
        public InputField accountInput;
        public InputField pwdInput;
        #endregion

        #region private member

        #endregion
        
        #region unity Methods
        void Start()
        {
            lhDebug.Log((object)Application.persistentDataPath);
        }
        void ApplicationQuit()
        {
        }
        #endregion

        #region lhUIBase
        public override void Initialize(Action onInitialOver)
        {
            base.Initialize(onInitialOver);
            lhUIManager.EnterUI("UI_SocketError", (uiBase) =>
            {
                uiBase.Open(null, null);
            }, null, false);
            onInitialOver();
        }
        public override void Open(Intent parameter, Action<Intent> onOpenOver)
        {
            if (lhCacheData.HasHistoryValue("Account"))
                accountInput.text = lhCacheData.GetHistoryValue("Account");
            if (lhCacheData.HasHistoryValue("Password"))
                pwdInput.text = lhCacheData.GetHistoryValue("Password");
            gameObject.SetActive(true);
            lhSceneManager.Load("1", (p) => { Debug.LogError(p); }, () => { Debug.Log("Ok"); },UnityEngine.SceneManagement.LoadSceneMode.Single);
            base.Open(parameter, onOpenOver);
            if (onOpenOver != null)
                onOpenOver(null);
        }
        public override void Close(Action<Intent> onCloseOver)
        {
            base.Close(onCloseOver);
            gameObject.SetActive(false);
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

        #region Button Event
        public void Login()
        {
            //lhUIManager.SoundPlay(EAudioType.ButtonDown);
            SetHistoryValue("", true, true, accountInput.text, pwdInput.text);
            lhUIManager.EnterUI("UI_ServerChoose", (uiBase) =>
            {
                Intent paramDic = new Intent();
                paramDic.PutExtras("account", accountInput.text);
                paramDic.PutExtras("pwd", pwdInput.text);
                uiBase.Open(paramDic, (param) =>
                {
                    lhUIManager.DestroyUI("UI_Login");
                });
            });
        }

        public void OnBtnVisitorLoginClick()
        {

        }

        public void OnBtnQuickRegisterClick()
        {

        }
        #endregion

        #region private methods
        private void SetHistoryValue(string ip, bool rememberPassword, bool autologin, string account, string password)
        {
            lhCacheData.SetHistoryValue("LoginType", "");
            lhCacheData.SetHistoryValue("IP", ip);
            lhCacheData.SetHistoryValue("RememberPassword", rememberPassword.ToString());
            lhCacheData.SetHistoryValue("AutoLogin", autologin.ToString());
            lhCacheData.SetHistoryValue("Account", account);
            lhCacheData.SetHistoryValue("Password", password);
        }

        #endregion
    } 
}