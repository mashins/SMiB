using UnityEngine;

public class Level : MonoBehaviour
{
 //   protected PlayerHandle player;

    public delegate void LevelDelegate(int levelId);
    public event LevelDelegate OnLevelCompleted;
    public event LevelDelegate OnLevelChanged;

    protected MapParam mapParam;
    //public virtual void Setup(PlayerHandle player)
    //{
    //    this.player = player;
    //}
    public virtual void OnStart(MapParam mParam)
    {
        mapParam = mParam;
    }
    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public virtual void OnExit()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnUpdate()
    {

    }

    protected void CompletedLevel()
    {
        OnLevelCompleted?.Invoke(-1);
    }

    protected void ChangeLevel(int levelId)
    {
        OnLevelChanged?.Invoke(levelId);
    }
}
