[System.Serializable]
public class Command
{
    public enum TYPE
    {
        LOAD_IMAGE,
        GOTO_SCENE,
        CHANGE_ATMOSPHERE,
        PLAY_SOUND
    }

    public TYPE type;
    public string commandText;
}

