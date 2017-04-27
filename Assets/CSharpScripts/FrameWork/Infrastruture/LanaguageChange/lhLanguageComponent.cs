using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LaoHan.Infrastruture
{
    public class lhLanguageComponent : lhMonoBehaviour
    {
        public Text text;
        public string languageIndex;
        void OnEnable()
        {
            ChangeLanguage();
        }
        public void ChangeLanguage()
        {
            string str = lhLanguage.GetContent(languageIndex);
            if (string.IsNullOrEmpty(str))
            {
                lhDebug.LogWarning((object)("LaoHan: this.languageIndex is invalid:" + gameObject.name + "/" + languageIndex));
                return;
            }
            text.text = str;
        }
    }
}