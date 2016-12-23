/**************************************************************************************/
/*! @file   PointMarker.cs
***************************************************************************************
@brief      ポイントマーカーの管理クラス
***************************************************************************************
@author     Ko Hashimoto
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;

/**************************************************************************************
@brief  ターゲットマーカーの管理クラス  
*/
public class TargetMarker : BaseObject {

    /**************************************************************************************
    @brief  ターゲットマーカーのUIクラス  
    */
    [System.Serializable]
    class CanvasUI
    {
        public RectTransform _right;
        public RectTransform _left;
    }

    [SerializeField]
    private GameObject m_ship;
    // キャンバスに表示するマーカーすべての親
    [SerializeField]
    private RectTransform m_canvasMarkRoot;

    [SerializeField]
    private CanvasUI m_canvasMark;

    // Spriteのマーカー
    [SerializeField]
    private GameObject m_spriteMark;
    
    // スプライトのオフセット
    [SerializeField]
    private Vector3 m_spriteOffset = Vector3.zero;

    // 次のポイント
    // 検証用にSerializeしているが
    // SetTargetで設定されることを想定している
#if UNITY_EDITOR
    [SerializeField]
#endif
    // カメラに映っているかのコンポーネント
    private ReflectedOnCamera m_target;

    // 初期化処理
    protected override void mOnRegistered()
    {
        base.mOnRegistered();
    }


    // なんかちらつくからその対策用
    //　実際の使用用途で消せるかも
    int m_updateCount = 0;
    /**************************************************************************************
    @brief  更新処理
    */
    public override void mOnUpdate()
    {
        // なんかちらつくから最初の2回は無視
        if(m_updateCount < 2)
        {
            m_updateCount += 1;
            return;
        }
        if (m_target == null) return;
        base.mOnUpdate();
        mSetSpriteMarkActive(m_target.mIsOnView);
        if (m_target.mIsOnView)
        {
            Vector3 position = m_target.transform.position;
            m_spriteMark.transform.position = position + m_spriteOffset;
        }
        else
        {
            mSetCanvasMarkActive(m_target.transform.position, m_ship.transform.position);
        }
#if UNITY_EDITOR
        m_target.mUpdate();
#endif
    }

    /**************************************************************************************
    @brief  マーカーのさすオブジェクトを設定
    */
    public void mSetTarget(ReflectedOnCamera nextPoint)
    {
        m_target = nextPoint;
    }

    /**************************************************************************************
    @brief  ターゲットとカメラの位置関係からどのオブジェクトのアクティブをオンにするか決める
    */
    private void mSetCanvasMarkActive(Vector3 target, Vector3 ship)
    {
        Vector3 diff = target - m_ship.transform.position;
        Matrix4x4 mat = Camera.main.worldToCameraMatrix;
        diff = mat * diff;


        // Vector3 diff = target - ship;
        // //Debug.Log(diff.x);
        //// Debug.Log(diff.x * m_ship.transform.forward.x);
        //bool isRight = (diff.x  > 0 || diff.x * m_ship.transform.forward.x > 0 );
        bool isRight = (diff.x > 0);
        m_canvasMark._right.SetActive(isRight);
         m_canvasMark._left.SetActive(!isRight);


    }

    /**************************************************************************************
    @brief  マーカーオブジェクトのアクティブ設定
    @note   true時はSpriteのActiveがtrueになって、frontはfalseになる
    */
    private void mSetSpriteMarkActive(bool isSpriteActive)
    {
        m_spriteMark.SetActive(isSpriteActive);
        
        if (isSpriteActive)
        {
            m_canvasMark._right.SetActive(false);
            m_canvasMark._left.SetActive(false);
        }

        m_canvasMarkRoot.SetActive(!isSpriteActive);

    }
}
