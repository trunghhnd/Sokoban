using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelIndex;
    [SerializeField] private Button button;
    [SerializeField] private Image buttonImage;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayLevel()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
