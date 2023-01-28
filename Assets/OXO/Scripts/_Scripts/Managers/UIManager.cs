using System;
using MuhammetInce.DesignPattern.Singleton;
using TMPro;
using UnityEngine;
using DG.Tweening;
using MuhammetInce.HelperUtils;

public class UIManager : LazySingleton<UIManager>
{
    public TextMeshProUGUI playerMoneyText;
    public GameObject levelEndCanvas;
    public float levelEndDuration;

    public GameObject tapToPlay;

    void Update() => UpdateInit();
    
    private void UpdateInit()
    {
        playerMoneyText.text = $"{Formatter.Format(ManagerMoney.Instance.currentMoney)}$";
    }

    public void EndCanvasEffect()
    {
        levelEndCanvas.SetActive(true);
        levelEndCanvas.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        levelEndCanvas.transform.DOScale(Vector3.one, levelEndDuration).SetEase(Ease.InExpo);
    }
}
