using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst = null;

    [HideInInspector]
    public Dictionary<string, AudioClip> audioClipList =
                        new Dictionary<string, AudioClip>();

    // Background Music
    [HideInInspector] public GameObject bgmObj = null;
    [HideInInspector] public AudioSource bgmSrc = null;

    // GUI Effect Sound
    [HideInInspector] public GameObject GUIObj = null;
    [HideInInspector] public AudioSource GUISrc = null;

    // Game Effect Sound
    private int maxCount = 4;
    [HideInInspector] public int curCount = 0;
    [HideInInspector] public List<GameObject> sndObjList = new List<GameObject>();
    [HideInInspector] public AudioSource[] sndSrcList = new AudioSource[10];

    AudioClip tempClip = null;

    // Start is called before the first frame update
    void Awake()
    {
        LoadChildObject();
        Inst = this;
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void LoadAudioClip(string fileName, AudioClip audioClip)
    {
        if (audioClipList.ContainsKey(fileName) == false)
        {
            audioClipList.Add(fileName, audioClip);
        }
    }

    void LoadChildObject()
    {
        if (bgmObj == null)
        {
            bgmObj = new GameObject();
            bgmObj.transform.SetParent(this.transform);
            bgmObj.transform.localPosition = Vector3.zero;
            bgmSrc = bgmObj.AddComponent<AudioSource>();
            bgmSrc.playOnAwake = false;
            bgmObj.name = "BgMusicObj";
        }

        if (GUIObj == null)
        {
            GUIObj = new GameObject();
            GUIObj.transform.SetParent(this.transform);
            GUIObj.transform.localPosition = Vector3.zero;
            GUISrc = GUIObj.AddComponent<AudioSource>();
            GUISrc.playOnAwake = false;
            GUISrc.loop = false;
            GUISrc.name = "GUISoundObj";
        }

        for (int i = 0; i < maxCount; i++)
        {
            if (sndObjList.Count < maxCount)
            {
                GameObject newObj = new GameObject();
                newObj.transform.SetParent(this.transform);
                newObj.transform.localPosition = Vector3.zero;
                AudioSource newSrc = newObj.AddComponent<AudioSource>();
                newSrc.playOnAwake = false;
                newSrc.loop = false;
                newObj.name = "EffSoundObj";

                sndSrcList[sndObjList.Count] = newSrc;
                sndObjList.Add(newObj);
            }
        }

        // Load All Audio Clips from Resources
        tempClip = null;
        object[] temp = Resources.LoadAll("Sounds");
        for (int i = 0; i < temp.Length; i++)
        {
            tempClip = temp[i] as AudioClip;
            LoadAudioClip(tempClip.name, tempClip);
        }
    }

    public void PlayBGM(string fileName)
    {
        tempClip = null;
        if (audioClipList.ContainsKey(fileName) == true)
        {
            tempClip = audioClipList[fileName] as AudioClip;
        }
        else
        {
            tempClip = Resources.Load("Sounds/" + fileName) as AudioClip;
            audioClipList.Add(fileName, tempClip);
        }

        if (tempClip != null && bgmSrc != null)
        {
            bgmSrc.clip = tempClip;
            bgmSrc.loop = true;
            bgmSrc.Play(0);
        }
    }

    public void PlayGUISound(string fileName)
    {
        tempClip = null;
        if (audioClipList.ContainsKey(fileName) == true)
        {
            tempClip = audioClipList[fileName] as AudioClip;
        }
        else
        {
            tempClip = Resources.Load("Sounds/" + fileName) as AudioClip;
            audioClipList.Add(fileName, tempClip);
        }

        if (tempClip != null && GUISrc != null)
        {
            GUISrc.clip = tempClip;
            GUISrc.loop = false;
            GUISrc.PlayOneShot(tempClip);
        }
    }

    public void PlayEffSound(string fileName)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (sndSrcList[i].clip == null)
                continue;

            if (sndSrcList[i].clip.name == fileName && sndSrcList[i].isPlaying)
                return;
        }

        tempClip = null;
        if (audioClipList.ContainsKey(fileName) == true)
        {
            tempClip = audioClipList[fileName] as AudioClip;
        }
        else
        {
            tempClip = Resources.Load("Sounds/" + fileName) as AudioClip;
            audioClipList.Add(fileName, tempClip);
        }

        if (tempClip != null && sndSrcList[curCount] != null)
        {
            sndSrcList[curCount].clip = tempClip;
            sndSrcList[curCount].loop = false;
            sndSrcList[curCount].Play(0);

            curCount++;
            if (maxCount <= curCount)
                curCount = 0;
        }
    }

    public void StopEffSound(string fileName)
    {
        for (int i = 0; i < maxCount; i++)
        {
            if (sndSrcList[i].clip == null)
                continue;

            if (sndSrcList[i].clip.name == fileName && sndSrcList[i].isPlaying)
                sndSrcList[i].Stop();
        }
    }
}