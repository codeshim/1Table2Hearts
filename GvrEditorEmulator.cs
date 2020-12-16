using System;
using System.Collections.Generic;
using Gvr.Internal;
using UnityEngine;

public class GvrEditorEmulator : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE
    private const string AXIS_MOUSE_X = "Mouse X";
    private const string AXIS_MOUSE_Y = "Mouse Y";

    private static readonly Vector3 NECK_OFFSET = new Vector3(0, 0.075f, 0f);

    private static GvrEditorEmulator instance;
    private static bool instanceSearchedFor = false;

    private static Camera[] allCameras = new Camera[32];

    private float mouseX = 0;
    private float mouseY = 0;
    private float mouseZ = 0;

    public static GvrEditorEmulator Instance
    {
        get
        {
            if (instance == null && !instanceSearchedFor)
            {
                instance = FindObjectOfType<GvrEditorEmulator>();
                instanceSearchedFor = true;
            }

            return instance;
        }
    }

    public Vector3 HeadPosition { get; private set; }

    public Quaternion HeadRotation { get; private set; }

    public void Recenter()
    {
        mouseX = mouseZ = 0;
        UpdateHeadPositionAndRotation();
        ApplyHeadOrientationToVRCameras();
    }

    public void UpdateEditorEmulation()
    {
        bool rolled = false;
        if (CanChangeYawPitch())
        {
            GvrCursorHelper.HeadEmulationActive = true;
            mouseX += Input.GetAxis(AXIS_MOUSE_X) * 5;
            if (mouseX <= -180)
            {
                mouseX += 360;
            }
            else if (mouseX > 180)
            {
                mouseX -= 360;
            }

            mouseY -= Input.GetAxis(AXIS_MOUSE_Y) * 2.4f;
            mouseY = Mathf.Clamp(mouseY, -85, 85);
        }
        else if (CanChangeRoll())
        {
            GvrCursorHelper.HeadEmulationActive = true;
            rolled = true;
            mouseZ += Input.GetAxis(AXIS_MOUSE_X) * 5;
            mouseZ = Mathf.Clamp(mouseZ, -85, 85);
        }
        else
        {
            GvrCursorHelper.HeadEmulationActive = false;
        }

        if (!rolled)
        {
            mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime / (Time.deltaTime + 0.1f));
        }

        UpdateHeadPositionAndRotation();
        ApplyHeadOrientationToVRCameras();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("More than one active GvrEditorEmulator instance was found in your " +
                           "scene.  Ensure that there is only one active GvrEditorEmulator.");
            this.enabled = false;
            return;
        }
    }

    private void Start()
    {
        UpdateAllCameras();
        for (int i = 0; i < Camera.allCamerasCount; ++i)
        {
            Camera cam = allCameras[i];
        }
    }

    private void Update()
    {
        UpdateEditorEmulation();
    }

    private bool CanChangeYawPitch()
    {
        return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
    }

    private bool CanChangeRoll()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    private void UpdateHeadPositionAndRotation()
    {
        HeadRotation = Quaternion.Euler(mouseY, mouseX, mouseZ);
        HeadPosition = (HeadRotation * NECK_OFFSET) - (NECK_OFFSET.y * Vector3.up);
    }

    private void ApplyHeadOrientationToVRCameras()
    {
        UpdateAllCameras();

        for (int i = 0; i < Camera.allCamerasCount; ++i)
        {
            Camera cam = allCameras[i];

            if (cam && cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None)
            {
                cam.transform.localPosition = HeadPosition * cam.transform.lossyScale.y;
                cam.transform.localRotation = HeadRotation;
            }
        }
    }

    private void UpdateAllCameras()
    {
        if (Camera.allCamerasCount > allCameras.Length)
        {
            int newAllCamerasSize = Camera.allCamerasCount;
            while (Camera.allCamerasCount > newAllCamerasSize)
            {
                newAllCamerasSize *= 2;
            }

            allCameras = new Camera[newAllCamerasSize];
        }

        Camera.GetAllCameras(allCameras);
    }

#endif
}