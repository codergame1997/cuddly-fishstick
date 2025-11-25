public interface ISaveService
{
    void SaveGame(GameSaveData data);
    GameSaveData LoadGame();
    void DeleteSave();
}