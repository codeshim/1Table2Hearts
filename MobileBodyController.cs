using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MobileBodyController : MonoBehaviourPunCallbacks, IPunObservable
{
    private PhotonView pv = null;

    private Transform tr;
    private Quaternion currRot = Quaternion.identity;

    void Awake()
    {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currRot = tr.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
        {
            tr.rotation = Quaternion.Slerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.rotation);
        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
