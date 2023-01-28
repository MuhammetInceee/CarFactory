using MuhammetInce.HelperUtils;
using Unity.VisualScripting;
using UnityEngine;

public class CarPiece : MonoBehaviour
{
    private bool IsBought => PlayerPrefs.GetInt($"{gameObject.GetInstanceID().ToString()}bought") != 0;

    private int ActiveSpline => PlayerController.Instance.activeSpline;

    private CraneController CraneController => transform.GetComponentInParent<CraneController>();

    private void OnEnable()
    {
        print(gameObject.name + PlayerPrefs.GetInt($"{gameObject.GetInstanceID().ToString()}bought"));
        
        if (IsBought)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void Bought()
    {
        GetComponent<MeshRenderer>().enabled = true;
        CraneController.RemainDeacreaser();

        if (CraneController.RemainingPiece == 0)
        {
            GameController.Instance.AreaProgress[CraneController.id] = false;
            HelperUtils.SpeedUp(CraneController.Follower);
            CraneController.EndProgress();
            ManagerMoney.Instance.ChangeMoney(ManagerMoney.Instance.CircleMoney);
            CraneController.IdUpgrade();

            if (GameController.Instance.SplineComputerList.Count < ActiveSpline) return;

            if (GameController.Instance.isTwoDoorCar)
            {
                CraneController.RemainingPiece =
                    GameController.Instance.SplineComputerList[CraneController.id].name switch
                    {
                        "Wheel" => 4,
                        "Door" => 2,
                        _ => 1
                    };
            }
            else
            {
                CraneController.RemainingPiece =
                    GameController.Instance.SplineComputerList[CraneController.id].name switch
                    {
                        "Wheel" => 4,
                        "Door" => 4,
                        _ => 1
                    };
            }
        }
        PlayerPrefs.SetInt($"{gameObject.GetInstanceID().ToString()}bought", 1);
    }
}
