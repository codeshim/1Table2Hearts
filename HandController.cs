using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandState
{
    None,
    Pick,
}

public class HandController : MonoBehaviour
{
    public HandState handState = HandState.None;
    Transform tr = null;

    // Mobile
    public GameObject mobileRoot = null;
    public GameObject mobileBody = null;
    float rotSpeed = 45f;
    Rigidbody basisBody = null;
    GameObject holdParts = null;
    HingeJoint partsJoint = null;
    Rigidbody partsBody = null;

    // Start is called before the first frame update
    void Start()
    {
        tr = this.transform;

        if (mobileBody != null)
        {
            basisBody = mobileBody.GetComponent<Rigidbody>();
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void RotateMobile(GameObject arrow)
    {
        if (arrow.name == "ClockWiseArrow")
        {
            mobileRoot.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * -1);
        }
        else
        {
            mobileRoot.transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        }
    }

    public void Pickup(GameObject gazeObj)
    {
        if (gazeObj.GetComponent<HingeJoint>() != null)
        {
            Destroy(gazeObj.GetComponent<HingeJoint>());
            gazeObj.GetComponent<Rigidbody>().isKinematic = true;
        }

        holdParts = gazeObj;
        holdParts.transform.SetParent(tr);
        holdParts.transform.localPosition = new Vector3(0, 0, 0);
        holdParts.transform.localRotation = Quaternion.identity;
        handState = HandState.Pick;
    }

    public void Attach(Vector3 gazeSpt)
    {
        if (mobileBody == null)
            return;

        holdParts.transform.position = gazeSpt;
        holdParts.transform.SetParent(mobileBody.transform);
        MobileLink();
        holdParts = null;
        handState = HandState.None;
    }

    void MobileLink()
    {
        partsJoint = holdParts.AddComponent<HingeJoint>();
        partsJoint.connectedBody = basisBody;
        partsBody = holdParts.GetComponent<Rigidbody>();
        partsBody.isKinematic = false;
    }
}
