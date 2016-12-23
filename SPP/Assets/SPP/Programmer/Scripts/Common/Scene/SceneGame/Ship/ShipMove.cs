/**************************************************************************************/
/*! @file   ShipMove.cs
***************************************************************************************
@brief      船の動きを行うクラス
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ShipMove : BaseObject
{


    public enum EEffectTimeType
    {
        Infnit, // 無限
        Normal, // 通常の状態
    }


    [System.Serializable]
    class CoefficientLift
    {
        [Range(0, 45)]
        public float m_direction_max;
        public AnimationCurve m_curve;
    }

    [SerializeField]
    private CoefficientLift m_cl;
    [SerializeField]
    private CoefficientLift m_cd;


    private WindObject m_wind;

    private float m_speedVector;
    private float m_surfacingRadian;

    private ShipDefine m_shipDefine;
    private float m_accelMagnification;

    [System.NonSerialized]
    public SailRotation m_sail;
    private RudderRotation m_rudder;


    /****************************************************************************** 
    @brief      船に発生した加速度
    */
    public float mMoveForce
    {
        get; private set;
    }

    //定数
    private const float mkFriction = 0.989f;              //摩擦
    private const float mkNormalMagnification = 1.0f;
    private const float mkAirDensity = 1.2f;
    
    public bool mIsInfnit
    {
        private get;
        set;
    }

    public void mInitialize()
    {
        m_speedVector = 0;
        m_surfacingRadian = 0;
        m_wind = GameInfo.mInstance.m_wind;
        mNormalAccel();

        m_rudder = GetComponent<RudderRotation>();
        m_rudder.mHandling = m_shipDefine.mHandling;

        m_sail = GetComponentInChildren<SailRotation>();
    }


    public override void mOnUpdate()
    {
        ///*Test Code
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mItemActivate(ItemEffect.Boost);
        }
        //*/


        //Move
        mAcceleration();
        if (m_speedVector >= m_wind.mWindForce * (m_shipDefine.mMaxSpeed / 100) * m_accelMagnification)
        {
            m_speedVector = m_wind.mWindForce * (m_shipDefine.mMaxSpeed / 100) * m_accelMagnification;
        }

        m_speedVector *= mkFriction;
        transform.Translate(new Vector3(0.0f, 0.0f, m_speedVector * Time.deltaTime));

        ////FloatMove;
        //m_surfacingRadian += Time.deltaTime * 120;
        //transform.position = new Vector3(transform.position.x, Mathf.Sin(m_surfacingRadian / 180 * 3.14f) / 8, transform.position.z);
    }

    /****************************************************************************** 
    @brief      速度の加算　最大値を超えていた場合収めるが風力によって変わる    
    @note       MaxSpeed,Accelaration,
    *******************************************************************************/
    private void mAcceleration()
    {
        float liftForce = mLiftForce();
        float dragForce = mDragForce();
        float direction = 1;


        Vector3 force = new Vector3(liftForce, 0, dragForce);
        {
            Quaternion rote = Quaternion.AngleAxis(m_wind.mWindDirection, Vector3.up);
            float fl = transform.eulerAngles.y - m_wind.mWindDirection;
            if (Mathf.Abs(fl) > 180)
            {
                if (fl < 0)
                {
                    fl = 360 + fl;
                }
                else
                {
                    fl = fl - 360;
                }
            }
            if (fl < 0)
            {
                direction = -1;
            }
            force.x *= direction;
            force = rote * force;

            //ベクトルの正射影
            Vector3 project = Vector3.Project(force, transform.right);
            force = force - project;
        }

        {
            if(Vector3.Dot(force,transform.forward) < 0) { 
                mMoveForce = 0;
                return;
            }
        }

        m_speedVector += (force.sqrMagnitude/10) * (m_shipDefine.mAcceleration / 100) * m_accelMagnification;
        mMoveForce = force.sqrMagnitude;


    }


    /****************************************************************************** 
    @brief      風を受けて力へ変える関数(抗力)
    @return     抗力
    *******************************************************************************/
    private float mDragForce()
    {
        float angle = mAngleAttack(m_wind.mWindDirection, m_sail.transform.eulerAngles.y);
        if (angle >= 90)
        {
            angle = 180 - angle;
        }
        float diff = angle / m_cd.m_direction_max;
        float cd = m_cd.m_curve.Evaluate(diff);

        float DragForce = (m_wind.mWindForce * cd * mkAirDensity);
        return -DragForce;
    }


    /****************************************************************************** 
    @brief      風を受けて力へ変える関数(揚力）
    @return     揚力
    *******************************************************************************/
    private float mLiftForce()
    {
        //風の向きに対してセールが正しい向きをでない場合揚力は発生しない
        float shipFlagment = transform.eulerAngles.y - m_wind.mWindDirection;
        if (Mathf.Abs(shipFlagment) > 180)
        {
            if (shipFlagment < 0)
            {
                shipFlagment = 360 + shipFlagment;
            }
            else
            {
                shipFlagment = shipFlagment - 360;

            }
        }
        float sailFlagment = m_sail.transform.eulerAngles.y - m_wind.mWindDirection;
        if (Mathf.Abs(sailFlagment) > 180){
            if (sailFlagment < 0)
            {
                sailFlagment = 360 + sailFlagment;
            }
            else
            {
                sailFlagment = sailFlagment - 360;

            }
        }

        //Debug.Log(shipFlagment+" "+sailFlagment);
        //９０°辺りはその限りではないので無視させる
        if (Mathf.Abs(shipFlagment) < 90)
        {
            //進行方向とセールの向きが不一致かどうか
            if (sailFlagment < 0 && shipFlagment > 0 || sailFlagment > 0 && shipFlagment < 0)
            {
                //Debug.Log("Error: not Lift");
                return 0.0f;
            }
        }

        //揚力で計算してみる
        //まず迎え角を求める
        //揚力係数を疑似カーブから引っ張る
        float angle = mAngleAttack(m_wind.mWindDirection, m_sail.transform.eulerAngles.y);

        float diff = angle / m_cl.m_direction_max;
        float cl = m_cl.m_curve.Evaluate(diff);
                //Debug.Log("CL" + cl);

        float LiftForce = (m_wind.mWindForce * cl * mkAirDensity) / 2;
        //        Debug.Log("LiftForce" + LiftForce);

        return LiftForce;
    }
    /****************************************************************************** 
    @brief      迎え角を計算する
    @note       fluid   流体,0~360°    target  対象　transform.eulerAngle,
    @return     迎え角
    *******************************************************************************/
    private float mAngleAttack(float fluidDirec, float targetDirec)
    {
        Vector2 fluidVec, targetVec;
        fluidVec = SailMath.mDegToVector2(fluidDirec);
        targetVec = SailMath.mDegToVector2(targetDirec);
        //        Debug.Log("flued" + fluidVec);
        return Mathf.Acos(Vector2.Dot(fluidVec, targetVec)) * Mathf.Rad2Deg;
    }

    /****************************************************************************** 
    @brief      ScriptableObjectを受け取る
    @note       ShipCreateから呼ぶ
    *******************************************************************************/
    public void mSetShipDefine(ShipDefine define)
    {
        m_shipDefine = define;
    }

    /****************************************************************************** 
    @brief      風を受ける加速に変化をつける
    @note       Default 100(%) 
    *******************************************************************************/
    private void mTranslateAccel(float magnification, float time,EEffectTimeType timeType)
    {
        m_accelMagnification = magnification;
        StartCoroutine(mNormalWaitTime(time,timeType));
    }

    /****************************************************************************** 
    @brief      風を受ける加速を元に戻す
    *******************************************************************************/
    private void mNormalAccel()
    {
        m_accelMagnification = mkNormalMagnification;
    }

    /****************************************************************************** 
    @brief      効果時間待ち
    *******************************************************************************/
    private IEnumerator mNormalWaitTime(float time, EEffectTimeType timeType)
    {
        if (timeType == EEffectTimeType.Normal)
        {
            yield return new WaitForSeconds(time);
        }
        else
        {
            while (mIsInfnit)
            {
                yield return null;
            }
        }
        mNormalAccel();
    }


    /****************************************************************************** 
    @brief      タイプを渡されたら処理を行う
    @in         アイテムタイプ
@note       時間も渡すか検討    
    *******************************************************************************/
    public void mItemActivate(ItemEffect type, EEffectTimeType timeType = EEffectTimeType.Normal)
    {
        switch (type)
        {
            case ItemEffect.Invalid:
                mIsInfnit = true;
                mTranslateAccel(0f, 0f,timeType);
                break;
            case ItemEffect.Boost:
                mTranslateAccel(2.0f, 3.0f,timeType);
                break;
            default:
                break;
        }
    }


}
