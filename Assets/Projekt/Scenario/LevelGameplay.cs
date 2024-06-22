using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameplay : Level
{
    [SerializeField] private int menuId;
    [SerializeField] private int mapMaxSize = 30;

    [Space]
    [SerializeField] private CarHandle carHandle;
    [SerializeField] private GameObject blockageContainer;
    [SerializeField] private GameObject blockadeVisual;

    //public void Setup(MapParam mParam)
    //{
    //    carHandle.OnSetup(mParam);
    //}
    private List<GameObject> allBlockadesVisual = new List<GameObject>();
    private void Awake()
    {
        int value = (mapMaxSize - 1) * mapMaxSize * 4;

        for (int i = 0; i < value; i++)
        {
            GameObject blockade = Instantiate(blockadeVisual, blockageContainer.transform);
            blockade.SetActive(false);
            allBlockadesVisual.Add(blockade);
        }

        carHandle.SetupBlockades(allBlockadesVisual);

    }



    public override void OnStart(MapParam m)
    {
        base.OnStart(m);
        gameObject.SetActive(false);
        carHandle.OnSetup(mapParam);


    }

    public override void OnEnter()
    {
        base.OnEnter();
        carHandle.OnStart();
    }
    public override void OnExit()
    {
        base.OnExit();
        for (int i = 0; i < allBlockadesVisual.Count; i++)
        {
            allBlockadesVisual[i].SetActive(false);
        }
    }
    public override void OnUpdate()
    {
        carHandle.OnUpdate();
    }

    public void OnBackClick()
    {
        ChangeLevel(menuId);
    }




}
