/**************************************************************************************/
/*! @file   PointMarker.cs
***************************************************************************************
@brief      ポイントマーカーの管理クラス
***************************************************************************************
@author     Ko Hashimoto and Hiroto Morikawa
***************************************************************************************
* Copyright © 2016 Ko Hashimoto All Rights Reserved.
***************************************************************************************/
using UnityEngine;
using System.Collections;

/**************************************************************************************
@brief  ターゲットマーカーの管理クラス  
*/
public class GuideMarker : BaseObject
{

    // Spriteのマーカー
    [SerializeField]
    private GameObject m_spriteMark;

    [SerializeField]
    private GameObject m_ship;

    // スプライトのオフセット
    [SerializeField]
    private Vector3 m_spriteOffset = Vector3.zero;

    // 船の座標値
    private Vector3 m_shipPosition;

    // 目的地の座標値
    private Vector3 m_targetPosition;

    // マーカーの角度を格納する変数
    private float m_markerAngle;

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

    /**************************************************************************************
    @brief  更新処理
    */
    public override void mOnUpdate()
    {
        // ターゲットがnullなら処理を飛ばす
        if (m_target == null) return;

        base.mOnUpdate();

        mSetSpriteMarkActive(m_target.mIsOnView);
        if (m_target.mIsOnView)
        {
            Vector3 position = m_target.transform.position;
            m_spriteMark.transform.position = position + m_spriteOffset;
        }

        mSetTargetPosition();

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
    @brief  マーカーオブジェクトのアクティブ設定
    @note   true時はSpriteのActiveがtrueになって、frontはfalseになる
    */
    private void mSetSpriteMarkActive(bool isSpriteActive)
    {
        m_spriteMark.SetActive(isSpriteActive);
    }

    /**************************************************************************************
    @brief マーカーの表示位置と向きを目的地に向けて設定
    */
    private void mSetTargetPosition()
    {
        //// 座標地を取得
        //Vector3 ship = m_ship.transform.position;
        //Vector3 target = m_target.transform.position;
        //Vector3 guide = this.transform.eulerAngles;

        //// 座標値から角度を測定
        //float dx = target.x - ship.x;
        //float dy = target.y - ship.y;

        //float radian = Mathf.Atan2(dx, dy);
        //float angle = (radian * 180f / Mathf.PI) % 360f;

        //// 画像を角度に合わせて回転
        //this.transform.Rotate(new Vector2(0f, angle));

        // 目的地の座標値を取得
        m_targetPosition = m_target.transform.position;

        // 船の座標値を取得
        m_shipPosition = m_ship.transform.position;

        // マーカーの座標値を船の座標値と同じにする
        this.transform.position = m_shipPosition;

        // 目的地へマーカーを向かせる
        this.transform.LookAt(m_targetPosition);
        transform.position += transform.forward * 5;



        // マーカーを立ててある状態から寝かせる
        this.transform.Rotate(new Vector3(1, 0, 0), 90);
    }
}