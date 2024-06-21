using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : Level
{
    [SerializeField] private int gameplayID;

    public void OnStartClick()
    {
        ChangeLevel(gameplayID);
    }



}
