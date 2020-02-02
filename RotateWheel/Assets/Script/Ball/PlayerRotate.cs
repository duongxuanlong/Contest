using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    #region rotate params
    Vector3 m_ZRotation;
    #endregion

    void Start ()
    {
        m_ZRotation = Vector3.zero;
    }

    // Update is called once per frame
    public void UpdateRotate(float param)
    {
        m_ZRotation = transform.eulerAngles;
        m_ZRotation.z = param;
        transform.Rotate(m_ZRotation);
    }
}
