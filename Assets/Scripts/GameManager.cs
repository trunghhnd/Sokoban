using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap objectMap;
    [SerializeField] private TileBase targetTile;
    [SerializeField] private TileBase boxTile;
    [SerializeField] private GameObject UIplay;
    [SerializeField] private GameObject UIwin;
    bool isWin = false;
    [SerializeField] private MusicManager musicManager;
    private void Start()
    {
        UIplay.SetActive(true);
        UIwin.SetActive(false);
    }
    public void CheckWin()
    {
        if (isWin) return;

        foreach (Vector3Int pos in groundMap.cellBounds.allPositionsWithin)
        {
            if (groundMap.GetTile(pos) != targetTile)
                continue;

            if (objectMap.GetTile(pos) != boxTile)
                return;
        }

        isWin = true;
        ShowWinUI();
        musicManager.PlayWinMusic();
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        LevelProgress.Unlock(currentLevel);
    }
    void ShowWinUI()
    {
        UIplay.SetActive(false);
        UIwin.SetActive(true);

        Time.timeScale = 0f;
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void NextLevel()
    {
        Time.timeScale = 1f;

        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
    }
}
