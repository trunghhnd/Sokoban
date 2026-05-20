using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;
using Firebase;
using Firebase.Analytics;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap objectMap;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase targetTile;
    [SerializeField] private TileBase boxTile;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject UIplay;
    [SerializeField] private GameObject UIwin;
    [SerializeField] private TextMeshProUGUI textStage;

    public static int currentLevel = 1;

    private bool isWin = false;
    public int moveCount = 0;
    public int undoCount = 0;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UIplay.SetActive(true);
        UIwin.SetActive(false);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                AnalyticsManager.LogLevelStart(currentLevel);
            }
            else
            {
                Debug.LogError("Firebase không khởi tạo được: " + task.Result);
            }
        });
        textStage.text = $"STAGE {currentLevel}";
        LevelLoader.Load(currentLevel, groundMap, objectMap,
                         wallTile, floorTile, targetTile, boxTile, player);
    }

    public void CheckWin()
    {
        if (isWin) return;

        groundMap.CompressBounds();

        foreach (Vector3Int pos in groundMap.cellBounds.allPositionsWithin)
        {
            if (groundMap.GetTile(pos) != targetTile) continue;
            if (objectMap.GetTile(pos) != boxTile) return;
        }

        isWin = true;
        LevelProgress.Unlock(currentLevel);
        AnalyticsManager.LogLevelComplete(currentLevel);
        UIplay.SetActive(false);
        UIwin.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int next = currentLevel + 1;
        if (Resources.Load<TextAsset>("Levels/Level" + next) == null) return;

        currentLevel = next;
        SceneManager.LoadScene("Level");
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
