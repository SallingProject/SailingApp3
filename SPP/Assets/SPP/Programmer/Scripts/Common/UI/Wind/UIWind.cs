using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class UIWind : BaseObject
{

    [SerializeField]
    private WindObject m_wind;
    [SerializeField]
    private ShipMove m_ship;

    public override void mOnUpdate()
    {
       
        base.mOnUpdate();

        //m_WindDirectionの値分回転
        transform.localEulerAngles = new Vector3(0, m_wind.mWindDirection - m_ship.transform.eulerAngles.y, 0);
    }
}