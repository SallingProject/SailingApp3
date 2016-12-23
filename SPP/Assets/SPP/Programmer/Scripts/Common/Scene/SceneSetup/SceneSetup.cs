//=============================================================================
/**
	@file	SceneSetup.cs
	@brief	アプリケーションの詳細設定などを行う
	@author	Ko Hashimoto
*/
//=============================================================================

using UnityEngine;
using System.Collections;

public class SceneSetup : SceneBase {


    protected override void mOnRegistered()
    {
        base.mOnRegistered();

        /* アプリの環境設定 */

        // フレームレート設定
        Application.targetFrameRate = 60;

        // 垂直同期
        // ティアリングが起きたら別の値にしようかな
        QualitySettings.vSyncCount = 0;

       
        // 画面の回転する向きを固定
        Screen.autorotateToPortrait             = true;   // 縦
        Screen.autorotateToPortraitUpsideDown   = true;   // 上下逆
        Screen.autorotateToLandscapeLeft        = false;  // 左
        Screen.autorotateToLandscapeRight       = false;  // 右
    }
    
    protected override void Start()
    {
        base.mOnUpdate();
#if SPP_DEBUG
        GameInstance.mInstance.mSceneLoad(new LoadInfo("DebugHome", LoadInfo.ELoadType.Sync, 1));
#else
        GameInstance.mInstance.mSceneLoad(new LoadInfo("Title", LoadInfo.ELoadType.Sync, 1));
#endif

    }
}
