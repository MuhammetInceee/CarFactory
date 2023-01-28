using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    private Image _image;
    private void Start()
    {
        _image = GetComponent<Image>();
        
        if (!_image.raycastTarget)
            _image.raycastTarget = true;
    }

    public void NextLevel()
    {
        if (ManagerMoney.Instance.currentMoney < ManagerMoney.Instance.NextLevelNeed) return;
        BoingButton();
        StartCoroutine(NextLevelDelayer());
    }

    public void Exit()
    {
        Actions.OnExitButtonClicked?.Invoke();
    }

    private void BoingButton()
    {
        GameObject targetObj = transform.parent.gameObject;

        targetObj.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.InExpo);
    }

    private IEnumerator NextLevelDelayer()
    {
        for (int i = 0; i < GameController.Instance.AreaProgress.Count; i++)
        {
            GameController.Instance.AreaProgress[i] = false;
        }
        yield return new WaitForSeconds(0.5f);
        ManagerMoney.Instance.ChangeMoney(-ManagerMoney.Instance.NextLevelNeed);
        Actions.OnNextLevelButtonClicked?.Invoke();

    }
}
