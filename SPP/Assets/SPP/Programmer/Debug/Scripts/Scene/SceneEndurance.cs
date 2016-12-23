using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SceneEndurance : SceneBase {

    [System.Serializable]
    class CloneSettings
    {
        [Tooltip("複製するもとになるやつ")]
        public GameObject _rootPrefabs;

        [Tooltip("複製する際の一番上の座標")]
        public Vector3 _topPosition = Vector3.zero;

        [Tooltip("複製する際の間隔となる座標")]
        public Vector3 _rangeValue = Vector3.zero;
    }


    [SerializeField, Tooltip("複製するときの設定")]
    CloneSettings m_settings;

    [SerializeField]
    Camera m_camera;

    [SerializeField]
    RectTransform m_touchPanel;

    [SerializeField]
    SystemWatcher m_systemWatcher;

    [SerializeField]
    Button m_cloneButton;

    [SerializeField]
    Button m_deleteButton;

    
    List<GameObject> m_cloneList = new List<GameObject>();

    protected override void mOnRegistered()
    {
        base.mOnRegistered();
        EventTrigger trigger = m_touchPanel.GetComponent<EventTrigger>();

        // Dragイベントの追加
        {
            EventTrigger.Entry drag = new EventTrigger.Entry();
            drag.eventID = EventTriggerType.Drag;
            drag.callback.RemoveAllListeners();
            drag.callback.AddListener(data =>
            {

                Debug.Log("きた");
                var touch = InputManager.mInstance.mGetTouchInfo();
                m_camera.transform.position += new Vector3(touch.mDeltaPosition.x, 0, touch.mDeltaPosition.y);
            });
            trigger.triggers.Add(drag);
        }
    }

    protected override void Start()
    {
        base.Start();

        m_cloneButton.onClick.AddListener(() =>
        {
            var clone = mCreate(m_settings._rootPrefabs) as GameObject;

            
            clone.transform.position = m_settings._topPosition + (m_settings._rangeValue * m_cloneList.Count);
            m_cloneList.Add(clone);
            
            StartCoroutine(mLateOneFlame());
        });

        m_deleteButton.onClick.AddListener(() =>
        {
            if (m_cloneList.Count <= 0) return;
            
            var last = m_cloneList[m_cloneList.Count - 1];
            m_cloneList.Remove(last);

            mDelete(last);

            for(int i = 0; i < m_cloneList.Count; ++i)
            {
                m_cloneList[i].transform.position = m_settings._topPosition + (m_settings._rangeValue * i);
            }

            StartCoroutine(mLateOneFlame());
        });

    }

    // 1フレーム遅らせてから行う処理
    IEnumerator mLateOneFlame()
    {
        yield return null;

        m_systemWatcher.mAllObjectInCurrentScene = FindObjectsOfType<GameObject>();
        m_systemWatcher.mUpdateDrawCalls();
    }

}
