using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class VisitorController : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView pv = null;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        //PhotonVoiceController.photonVoiceView = this.GetComponent<PhotonVoiceView>();

        if (pv.IsMine)
        {
            if (this.transform.position == Vector3.zero)
                return;

            PhotonInit.inst.mainCam.transform.parent = this.transform;
            PhotonInit.inst.mainCam.transform.position = this.transform.position;
            PhotonInit.inst.mainCam.transform.rotation = this.transform.rotation;
        }
        else
        {
            if (this.transform.position == Vector3.zero)
                return;

            if (this.transform.eulerAngles.y != 0.0f)
            {
                HeartManager.hearts[0].owner = this.gameObject;
            }
            else
            {
                HeartManager.hearts[1].owner = this.gameObject;
            }
        }
    }

    void Start()
    {
        Vector3 pos = Vector3.zero;
        if (pv.IsMine && this.transform.position == Vector3.zero)
        {
            if (HeartManager.hearts[0].owner == null)
            {
                HeartManager.hearts[0].owner = this.gameObject;
                this.transform.position = HeartManager.hearts[0].ownerTr.position;
                this.transform.rotation = HeartManager.hearts[0].ownerTr.rotation;
            }
            else
            {
                HeartManager.hearts[1].owner = this.gameObject;
                this.transform.position = HeartManager.hearts[1].ownerTr.position;
                this.transform.rotation = HeartManager.hearts[1].ownerTr.rotation;
            }
            PhotonInit.inst.mainCam.transform.parent = this.transform;
            PhotonInit.inst.mainCam.transform.position = this.transform.position;
            PhotonInit.inst.mainCam.transform.rotation = this.transform.rotation;
            this.gameObject.name = "Visitor" + pv.ViewID.ToString();
        }
        else if (!pv.IsMine && this.transform.position == Vector3.zero)
        {
            if (HeartManager.hearts[0].owner == null)
            {
                HeartManager.hearts[0].owner = this.gameObject;
                this.transform.position = HeartManager.hearts[0].ownerTr.position;
                this.transform.rotation = HeartManager.hearts[0].ownerTr.rotation;
            }
            else
            {
                HeartManager.hearts[1].owner = this.gameObject;
                this.transform.position = HeartManager.hearts[1].ownerTr.position;
                this.transform.rotation = HeartManager.hearts[1].ownerTr.rotation;
            }
            this.gameObject.name = "Visitor" + pv.ViewID.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
