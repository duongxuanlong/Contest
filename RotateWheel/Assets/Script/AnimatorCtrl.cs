using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCtrl : MonoBehaviour
{
    #region params
    Animator mAnimCtrl;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        mAnimCtrl = GetComponent<Animator>();
        mAnimCtrl.keepAnimatorControllerStateOnDisable = true;
    }

    // Update is called once per frame
    #region public methods
    public void PlayAnim (string name, Vector3 pos)
    {
        this.mAnimCtrl.Play(name, -1, 0);
        transform.position = pos;
    }

    public void SetActive (bool isactive)
    {
        gameObject.SetActive(isactive);
    }
    #endregion
}
