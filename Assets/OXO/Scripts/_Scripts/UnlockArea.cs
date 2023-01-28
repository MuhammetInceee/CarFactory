using Dreamteck.Splines;
using MuhammetInce.HelperUtils;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UnlockArea : MonoBehaviour
{
    public Slider slider;
    public int startPrice;
    public int priceLeft;
    public bool isBuyed;
    public TMP_Text priceText;
    public GameObject area;
    public GameObject canvas;
    public GameObject worker;
    public ParticleSystem puffEffect;

    public bool goingLeftCircle;

    void Awake()
    {
        priceLeft = PlayerPrefs.GetInt("priceLeft" + gameObject.GetInstanceID(), startPrice);
        priceText.text = priceLeft.ToString();
        slider.minValue = -startPrice;
        slider.maxValue = 0;
        slider.value = -priceLeft;
        if (isBuyed || PlayerPrefs.GetInt("isBuyed" + gameObject.GetInstanceID()) == 1)
        {
            isBuyed = true;
            canvas.SetActive(false);
            if (area != null)
            {
                area.SetActive(true);
                worker.SetActive(true);
            }
        }
        if (!isBuyed)
        {
            canvas.SetActive(true);
            if (area != null)
            {
                area.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        GiveMoney(other);
    }

    private void GiveMoney(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isBuyed && priceLeft > 0 && ManagerMoney.Instance.currentMoney > 0 && (other.gameObject.GetComponent<SplineFollower>().followSpeed < 0.1f))
        {
            priceLeft -= startPrice / 100;
            priceText.text = "" + priceLeft;
            PlayerPrefs.SetInt("priceLeft" + gameObject.GetInstanceID(),priceLeft);
            ManagerMoney.Instance.currentMoney = Mathf.Clamp(ManagerMoney.Instance.currentMoney - (startPrice / 100), 0, 99999);
            if (ManagerMoney.Instance.currentMoney < 1000)
            {
                UIManager.Instance.playerMoneyText.text = "" + ManagerMoney.Instance.currentMoney;
            }
            else
            {
                float sayi = (ManagerMoney.Instance.currentMoney / 1000f);
                string last = sayi.ToString("f2");
                UIManager.Instance.playerMoneyText.text = last + "K";
            }
            ManagerMoney.Instance.currentMoney = Mathf.Clamp(ManagerMoney.Instance.currentMoney, 0, 99999);
            slider.value = -priceLeft;
            PlayerPrefs.SetFloat("Money", ManagerMoney.Instance.currentMoney);
            if (priceLeft < 1)
            {
                Open(other);
            }
        }
    }

    private void Open(Collider other)
    {
        isBuyed = true;
        PlayerPrefs.SetInt("isBuyed" + gameObject.GetInstanceID(), 1);
        priceLeft = 0;
        GetComponent<Collider>().enabled = false;
        canvas.SetActive(false);
        if (area != null)
        {
            area.SetActive(true);
            puffEffect.Play();
            PlayerController.Instance.trolleyObjectList.ListFucker();
            
            PlayerController.Instance.UpgradeSpline();
            other.GetComponent<SplineFollower>().spline = GameController.Instance.SplineComputerList[PlayerPrefs.GetInt("ActiveSpline")];
            
            if(PlayerController.Instance.activeHolder <= 3)
            {
                PlayerController.Instance.UpgradeHolder();
                GameController.Instance.TrolleyHolderHolder[PlayerPrefs.GetInt("ActiveHolder")].SetActive(true);
                GameController.Instance.TrolleyHolderHolder[PlayerPrefs.GetInt("ActiveHolder") - 1].SetActive(false);
            }
            
            if(PlayerPrefs.GetInt("ActiveSpline") == 2 || PlayerPrefs.GetInt("ActiveSpline") == 4)
                other.GetComponent<SplineFollower>().direction = Spline.Direction.Backward;
            else
                other.GetComponent<SplineFollower>().direction = Spline.Direction.Forward;
            
            other.GetComponent<SplineFollower>().Restart();
            worker.SetActive(true);
            PlayerPrefs.SetInt("goLeft", goingLeftCircle ? 1 : 0);
        }
    }
}