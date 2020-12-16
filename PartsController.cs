using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PartsController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView pv = null;

    private Transform tr;
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currPos = tr.position;
        currRot = tr.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
        {
            if (10.0f < (tr.position - currPos).magnitude)
            {
                tr.position = currPos;
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
