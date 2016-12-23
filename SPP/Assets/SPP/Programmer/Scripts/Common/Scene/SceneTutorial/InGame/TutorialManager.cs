using UnityEngine;
using System.Collections;

public class TutorialManager : BaseObject
{


    [SerializeField]
    private GameObject[] m_stagePrefs;
    [SerializeField]
    private RaceTimer m_timer;
    [SerializeField]
    private ResultWindow m_result;
    protected override void mOnRegistered()
    {
    }

    protected override void Start()
    {
        //ECourseType　ステージ生成
        var type = (ECourseType)PlayerPrefs.GetInt(SaveKey.mTutorialKey);
        //Debug.Log("StageType" + type);
        mCreate(m_stagePrefs[(int)type]);

        if(type == ECourseType.straight)
        {
            GameInfo.mInstance.m_wind.mWindDirection = 270;
        }
        if (type == ECourseType.accelerate)
        {
            m_timer.gameObject.SetActive(true);
            m_timer.StartTime();
        }

    }

    public override void mOnUpdate()
    {
        if (GameInfo.mInstance.mIsEnd)
        {
            if (m_timer.gameObject.activeSelf)
            {
                m_timer.StopTime();
            }
            m_result.Open();
            GameInfo.mInstance.mIsEnd = false;
        }
    }


}
