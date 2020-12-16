using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeartController : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView pv = null;

    public GameObject owner = null;
    public Transform ownerTr = null;
    [HideInInspector] public Transform tr = null;
    Material heartMtrl = null;
    Light heartLight = null;
    Transform heartTr = null;

    // Color
    float tempRed = 0.0f;
    float maxRed = 1.0f;
    float minRed = 0.6f;
    float dimVal = 1.5f;

    // Light
    float maxLight = 0.55f;
    float minLight = 0.5f;

    // Size
    float size = 0.2f;
    float changeSize = 0.22f;

    // Pulse
    float ranGap = 0.0f;
    float maxPulse = 0.95f;
    float minPulse = 0.75f;
    [HideInInspector] public float pulseTime = 0.0f;
    [HideInInspector] public float prevTime = 0.0f;

    // Network
    bool once = true;
    float netGap = 0.0f;
    float netTime = 0.0f;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    public void HeartInit()
    {
        tr = this.GetComponent<Transform>();
        heartMtrl = this.GetComponentInChildren<MeshRenderer>().materials[0];
        if (heartMtrl != null)
            heartMtrl.color = new Color(minRed, 0, 0.15f);

        heartLight = this.GetComponentInChildren<Light>();
        if (heartLight != null)
            heartLight.range = minLight;
        //heartLight.range = 0.1f;

        heartTr = this.transform.GetChild(1);
        if (heartTr != null)
            heartTr.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        if (pv.IsMine)
        {
            // Without heartbeat sensor
            ranGap = Random.Range(minPulse, maxPulse);
        }
    }

    public void HeartUpdate()
    {
        if (owner == null)
            return;

        // Without heartbeat sensor
        if (pv.IsMine)
        {
            if (ranGap <= 0)
            {
                ranGap = Random.Range(minPulse, maxPulse);
                HeartBeat();
                pv.RPC("HeartBeat", RpcTarget.Others, null);
                prevTime = pulseTime;
                pulseTime = Time.time;
            }
            else
            {
                ranGap -= Time.deltaTime;
                HeartRender();
            }
        }
        else
        {
            if (pulseTime != netTime)
            {
                prevTime = pulseTime;
                pulseTime = netTime;
            }
            HeartRender();
        }
    }

    [PunRPC]
    void HeartBeat()
    {
        size = changeSize;
        heartMtrl.color = new Color(maxRed, heartMtrl.color.g, heartMtrl.color.b);
        heartLight.range = Mathf.Lerp(heartLight.range, maxLight, 1.0f);
    }

    void HeartRender()
    {
        heartTr.localScale = new Vector3(size, size, size);
        size -= 0.001f;
        if (size < 0.2f)
            size = 0.2f;
        heartLight.range -= dimVal * Time.deltaTime;
        if (heartLight.range < minLight && owner != null)
            heartLight.range = minLight;
        tempRed = heartMtrl.color.r;
        tempRed -= 0.25f * Time.deltaTime;
        if (tempRed < minRed)
            tempRed = minRed;
        heartMtrl.color = new Color(tempRed, heartMtrl.color.g, heartMtrl.color.b);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ranGap);
            stream.SendNext(pulseTime);
        }
        else
        {
            netGap = (float)stream.ReceiveNext();
            netTime = (float)stream.ReceiveNext();
        }
    }
}