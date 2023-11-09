using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI recipesDelivererdText;

    private void Start()
    {
        GameManager.Instance.OnStateChanges += GameManager_OnStateChanges;
        Hide();
    }

    private void GameManager_OnStateChanges(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            recipesDelivererdText.text = Mathf.Ceil(DeliveryManager.Instance.GetSuccesfullRecipesAmount()).ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
