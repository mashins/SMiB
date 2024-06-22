using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class LevelMenu : Level
{
    [SerializeField] private int gameplayID;
    [SerializeField] private int mapSizeMax = 500;

    [Header("General")]
    [SerializeField] private MapGeneration map;

    [Header("GUI")]
    [SerializeField] private TMP_InputField mapSizeInputField;
    [SerializeField] private TMP_InputField startXInputField;
    [SerializeField] private TMP_InputField startYInputField;
    [SerializeField] private TMP_InputField endXInputField;
    [SerializeField] private TMP_InputField endYInputField;
    [SerializeField] private TMP_InputField probabilityInputField;

    public override void OnEnter()
    {
        base.OnEnter();

        PathFinding.instance.InitSize(mapParam.Size);

        mapSizeInputField.onEndEdit.AddListener(OnMapSizeChange);

        startXInputField.onEndEdit.AddListener(OnStartXChange);
        startYInputField.onEndEdit.AddListener(OnStartYChange);

        endXInputField.onEndEdit.AddListener(OnEndXChange);
        endYInputField.onEndEdit.AddListener(OnEndYChange);

        probabilityInputField.onEndEdit.AddListener(OnProbabilityChange);
    }
    public override void OnExit()
    {
        mapSizeInputField.onEndEdit.RemoveListener(OnMapSizeChange);

        startXInputField.onEndEdit.RemoveListener(OnStartXChange);
        startYInputField.onEndEdit.RemoveListener(OnStartYChange);

        endXInputField.onEndEdit.RemoveListener(OnEndXChange);
        endYInputField.onEndEdit.RemoveListener(OnEndYChange);

        probabilityInputField.onEndEdit.RemoveListener(OnProbabilityChange);

        base.OnExit();

    }
    public void OnStartClick()
    {
        ChangeLevel(gameplayID);
    }

    public void OnMapSizeChange(string mapSize)
    {
        if (!int.TryParse(mapSize, out int result)) return;

        result = Mathf.Clamp(Mathf.Abs(result), 0, mapSizeMax);

        mapParam.Size = result;
        map.SetupMap(result);

        OnStartPosChange();
        OnEndPosChange();

        mapSizeInputField.text = result.ToString();
    }

    private void OnStartXChange(string x)
    {
        if (!int.TryParse(x, out int result)) return;

        result = Mathf.Clamp(Mathf.Abs(result), 0, mapParam.Size - 1);

        mapParam.StartIndex = new Vector2Int(result, mapParam.StartIndex.y);
        OnStartPosChange();
    }
    private void OnStartYChange(string y)
    {
        if (!int.TryParse(y, out int result)) return;

        result = Mathf.Clamp(Mathf.Abs(result), 0, mapParam.Size - 1);

        mapParam.StartIndex = new Vector2Int(mapParam.StartIndex.x, result);
        OnStartPosChange();
    }

    private void OnEndXChange(string x)
    {
        if (!int.TryParse(x, out int result)) return;

        result = Mathf.Clamp(Mathf.Abs(result), 0, mapParam.Size - 1);

        mapParam.EndIndex = new Vector2Int(result, mapParam.EndIndex.y);
        OnEndPosChange();
    }
    private void OnEndYChange(string y)
    {
        if (!int.TryParse(y, out int result)) return;

        result = Mathf.Clamp(Mathf.Abs(result), 0, mapParam.Size - 1 );

        mapParam.EndIndex = new Vector2Int(mapParam.EndIndex.x, result);
        OnEndPosChange();
    }

    public void OnStartRandom()
    {
        int x = Random.Range(0, mapParam.Size);
        int y = Random.Range(0, mapParam.Size);

        mapParam.StartIndex = new Vector2Int(x, y);
        OnStartPosChange();
    }
    public void OnEndRandom()
    {
        int x = Random.Range(0, mapParam.Size);
        int y = Random.Range(0, mapParam.Size);

        mapParam.EndIndex = new Vector2Int(x, y);
        OnEndPosChange();
    }

    private void OnStartPosChange()
    {

        Vector2Int position = mapParam.StartIndex;
        map.SetupStart(position, mapParam.Size > 1);

        startXInputField.text = position.x.ToString();
        startYInputField.text = position.y.ToString();
    }
    private void OnEndPosChange()
    {
        Vector2Int position = mapParam.EndIndex;
        map.SetupEnd(position, mapParam.Size > 1);

        endXInputField.text = position.x.ToString();
        endYInputField.text = position.y.ToString();
    }

    public void OnProbabilityRandom()
    {
        float random = Random.Range(0, 1f);

        OnProbabilityChange(random.ToString());
    }

    
    private void OnProbabilityChange(string value)
    {
        if (!float.TryParse(value, out float result)) return;

        result = Mathf.Clamp(result, 0, 1);

        mapParam.BlockProbability = result;
        probabilityInputField.text = mapParam.BlockProbability.ToString();
    }
}
