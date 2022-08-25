using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    [Header("Status Labels")]
    [SerializeField] private GameObject playerStatus;
    [SerializeField] private GameObject enemiesStatus;

    [Header("Action Buttons")]
    [SerializeField] private Button castMagicButton;
    [SerializeField] private Button selectAttackButton;
    [SerializeField] private Button enemyButton;

    private void Start()
    {
        SelectAction();
    }

    public void ShowPlayerStatus()
    {
        IEnumerator ShowSlower()
        {
            yield return new WaitForSeconds(0.8f);
            playerStatus.SetActive(true);
            enemiesStatus.SetActive(false);
        }

       StartCoroutine(ShowSlower());
    }

    public void ShowEnemiesStatus()
    {
        IEnumerator ShowSlower()
        {
            yield return new WaitForSeconds(0.8f);
            playerStatus.SetActive(true);
            enemiesStatus.SetActive(false);
        }
        StartCoroutine(ShowSlower());
    }

    public void SelectMagic()
    {
        castMagicButton.Select();
    }

    public void SelectAction()
    {
        selectAttackButton.Select();
    }

    public void SelectEnemy()
    {
        enemyButton.Select();
    }
}
