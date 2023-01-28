using MuhammetInce.DesignPattern.Singleton;
using UnityEngine;

public class PainterTrigger : LazySingleton<PainterTrigger>
{
    public Material targetMaterial;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            CraneController craneController = other.GetComponent<CraneController>();

            craneController.targetMaterial.color = targetMaterial.color;
        }

    }
}
