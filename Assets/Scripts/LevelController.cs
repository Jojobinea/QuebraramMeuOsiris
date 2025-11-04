using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private int maxTurns;
    [SerializeField] private GameObject _victoryUI;
    [SerializeField] private GameObject[] _stars;
    [SerializeField] private TMP_Text _timeTxt;
    [SerializeField] private TMP_Text _turnsText;
    [SerializeField] private TMP_Text _maxTimeText;
    [SerializeField] private TMP_Text _maxTurnsText;
    private float _timeSpent = 0f;
    private int _turns = 0;
    private int totalStars = 1;

    private void Start()
    {
        EventManager.onGameLooseEvent += ReloadScene;
        EventManager.onGameWinEvent += GameOverWin;
        EventManager.OnPhaseIsRotatingEvent += AddToTurn;

        _maxTimeText.text = maxTime.ToString("F2") + "s";
        _maxTurnsText.text = maxTurns.ToString();

        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        EventManager.onGameLooseEvent -= ReloadScene;
        EventManager.onGameWinEvent -= GameOverWin;
        EventManager.OnPhaseIsRotatingEvent -= AddToTurn;
    }

    private void Update()
    {
        _timeSpent += Time.deltaTime;

        _timeTxt.text = "Time: " + _timeSpent.ToString("F2") + "s";
        _turnsText.text = "Turns: " + _turns.ToString();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOverWin()
    {
        Time.timeScale = 0f;

        SetStars();

        _victoryUI.SetActive(true);
    }

    private void AddToTurn()
    {
        _turns += 1;
    }

    private void SetStars()
    {
        if (_timeSpent <= maxTime)
            totalStars += 1;

        if (_turns <= maxTurns)
            totalStars += 1;

        for(int i = 0; i < totalStars; i++)
        {
            _stars[i].SetActive(true);
        }
    }

    public void NextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
