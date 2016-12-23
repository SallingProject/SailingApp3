/**************************************************************************************/
/*! @file   Point.cs
***************************************************************************************
@brief      ポイントの判定
***************************************************************************************
@author     Kaneko Kazuki
***************************************************************************************/
using UnityEngine;
using System.Collections;


public class Point : BaseObject{
    [SerializeField]
    private GameObject m_detectionPrefab;   //当たり判定用プレハブ

    public enum eBuoyType
    {
        Straight,Curve
    }
    [SerializeField]
    public eBuoyType m_buoyType;

    [System.Serializable]
    public class BuoyDetermination
    {
        public enum eDirection
        {
            Back = -1, NotUse = 0 ,Forward = 1,
        }

        [Range(-180,180)]
        public float m_angle;     //始まりの角度
        public eDirection m_direction = eDirection.NotUse;     //方向
        public string m_name = "";
    }

    [SerializeField]
    private BuoyDetermination[] m_determination;

    public int[] m_pointId;

    [SerializeField]
    private float m_radius;         //サークル半径


    private GameObject[] m_angleObject;
    private PointArrayObject m_pointArray;

    private bool m_stayArea;    //エリア内フラグ
    private int m_index;
    private const float mk_scaleY = 0.01f;  //縦固定値

    private bool m_initialized = false;

    protected override void mOnRegistered()
    {
        mUnregisterList(this);
    }

    /****************************************************************************** 
    @brief      初期化用関数
    */
    public void mInitializer()
    {
        m_pointArray = GameInfo.mInstance.m_pointArray;
        //オブジェクトの生成
        m_angleObject = new GameObject[m_determination.Length];
        for (int i = 0; i < m_determination.Length; i++)
        {
            if (m_determination[i].m_direction != 0)
            {
                mCollisionCreate(m_determination[i], out m_angleObject[i]);
            }
        }

        m_index = 0;
        transform.GetComponent<SphereCollider>().radius = m_radius * 2;

        //管理配列に登録
        foreach (var i in m_pointId)
        {
            m_pointArray.mRegisterArray(i, this);
        }
        enabled = false;
        m_initialized = true;
    }
    /****************************************************************************** 
    @brief      ポイント判定用板の生成 （簡略化用）
    @in         インスペクターから受け取る配置角度、向きなど
    @return     生成されたオブジェクト
    */
    private void mCollisionCreate(BuoyDetermination buoy,out GameObject receive)
    {
        var Obj = mCreate(m_detectionPrefab);
        receive = Obj;
        receive.transform.parent = transform;
        if (m_buoyType == eBuoyType.Straight)
        {
            receive.transform.localScale = new Vector3(2, mk_scaleY, m_radius*1.5f);
            receive.transform.Rotate(0, buoy.m_angle, 0);
            receive.transform.localPosition = Vector3.zero;
        }else{
            receive.transform.localScale = new Vector3(4, mk_scaleY, m_radius*1.5f);
            receive.transform.Rotate(0, buoy.m_angle, 0);
            receive.transform.localPosition = Vector3.zero;
            receive.transform.Translate(0, 0, m_radius-1);
        }
        receive.transform.name = buoy.m_name;
        receive.GetComponent<CollisionDetection>().mDirection = (int)buoy.m_direction;

    }

    public IEnumerator mUpdate()
    {
        if (!m_initialized) yield return null;
            while (m_stayArea) {
            //すべて通っていたら次へ
            if (m_angleObject[m_index].GetComponent<CollisionDetection>().mIsEntered)
            {
                if (m_angleObject.Length - 1 == m_index)
                {
                    m_pointArray.mNext();
                    yield break;
                }
                m_index++;
            }

            yield return null;
        }
        foreach (var obj in m_angleObject)
        {
            obj.GetComponent<CollisionDetection>().mIsEntered = false;
        }
        m_index = 0;


    }


    //ポイント周囲の空間にはいっているか
    void OnTriggerEnter(Collider col)
    {
            m_stayArea = true;
            StartCoroutine("mUpdate");
    }
    void OnTriggerExit(Collider col)
    {
        m_stayArea = false;

    }

    private void OnEnable()
    {
        if (m_angleObject == null) return;
        foreach (var obj in m_angleObject)
        {
            obj.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (m_angleObject == null) return;
        foreach (var obj in m_angleObject)
        {
            obj.SetActive(false);
        }
    }

}
