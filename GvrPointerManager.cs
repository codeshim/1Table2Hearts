using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointerState
{
    Gaze,
    Fill,
    None,
}

public class GvrPointerManager : MonoBehaviour
{
    const float maxDistance = 10;
    public GameObject gazedAtObject = null;
    Vector3 gazedSpot = Vector3.zero;
    float holdSec = 1.0f;

    // Pointer Modules
    public GameObject reticleObj = null;
    public GameObject fillRndObj = null;
    MeshRenderer reticleRend = null;
    GvrReticlePointer reticlePointer = null;
    GvrFillRoundPointer fillRndPonter = null;

    public static PointerState pointerState = PointerState.Gaze;
    PointerState prevState = PointerState.Gaze;
    public static float waitSec = 2.0f;

    // Hand to pick up
    public GameObject hand = null;
    HandController handCtrl = null;

    // Start is called before the first frame update
    void Start()
    {
        if(reticleObj != null)
        {
            reticlePointer = reticleObj.GetComponent<GvrReticlePointer>();
            reticleRend = reticleObj.GetComponent<MeshRenderer>();
        }

        if (fillRndObj != null)
        {
            fillRndPonter = fillRndObj.GetComponent<GvrFillRoundPointer>();
        }

        if (hand != null)
        {
            handCtrl = hand.GetComponent<HandController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastUpdate();
        PickupOrder();
    }

    void RaycastUpdate()
    {
        if (pointerState == PointerState.None)
            return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            if ((hit.collider.gameObject.tag == "parts" && handCtrl.handState == HandState.Pick) ||
                (hit.collider.gameObject.tag == "spot" && handCtrl.handState == HandState.None))
                return;

            //Debug.Log(hit.collider.gameObject.name);
            if (gazedAtObject == hit.collider.gameObject &&
                pointerState == PointerState.Gaze)
            {
                holdSec -= Time.deltaTime;
                if (holdSec < 0.0f)
                {
                    if (hit.collider.gameObject.tag == "arrow")
                    {
                        handCtrl.RotateMobile(gazedAtObject);
                        return;
                    }
                    pointerState = PointerState.Fill;
                }
            }
            else
            {
                if (gazedAtObject != hit.collider.gameObject &&
                pointerState == PointerState.Fill)
                {
                    pointerState = PointerState.Gaze;
                }
                gazedAtObject = hit.collider.gameObject;
                gazedSpot = hit.point;
                reticlePointer.OnPointerExit();
                reticlePointer.OnPointerEnter();
                holdSec = 1.0f;
            }
        }
        else
        {
            pointerState = PointerState.Gaze;
            gazedSpot = Vector3.zero;
            gazedAtObject = null;
            reticlePointer.OnPointerExit();
            holdSec = 1.0f;
        }
    }

    void PickupOrder()
    {
        if (pointerState == PointerState.Gaze)
        {
            if (!reticleRend.enabled)
            {
                reticleRend.enabled = true;
            }

            if (fillRndObj.activeSelf)
            {
                fillRndPonter.OnPointerExit();
                fillRndObj.SetActive(false);
            }
        }
        else if (pointerState == PointerState.Fill)
        {
            if (reticleRend.enabled)
            {
                reticleRend.enabled = false;
            }

            if (!fillRndObj.activeSelf)
            {
                fillRndObj.SetActive(true);
            }

            fillRndPonter.OnPointerHover();
            if (prevState == PointerState.Fill && pointerState == PointerState.Gaze)
            {
                if (handCtrl != null && handCtrl.handState == HandState.None && gazedAtObject != null)
                {
                    // Pick up
                    handCtrl.Pickup(gazedAtObject);
                }
                else if(handCtrl != null && handCtrl.handState == HandState.Pick && gazedSpot != Vector3.zero)
                {
                    // Attach parts
                    handCtrl.Attach(gazedSpot);
                }
            }
        }
        else
        {
            if (reticleRend.enabled)
            {
                reticleRend.enabled = false;
            }

            if (fillRndObj.activeSelf)
            {
                fillRndPonter.OnPointerExit();
                fillRndObj.SetActive(false);
            }
        }

        prevState = pointerState;
    }
}
