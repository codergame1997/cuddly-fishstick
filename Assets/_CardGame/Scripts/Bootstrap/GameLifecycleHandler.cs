using UnityEngine;
using Zenject;

public class GameLifecycleHandler : MonoBehaviour
{
    private GameModel _model;
    private SaveService _save;

    [Inject]
    public void Construct(GameModel model, SaveService save)
    {
        _model = model;
        _save = save;
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            _save.SaveGame(_model.ToSaveData());
        }
    }

    void OnApplicationQuit()
    {
        _save.SaveGame(_model.ToSaveData());
    }
}