using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            other.GetComponent<CraneController>().CheckCar();
        }
    }
}
