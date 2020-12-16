using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPoint
{
    public Transform tr = null;
    public bool isFull = false;
}

public class HeartManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [HideInInspector] public PhotonView pv = null;

    // Environment
    public Light mainLight = null;
    public Material skyMtrl = null;
    public Material oceanMtrl = null;
    [Header("Light Intensity Change")]
    public float[] intensity = new float[2];
    [Header("Sky Color Change")]
    public Color[] skyColors = new Color[2];
    [Header("Ocean Color Change")]
    public Color[] oceanColors = new Color[2];
    IEnumerator coroutine;
    float quick = 1.0f;
    float slow = 0.01f;

    // Hearts
    public GameObject[] heartObjs = new GameObject[2];
    public static List<HeartController> hearts = new List<HeartController>();

    // Clicked!
    [HideInInspector] public float totalTime = 0.0f;
    float saveTime = 0.0f;

    // Creating Parts
    int ratio = 40;
    public GameObject[] parts;
    public GameObject spawnPoint = null;
    List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Inst.PlayBGM("cricket");

        for (int i = 0; i < heartObjs.Length; i++)
        {
            if (heartObjs[i] != null)
            {
                hearts.Add(heartObjs[i].GetComponent<HeartController>());
                hearts[i].HeartInit();
            }
        }

        if (spawnPoint != null)
        {
            Transform[] tempPoints = spawnPoint.GetComponentsInChildren<Transform>();
            for (int i = 0; i < tempPoints.Length; i++)
            {
                SpawnPoint tempPoint = new SpawnPoint();
                tempPoint.tr = tempPoints[i];
                tempPoint.isFull = false;
                spawnPoints.Add(tempPoint);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    SoundManager.Inst.PlayEffSound("reveal");
        //}

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].HeartUpdate();
        }

        PulseMeasure();
    }

    void PulseMeasure()
    {
        if (hearts[0].prevTime == 0 || hearts[1].prevTime == 0 ||
            hearts[0].pulseTime == saveTime)
            return;

        if (Mathf.Abs(hearts[0].pulseTime - hearts[1].pulseTime) < 0.05f)
        {
            if (Mathf.Abs((hearts[0].pulseTime - hearts[0].prevTime) -
                (hearts[1].pulseTime - hearts[1].prevTime)) < 0.1f)
            {
                // Clicked!
                totalTime += hearts[0].pulseTime - hearts[1].prevTime;
                saveTime = hearts[0].pulseTime;
                Debug.Log("click!(" + totalTime + ")");

                ClickedAction();
            }
        }
    }

    void ClickedAction()
    {
        // Environment change
        if (mainLight != null && skyMtrl != null && oceanMtrl != null)
        {
            coroutine = EnvironmentChange();
            StopCoroutine("EnvironmentChange");
            StartCoroutine("EnvironmentChange");
        }

        int luck = Random.Range(0, 100);

        if (luck > ratio)
            return;

        // Creat Parts
        if (PhotonNetwork.IsMasterClient == true)
        {
            int count = 0;
            int pointNum = 0;
            pointNum = Random.Range(0, spawnPoints.Count);
            while (spawnPoints[pointNum].isFull)
            {
                pointNum = Random.Range(0, spawnPoints.Count);
                count++;
                if (count > 10)
                    return;
            }

            if (!spawnPoints[pointNum].isFull && spawnPoints[pointNum].tr != null &&
                parts != null)
            {
                int partsNum = Random.Range(0, parts.Length);
                string partsName = parts[partsNum].name;
                PhotonNetwork.InstantiateRoomObject("Parts/" + partsName, spawnPoints[pointNum].tr.position,
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 0);
                spawnPoints[pointNum].isFull = true;
                SoundManager.Inst.PlayEffSound("reveal");
            }
        }
    }

    IEnumerator EnvironmentChange()
    {
        float tick = 0f;
        while (mainLight.intensity <= intensity[1] && skyMtrl.color != skyColors[1] &&
            oceanMtrl.GetColor("Color_198818EE") != oceanColors[1])
        {
            tick += Time.deltaTime * quick;
            mainLight.intensity = Mathf.Lerp(mainLight.intensity, intensity[1], tick);
            skyMtrl.color = Color.Lerp(skyMtrl.color, skyColors[1], tick);
            oceanMtrl.SetColor("Color_198818EE", Color.Lerp(oceanMtrl.GetColor("Color_198818EE"), oceanColors[1], tick));
            yield return null;
        }

        tick = 0f;
        while (mainLight.intensity >= intensity[0] && skyMtrl.color != skyColors[0] &&
            oceanMtrl.GetColor("Color_198818EE") != oceanColors[0])
        {
            tick += Time.deltaTime * slow;
            mainLight.intensity = Mathf.Lerp(mainLight.intensity, intensity[0], tick);
            skyMtrl.color = Color.Lerp(skyMtrl.color, skyColors[0], tick);
            oceanMtrl.SetColor("Color_198818EE", Color.Lerp(oceanMtrl.GetColor("Color_198818EE"), oceanColors[0], tick));
            yield return null;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }
}
