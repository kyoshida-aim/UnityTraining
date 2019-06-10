using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamPriorityChanger : MonoBehaviour {
    
    [SerializeField] private CinemachineVirtualCamera cameraToActivate;
    private const int PriorityHigh = 11;
    private const int PriorityLow = PriorityHigh - 1;


    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "UnityChan")
        {
            cameraToActivate.m_Priority = PriorityHigh;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "UnityChan")
        {
            cameraToActivate.m_Priority = PriorityLow;
        }
    }

}
