using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public TextMeshProUGUI PlayerMana, EnemyMana;
    public TextMeshProUGUI PlayerHP, EnemyHP;

    public Sprite ActiveManaPoint, InactiveManaPoint;
    public List<GameObject> PlayerManaPoints, EnemyManaPoints;

    public GameObject ResultGO;
    public GameObject pausePanel, settingsPanel;
    public TextMeshProUGUI ResultTxt;

    public TextMeshProUGUI TurnTimeTxt, WhoseTurn;
    public Button EndTurnButton;
    public Button PauseButton;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(this);
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // ��������� ����
        pausePanel.SetActive(true); // ����� ���� �����

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void StartGame()
    {
        EndTurnButton.interactable = true;
        ResultGO.SetActive(false);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);

    }

    public void CloseSettings()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);

    }

    public void UpdateHPAndMana()
    {
        //Updating mana
        PlayerMana.text = GameManagerScr.Instance.CurrentGame.Player.Mana.ToString() + " / " + GameManagerScr.Instance.CurrentGame.Player.Manapool.ToString();
        if (GameManagerScr.Instance.CurrentGame.Player.Mana != 0)
        {
            for (int i = 0; i < GameManagerScr.Instance.CurrentGame.Player.Mana; i++)
            {
                PlayerManaPoints[i].GetComponent<Image>().sprite = ActiveManaPoint;
            }
        }
        if (GameManagerScr.Instance.CurrentGame.Player.Mana != GameManagerScr.Instance.CurrentGame.Player.GetMaxManapool())
        {
            for (int i = GameManagerScr.Instance.CurrentGame.Player.Mana; i < GameManagerScr.Instance.CurrentGame.Player.GetMaxManapool(); i++)
            {
                PlayerManaPoints[i].GetComponent<Image>().sprite = InactiveManaPoint;
            }
        }

        EnemyMana.text = GameManagerScr.Instance.CurrentGame.Enemy.Mana.ToString() + " / " + GameManagerScr.Instance.CurrentGame.Enemy.Manapool.ToString();
        if (GameManagerScr.Instance.CurrentGame.Enemy.Mana != 0)
        {
            for (int i = 0; i < GameManagerScr.Instance.CurrentGame.Enemy.Mana; i++)
            {
                EnemyManaPoints[i].GetComponent<Image>().sprite = ActiveManaPoint;
            }
        }
        if (GameManagerScr.Instance.CurrentGame.Enemy.Mana != GameManagerScr.Instance.CurrentGame.Enemy.GetMaxManapool())
        {
            for (int i = GameManagerScr.Instance.CurrentGame.Enemy.Mana; i < GameManagerScr.Instance.CurrentGame.Enemy.GetMaxManapool(); i++)
            {
                EnemyManaPoints[i].GetComponent<Image>().sprite = InactiveManaPoint;
            }
        }

        //Updating HP
        PlayerHP.text = GameManagerScr.Instance.CurrentGame.Player.HP.ToString();
        EnemyHP.text = GameManagerScr.Instance.CurrentGame.Enemy.HP.ToString();
    }

    public void ShowResult()
    {
        ResultGO.SetActive(true);

        if (GameManagerScr.Instance.CurrentGame.Enemy.HP == 0)
            ResultTxt.text = "Hooraaaay! You won!";
        else
            ResultTxt.text = "Womp-womp... You lost.";
    }

    public void EnableTurnTime(bool enable)
    {
        if (TurnTimeTxt != null)
            TurnTimeTxt.enabled = enable;
    }

    public void UpdateTurnTime(int Time)
    {
        TurnTimeTxt.text = Time.ToString();
    }

    public void WhoseTurnUpdate()
    {
        if (GameManagerScr.Instance.PlayersTurn)
            WhoseTurn.text = "Your turn";
        else
            WhoseTurn.text = "Enemy turn";
    }

    public void EnableTurnBtn()
    {
        EndTurnButton.interactable = GameManagerScr.Instance.PlayersTurn;
    }


}
