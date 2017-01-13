using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedMater : BaseObject{
    [SerializeField]
    private ShipMove m_shipObj;

    private Slider m_sliderObj;
    private float m_range;
    protected override void mOnRegistered()
    {
        m_sliderObj = GetComponent<Slider>();
    }

    public override void mOnUpdate()
    {
        float val = m_shipObj.mSpeedVector / m_shipObj.mMaxSpeed;
        m_sliderObj.value = val;

    }

}
