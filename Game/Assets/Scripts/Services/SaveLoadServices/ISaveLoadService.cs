namespace Services.SaveLoadServices
{
    public interface ISaveLoadService
    {
        public PlayerProgress GetProgress();
        public void Save();
        public void Load();
        public void ClearSave();
    }
}