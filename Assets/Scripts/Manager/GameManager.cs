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

}
