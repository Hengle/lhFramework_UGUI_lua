using UnityEngine;
using System;
using System.Collections.Generic;
using LaoHan.Infrastruture;
namespace LaoHan.UGUI
{
    public class UIEntry:MonoBehaviour
    {
        public lhInfrastrutureManager m_frastrutureManager;
        void Awake()
        {
            
        }
        void Start()
        {
            //资源更新
            m_frastrutureManager.SourceUpdate(
                ()=> {
                    //初始化数据配置
                    m_frastrutureManager.InitializeGame(
                        () => {
                            //启动游戏界面
                            m_frastrutureManager.StartGame(
                                () => {
                                    //界面启动完毕，销毁自己 
                                    Destroy(gameObject);
                                }
                            );
                        }
                    );
                }
            );
        }
    }
}
