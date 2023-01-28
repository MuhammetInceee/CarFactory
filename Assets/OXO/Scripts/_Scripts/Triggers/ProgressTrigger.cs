using System.Collections;
using Dreamteck.Splines;
using MuhammetInce.HelperUtils;
using UnityEngine;

public class ProgressTrigger : MonoBehaviour
{
    public GameObject activeCar;
    
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(StopDelay(other));
    }

    private void OnTriggerExit(Collider other)
    {
        HelperUtils.SpeedUp(other.GetComponent<SplineFollower>());
    }

    private IEnumerator StopDelay(Collider other)
    {
        yield return null;
        HelperUtils.StopIt(other.GetComponent<SplineFollower>());
    }


}
