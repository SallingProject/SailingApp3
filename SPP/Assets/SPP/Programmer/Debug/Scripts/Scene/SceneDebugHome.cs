/**************************************************************************************/
/*! @file   SceneDebugHome.cs
***************************************************************************************
@brief      デバッグシーンクラス
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class SceneDebugHome : SceneBase {

    [SerializeField]
    private List<GameObject> m_pageList;
    
    [SerializeField]
    private Button m_prev;

    [SerializeField]
    private Button m_next;

    private int m_pageCount;

    /****************************************************************************** 
    @brief      初期化用。タイミングはAwakeと一緒。BaseObjectの実装
    @return     none
    */
    protected override void Start()
    {
        base.Start();

        m_pageCount = 0;

        m_next.onClick.AddListener(() =>
        {
            m_pageList[m_pageCount].SetActive(false);
            m_pageCount += 1;
            if (m_pageCount >= m_pageList.Count)
            {
                m_pageCount = m_pageList.Count - 1;
            }
            m_pageList[m_pageCount].SetActive(true);

        });

        m_prev.onClick.AddListener(() =>
        {
            m_pageList[m_pageCount].SetActive(false);
            m_pageCount -= 1;
            if(m_pageCount < 0)
            {
                m_pageCount = 0;
            }
            m_pageList[m_pageCount].SetActive(true);
        });
    }
    
    /****************************************************************************** 
    @brief      タイトルへ飛ぶ
    @return     none
    */
    public void mGoToTitle()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Title", LoadInfo.ELoadType.Async, 1f));
    }

    /****************************************************************************** 
    @brief      ホームへ飛ぶ
    @return     none
    */
    public void mGoToHome()
    {
        
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Home", LoadInfo.ELoadType.Async, 1f));
    }

    /****************************************************************************** 
    @brief      ライブラリへ飛ぶ
    @return     none
    */
    public void mGoToLibrary()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Library", LoadInfo.ELoadType.Async, 1f));
    }
    /****************************************************************************** 
    @brief      ゲームに飛ぶ
    @return     none
    */
    public void mGoToGame()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("InGame" , LoadInfo.ELoadType.Async, 1f));
    }

    /****************************************************************************** 
    @brief      耐久テストシーンに飛ぶ
    @return     none
    */
    public void mGoToEndurance()
    {
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Endurance", LoadInfo.ELoadType.Async, 1f));
    }

}
