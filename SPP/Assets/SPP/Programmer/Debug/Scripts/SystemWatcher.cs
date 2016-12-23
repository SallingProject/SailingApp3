/**************************************************************************************/
/*! @file   SystemWatcher.cs
***************************************************************************************
@brief      FPSとかDrawCallとかをTextクラスに書き出すクラス
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/******************************************************************************* 
@brief  システム状況をみるためのクラス
*/
public class SystemWatcher : BaseObject
{

    [SerializeField]
    private Text m_fpsLabel;

    [SerializeField]
    private Text m_3dDrawCallLabel;

    [SerializeField]
    private Text m_2dDrawCallLabel;

    private int m_frameCount = 0;
    private int m_oldFrame = 0;

    [System.NonSerialized]
    public GameObject[] mAllObjectInCurrentScene;

    private float m_time = 0.0f;

    protected override void mOnRegistered()
    {
        base.mOnRegistered();

        mAllObjectInCurrentScene = FindObjectsOfType<GameObject>();
    }

    protected override void Start()
    {
        base.Start();
        mUpdateDrawCalls();
    }

    public override void mOnUpdate()
    {
        base.mOnUpdate();

        mUpdateFPS();
    }

    /******************************************************************************* 
    @brief  FPSの更新処理
    */
    private void mUpdateFPS()
    {
        m_frameCount += 1;
        m_time += Time.deltaTime;

        if (m_time < 1.0f) return;

        if (m_oldFrame != m_frameCount)
        {
            if (m_fpsLabel != null)
                m_fpsLabel.text = "FPS:" + m_frameCount;

            m_oldFrame = m_frameCount;
        }
        m_frameCount = 0;
        m_time = 0;

    }

    /******************************************************************************* 
    @brief  ドローコール数の更新処理
    */
    public void mUpdateDrawCalls()
    {
        int drawCall3D = 0;
        int drawCall2D = 0;
        foreach (var obj in mAllObjectInCurrentScene)
        {
            var render = obj.GetComponent<Renderer>();
            if (render != null && render.isVisible)
                drawCall3D += 1;


            var canvasRender = obj.GetComponent<CanvasRenderer>();
            if (canvasRender != null && obj.activeSelf)
                drawCall2D += 1;
        }
        
        if (m_3dDrawCallLabel != null)
            m_3dDrawCallLabel.text = "3D:" + drawCall3D.ToString();

        if (m_2dDrawCallLabel != null)
            m_2dDrawCallLabel.text = "2D:" + drawCall2D.ToString();

        
    }
}
