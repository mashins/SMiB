using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameplay : Level
{
    [SerializeField] private int menuId;
    [SerializeField] private CarHandle carHandle;

    public void Setup(MapParam mParam)
    {

        carHandle.OnSetup(mParam);
    }

    public override void OnEnter()
    {
 

    }


    public override void OnExit()
    {

    }



}
