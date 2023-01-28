using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private bool oneShot;

    
    private List<GameObject> AllCranes => GameController.Instance.AllCranes;

    private void OnTriggerEnter(Collider other)
    {
        if (AllCranes.Contains(other.gameObject))
        {
            AllCranes.Remove(other.gameObject);
            Destroy(other.gameObject);
            if (AllCranes.Count == 0)
            {
                if (oneShot)
                    StartCoroutine(JustStupidFunc());
            }
        }
    }
    private IEnumerator JustStupidFunc()
    {
        yield return null;
        UIManager.Instance.EndCanvasEffect();
        oneShot = true;
    }
}
