using UnityEngine;
using UnityEngine.UI;

using System.Collections;


public class RaceTimer : BaseObject {

    private bool m_isStart = false;


    [SerializeField]
    GameObject m_root;
    public GameObject mRoot
    {
        get { return m_root; }
    }
    
    [SerializeField]
    Text m_timeText;
    public string mGetTimeText
    {
        get { return m_timeText.text; }
    }
    
    private System.TimeSpan m_time = System.TimeSpan.Zero;
    public System.TimeSpan mGetTime
    {
        get { return m_time; }
    }



    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        mUnregisterList(this);
    }
    

    public void StartTime()
    {
        m_isStart = true;
        m_time = System.TimeSpan.Zero;
        m_timeText.text = "00:00:00";
        StartCoroutine(OnTimeWatchStart());
    }

    public void StopTime()
    {
        m_isStart = false;
    }

    private IEnumerator OnTimeWatchStart()
    {
        // 開始時間を設定
        System.DateTime startTime = System.DateTime.Now;

        while (m_isStart)
        {
            var diff = System.DateTime.Now - startTime;

            m_timeText.text =
                  ((diff.Minutes <= 10) ? "0" + diff.Minutes.ToString() : diff.Minutes.ToString()) + ":"
                + ((diff.Seconds <= 10) ? "0" + diff.Seconds.ToString() : diff.Seconds.ToString()) + ":"
                + ((diff.Milliseconds <= 10) ? "0" + diff.Milliseconds.ToString()[0] : diff.Milliseconds.ToString()[0].ToString() + diff.Milliseconds.ToString()[1].ToString());

            m_time = diff;
            yield return null;
        }
    }
}
