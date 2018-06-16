public class GameData {
    private static GameData instance;

    // Konstruktor
    private GameData()
    {
        // Alle Array-Stats werden initialisiert.
        highscore_names = new string[HIGHSCORELIST_LENGTH];
        highscore_points = new int[HIGHSCORELIST_LENGTH];
        itemCollected = new bool[ITEM_AMOUNT];
        ammo = new int[ITEM_AMOUNT];

        if (instance != null)
            return;
        instance = this;
    }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
                instance = new GameData();
            return instance;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------//
    // GameData /------------------------------------------------------------------------------------------------------//
    // Parameter /--------------------------------------------------------//
    // Hier werden die Standardwerte gesetzt.
    private int HIGHSCORELIST_LENGTH = 10;
    private int START_LIVES = 1;
    private int START_HEALTH = 1;
    private int ITEM_AMOUNT = 5;
    private int RESOURCES_AMOUNT = 5;

    // Globale Stats /----------------------------------------------------//
    // Diese Attribute werden spielübergreifend gespeichert.

    // Highscores
    // Die Highscores bestehen aus 2 Listen: Namen und Punkte
    // Auf die kann nicht zugegriffen werden.
    private string[] highscore_names;
    private int[] highscore_points;
    // Man kann jedoch die Listen bekommen.
    public string[] GetHighscoreNames() { return highscore_names; }
    public int[] GetHighscorePoints() { return highscore_points; }
    // Es konnen Einträge hinzugefügt werden.
    // Wenn die neue Punktzahl höher ist als vorhandene, wird sie an der Stelle eingetragen.
    // Alle hinteren Einträge werden nach hintenverschoben.
    public void PushHighscore(string name, int points)
    {
        for(int i = 0; i < highscore_names.Length; i++)
        {
            if(points >= highscore_points[i])
            {
                // Neuer Eintrag wird gespeichert
                string temp_name = highscore_names[i];
                int temp_points = highscore_points[i];
                highscore_names[i] = name;
                highscore_points[i] = points;
                // Die nachfolgenden Einträge werden verschoben
                for(int j = i+1; j < highscore_names.Length-1; j++)
                {
                    name = highscore_names[j];
                    points = highscore_points[j];
                    highscore_names[j] = temp_name;
                    highscore_points[j] = temp_points;
                    temp_name = name;
                    temp_points = points;
                }
                break;
            }
        }
    }

    // Temporäre Stats /--------------------------------------------------//
    // Diese Attribute werden für eine Spielrunde gespeichert.
    // Es wird angenommen, dass es nur einen Spieler gibt.

    // Scores /------------------------//
    // Die aktuelle Punktezahl ist immer zugreifbar.
    public int Scores { get; set; }

    // Lives/Health /------------------//
    // Lives sind die Versuche, die nach jedem Sterben weniger werden.
    // Bei 0 wird GameOver eingeleitet.(Im Player Script)
    public int Lives { get; set; }
    // Health ist der Gesundheitszustand in dem aktuellen Leben.
    // [0,1]
    // Bei 0 wird ein Leben abgezogen und die Gesundheit auf 1 gesetzt. (Im Player Script)
    public float Health { get; set; }

    // Items /-------------------------//
    // Gibt an ob das Item schon einsetzbar ist.
    public bool[] itemCollected;
    // Gibt an welches Item aktiv ist.
    // -1: Kein Item wird benutzt.
    public int ActiveItem { get; set; }
    // Speichert das Ammo der Items
    public int[] ammo;

    // Resources /---------------------//
    // Speichert die Anzahl der Ressource.
    public int[] resources;

    // Stats Methoden /----------------//
    // Alle temporären Stats werden zurückgesetzt.
    public void Reset()
    {
        Scores = 0;
        Lives = START_LIVES;
        Health = START_HEALTH;
        for (int i = 0; i < itemCollected.Length; i++)
            itemCollected[i] = false;
        ActiveItem = -1;
        for (int i = 0; i < ammo.Length; i++)
            ammo[i] = 0;
        for (int i = 0; i < resources.Length; i++)
            resources[i] = 0;
    }
}
