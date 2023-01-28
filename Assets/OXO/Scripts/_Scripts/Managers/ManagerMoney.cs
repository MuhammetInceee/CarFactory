using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using MuhammetInce.DesignPattern.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagerMoney : LazySingleton<ManagerMoney>
{
    private readonly Vector3 _scale = new Vector3(0.8f, 0.8f, 0.8f);
    
    public float currentMoney;

    public float CircleMoney;
    public float SellCarMoney;
    public float NextLevelNeed;
    public TextMeshProUGUI nextLevelCost;

    [SerializeField] private GameObject moneyParent;
    [SerializeField] private Image moneyImage;
    [SerializeField] private List<Image> moneyImageList;
    private void Start()
    {
        currentMoney = PlayerPrefs.GetFloat("Money");
    }

    private void Update()
    {
        AddAndRemoveMoney();
        nextLevelCost.text = $"{NextLevelNeed.ToString()} $";
    }

    public void ChangeMoney(float money)
    {
        currentMoney += money;
        SetPlayerPrefs();
    }

    private void AddAndRemoveMoney()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentMoney += 100;
            SetPlayerPrefs();
        }
        
        else if (Input.GetKeyDown(KeyCode.L))
        {
            currentMoney -= 100;
            SetPlayerPrefs();
        }
        
    }
    public void SetPlayerPrefs() => PlayerPrefs.SetFloat("Money", currentMoney);


    public IEnumerator SpawnMoney(int spawnCount, GameObject obj)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            yield return new WaitForSeconds(0.02f);
            Image money = Instantiate(moneyImage,moneyParent.transform);
            RectTransform moneyRect = money.GetComponent<RectTransform>();
            moneyRect.eulerAngles = new Vector3(moneyRect.eulerAngles.x, moneyRect.eulerAngles.y, Random.Range(0, 180));
            money.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
            moneyRect.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            moneyRect.DOScale(_scale, 0.2f).SetEase(Ease.InBounce).OnComplete((() =>
            {
                money.transform.DOMove(moneyImage.transform.position, 0.2f);
                Destroy(money, 0.2f);
            }));
            
        }
    }
}