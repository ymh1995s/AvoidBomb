using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public BasePlayer player;
    public TileManager tileManager;

    public GameObject startPopup;
    public GameObject losePopup;
    public GameObject winPopup;

    #region SINGLETON
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 중복 제거
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
    }
    #endregion

    private void Start()
    {
        Time.timeScale = 0f;
    }

    private void Update()
    {
        // TODO 승리 함수 부하를 줄이기 위해 업데이트 외로 뺌
        if (player.currentPosY == (tileManager.yoffsetEnd - 1))
        {
            Time.timeScale = 0f;
            LoadWinPopup();
        }
    }

    #region UI 관련
    public void OnButtonStart()
    {
        startPopup.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadLosePopup()
    {
        losePopup.SetActive(true);
    }

    public void LoadWinPopup()
    {
        winPopup.SetActive(true);   
    }

    public void OnButtonRegame()
    {
        SceneManager.LoadScene(0);
    }

    public void OnButtonQuit()
    {
        Application.Quit();
    }

    #endregion UI 관련
}
