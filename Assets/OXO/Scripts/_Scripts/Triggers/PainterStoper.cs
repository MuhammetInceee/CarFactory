using System;
using MuhammetInce.HelperUtils;
using UnityEngine;

public class PainterStoper : MonoBehaviour
{
    private GameObject _firstCrane;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            _firstCrane = other.gameObject;
            HelperUtils.StopIt(_firstCrane.GetComponent<CraneController>().Follower);
        }
    }

    private void OnDisable()
    {
        if(_firstCrane == null) return;
        HelperUtils.SpeedUp(_firstCrane.GetComponent<CraneController>().Follower);
    }
}
