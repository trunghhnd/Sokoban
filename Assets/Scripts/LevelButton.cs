using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;

    void Start()
    {
        int unlocked = LevelProgress.GetUnlockedLevel();

        if (levelIndex <= unlocked)
        {
            button.interactable = true;
            buttonImage.color = Color.white;
        }
        else
        {
            button.interactable = false;
            buttonImage.color = Color.gray;
        }
    }

    public void PlayLevel()
    {
        GameManager.currentLevel = levelIndex;
        SceneManager.LoadScene("Level");
    }
}
