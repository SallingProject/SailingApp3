/**************************************************************************************/
/*! @file   GameInstance.cs
***************************************************************************************
@brief      ゲームインスタンスクラス　シングルトン実装
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/******************************************************************************* 
@brief   シーン読み込み詳細設定用
*/
public class LoadInfo
{

    public enum ELoadType
    {

        // 基本的に使用しない
        _Async = 0x0000001,
        _Sync = 0x0000004,
        _Loadbar = 0x0000008,

        // こちらを使用する
        Async           = _Async,
        Sync            = _Sync,
        AsyncLoadBar    = Async | _Loadbar,

    }

    public Color _fadeColor = Color.black;  // フェードの色
    public string _nextSceneName = "";      // 次のシーンの名前
    public ELoadType _type;                 // 読み込み方法の指定
    public float _loadedWaitTime = 0.0f;    // ロード後の待機時間
    public LoadInfo(string sceneName, Color fadeColor, ELoadType type = ELoadType.Async,float waitTime = 0.5f)
    {
        _nextSceneName = sceneName;
        _fadeColor = fadeColor;
        _type = type;
        _loadedWaitTime = waitTime;
    }

    public LoadInfo(string sceneName, ELoadType type, float waitTime = 0.5f)
    {
        _nextSceneName = sceneName;
        _fadeColor = Color.black;
        _type = type;
        _loadedWaitTime = waitTime;
    }

    public LoadInfo(string sceneName)
    {
        _nextSceneName = sceneName;
        _fadeColor = Color.black;
        _type = ELoadType.Async;
        _loadedWaitTime = 1.0f;
    }
}

/******************************************************************************* 
@brief   ゲームインスタンスクラス　シングルトン実装
*/
public class GameInstance : BaseObjectSingleton<GameInstance> {

    const float kFadeAlphaValue = 1f;
    const float kCompleateLoad = 0.9f;
    const float kFadeTimeSpeed = 1.75f;
    
    [System.Serializable]
    class Loadbar
    {
        public GameObject _root;  
        public Image      _bar;
        public Text       _text;

    }

    public float mLoadProgress
    {
        get;
        private set;
    }

    // BaseObjectのアップデーター
    [SerializeField]
    private GameObject m_managerPrefbs;
    private GameObject m_manager;

    // スタティックキャンバス
    [SerializeField]
    private GameObject m_staticCanvas;
    public GameObject mStaticCanvas
    {
        get { return m_staticCanvas; }
        private set { m_staticCanvas = value; }
    }

    [SerializeField]
    private DebugManager m_debugManagerPref;
    private DebugManager m_debugManager;
    
    [SerializeField]
    private Loadbar m_loadbar = new Loadbar();
    
    [SerializeField]
    private Image m_fade;
    
    public EShipType mShipType
    {
        get;
        set;
    }

    /****************************************************************************** 
    @brief      初期化用。タイミングはAwakeと一緒。BaseObjectの実装
    @return     none
    */
    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        //初期化するべきオブジェクトの初期化や生成など
        if (m_manager == null)
        {
            m_manager = mCreate(m_managerPrefbs) as GameObject;
            m_manager.transform.SetParent(this.transform, false);
        }
        

        if (m_debugManager == null)
        {
            m_debugManager = mCreate(m_debugManagerPref) as DebugManager;
            m_debugManager.transform.SetParent(this.transform, false);
        }

        m_fade.gameObject.SetActive(false);
        m_loadbar._root.SetActive(false);
    }

    /****************************************************************************** 
    @brief      シーンロード
    @param[in]  遷移情報
    @return     none
    */

    public void mSceneLoad(LoadInfo info)
    {
        StartCoroutine(mOnSceneLoad(info));
    }

    

    /****************************************************************************** 
    @brief      非同期シーンの実処理
    @param[in]  次のシーンの名前
    @return     none
    */
    IEnumerator mOnSceneLoad(LoadInfo info)
    {
        mLoadProgress = 0;
        m_fade.gameObject.SetActive(true);

        m_fade.color = new Color(info._fadeColor.r, info._fadeColor.g, info._fadeColor.b, 0);
        // フェードイン
        while (m_fade.color.a < 1)
        {

            m_fade.color += new Color(0, 0, 0, kFadeAlphaValue * (kFadeTimeSpeed * Time.deltaTime));
            yield return null;
        }
        m_fade.color = new Color(info._fadeColor.r, info._fadeColor.g, info._fadeColor.b, 1);

        // ロードバーを使うか
        bool isActivebar = ((info._type & LoadInfo.ELoadType._Loadbar) == LoadInfo.ELoadType._Loadbar);
        if (isActivebar)
        {
            m_loadbar._root.gameObject.SetActive(isActivebar);
            m_loadbar._bar.fillAmount = 0f;
            m_loadbar._text.text = "読みこみ中";
        }

        yield return null;
        
        if ((info._type & LoadInfo.ELoadType.Async) == LoadInfo.ELoadType.Async)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(info._nextSceneName);
            async.allowSceneActivation = false;

            while (async.progress < kCompleateLoad)
            {
                mLoadProgress = async.progress;

                m_loadbar._text.text = async.progress.ToString();
                m_loadbar._bar.fillAmount = async.progress;
                yield return new WaitForEndOfFrame();
            }

            async.allowSceneActivation = true;      // シーン遷移許可
            yield return async;

        }
        else
        {
            SceneManager.LoadScene(info._nextSceneName);
        }
        
        m_loadbar._bar.fillAmount = 1f;

        yield return new WaitForEndOfFrame();
        m_loadbar._root.SetActive(false);

        // フェードアウト
        while (m_fade.color.a > 0)
        {
            m_fade.color -= new Color(0, 0, 0, kFadeAlphaValue * (kFadeTimeSpeed * Time.deltaTime));
            yield return null;
        }
 
        m_fade.color = new Color(info._fadeColor.r, info._fadeColor.g, info._fadeColor.b, 0);
        m_fade.gameObject.SetActive(false);
        
    }
}
