using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BasePlayer player;
    public TileManager tileManager;

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
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
    }
    #endregion

    private void Update()
    {
        // TODO 승리 함수 부하를 줄이기 위해 업데이트 외로 뺌
        if (player.currentPosY == (tileManager.yoffsetEnd - 1))
        {
            print("WIN");
        }
    }
}
