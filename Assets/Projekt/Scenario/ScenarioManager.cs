using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ScenarioManager : MonoBehaviour
{
    //   [SerializeField] private PlayerHandle player;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<Level> levels = new List<Level>();

    private int currentLevelIndex;

    private void Start()
    {
        currentLevelIndex = 0;
 //       player.OnStart();

        //foreach (var level in levels)
        //{
        //    level.Setup(player);
        //}

        levels[currentLevelIndex].OnEnter();
        levels[currentLevelIndex].OnLevelCompleted += OnLevelCompleted;

        levels[currentLevelIndex].OnLevelChanged += OnLevelChanged;

    }

    private void OnLevelCompleted(int levelId)
    {
        levels[currentLevelIndex].OnExit();
        levels[currentLevelIndex].OnLevelCompleted -= OnLevelCompleted;
        levels[currentLevelIndex].OnLevelChanged -= OnLevelChanged;
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Count) currentLevelIndex = 0;

        levels[currentLevelIndex].OnEnter();
        levels[currentLevelIndex].OnLevelCompleted += OnLevelCompleted;
        levels[currentLevelIndex].OnLevelChanged += OnLevelChanged;
    }

    private void OnLevelChanged(int levelId)
    {
        levels[currentLevelIndex].OnExit();
        levels[currentLevelIndex].OnLevelCompleted -= OnLevelCompleted;
        levels[currentLevelIndex].OnLevelChanged -= OnLevelChanged;

        currentLevelIndex = levelId;

        if (currentLevelIndex >= levels.Count) currentLevelIndex = levels.Count - 1;

        levels[currentLevelIndex].OnEnter();
        levels[currentLevelIndex].OnLevelCompleted += OnLevelCompleted;
        levels[currentLevelIndex].OnLevelChanged += OnLevelChanged;
    }

}
