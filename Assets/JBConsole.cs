using UnityEngine;

public delegate void MenuHandler();

{
    public const string allChannelsName = " * ";
    public const string defaultChannelName = " - ";
    public const ConsoleLevel defaultConsoleLevel = ConsoleLevel.Debug;

    public bool visible = true;
    public int menuItemWidth = 100;

    string[] topMenu;
    string[] levels;
    string[] channels;
    string[] customMenus;
    Dictionary<int, MenuHandler> customMenuHandlers;

    ConsoleLevel viewingLevel = ConsoleLevel.Debug;
    string[] viewingChannels;

    int currentTopMenuIndex = -1;
    string[] currentTopMenu;
    string[] currentSubMenu;
    SubMenuHandler subMenuHandler;

    string searchTerm = "";

    List<ConsoleLog> logs = new List<ConsoleLog>();
    List<ConsoleLog> cachedLogs;

    Vector2 scrollPosition;
        topMenu = currentTopMenu = Enum.GetNames(typeof(ConsoleMenu));
        levels = Enum.GetNames(typeof(ConsoleLevel));
        channels = new string[] { allChannelsName, defaultChannelName };

        customMenus = new string[0];
        customMenuHandlers = new Dictionary<int, MenuHandler>();

    public void Add(params string[] messages)
    {
        AddCh(defaultConsoleLevel, defaultChannelName, string.Join(" ", messages));
    }
            AddToStringArray(ref channels, channel);
            if (currentTopMenuIndex == (int)ConsoleMenu.Channels)
            {
                UpdateChannelsSubMenu();
            }
        cachedLogs = null;

    public void AddMenu(string name, MenuHandler callback)
    {
        AddToStringArray(ref customMenus, name);
        customMenuHandlers[customMenus.Length - 1] = callback;
    }

    void AddToStringArray(ref string[] strings, string str)
    {
        Array.Resize(ref strings, strings.Length + 1);
        strings[strings.Length - 1] = str;
    }

    string[] StringsWithoutString(string[] strings, string str)
    {
        string[] result;
        int index = Array.IndexOf(strings, str);
        if (index >= 0)
        {
            result = new string[strings.Length - 1];
            Array.Copy(strings, 0, result, 0, index);
            Array.Copy(strings, index + 1, result, index, strings.Length - index - 1);
        }
        else
        {
            result = new string[strings.Length];
            Array.Copy(strings, result, strings.Length);
        }
        return result;
    }
        }
        currentSubMenu = null;
        switch (index)
            case (int)ConsoleMenu.Channels:
                UpdateChannelsSubMenu();
                subMenuHandler = OnChannelClicked;
            case (int)ConsoleMenu.Levels:
                UpdateLevelsSubMenu();
                subMenuHandler = OnLevelClicked;
            case (int)ConsoleMenu.Search:
                searchTerm = "";
            case (int)ConsoleMenu.Menu:
                currentSubMenu = customMenus;
                subMenuHandler = OnCustomMenuClicked;
            case (int)ConsoleMenu.Hide:
                visible = false;
                return;
        currentTopMenu = SelectedStateArrayIndex(topMenu, index, true);

    void OnChannelClicked(int index)
    {
        string channel = channels[index];
        if (channel == allChannelsName)
        {
            viewingChannels = null;
        }
        else if (viewingChannels != null)
        {
            if (Array.IndexOf(viewingChannels, channel) >= 0)
            {
                if (viewingChannels.Length > 1) viewingChannels = StringsWithoutString(viewingChannels, channel);
                else viewingChannels = null;
            }
            else
            {
                AddToStringArray(ref viewingChannels, channel);
            }
        }
        else
        {
            viewingChannels = new string[] { channel };
        }
        UpdateChannelsSubMenu();
        cachedLogs = null;
    }

    void OnLevelClicked(int index)
    {
        viewingLevel = (ConsoleLevel)Enum.GetValues(typeof(ConsoleLevel)).GetValue(index);
        UpdateLevelsSubMenu();
        cachedLogs = null;
    }

    void OnCustomMenuClicked(int index)
    {
        if (customMenuHandlers.ContainsKey(index))
        {
            customMenuHandlers[index]();
        }
    }

    void UpdateChannelsSubMenu()
    {
        currentSubMenu = new string[channels.Length];
        Array.Copy(channels, currentSubMenu, channels.Length);
        if (viewingChannels == null)
        {
            SelectedStateArrayIndex(currentSubMenu, 0, false);
        }
        else
        {
            string channel;
            for (int i = channels.Length - 1; i >= 0; i--)
            {
                channel = channels[i];
                if (Array.IndexOf(viewingChannels, channel) >= 0)
                {
                    SelectedStateArrayIndex(currentSubMenu, i, false);
                }
            }
        }
    }

    void UpdateLevelsSubMenu()
    {
        currentSubMenu = SelectedStateArrayIndex(levels, (int)viewingLevel, true);
    }
        if (!visible) return;
        int selection = GUILayout.Toolbar(-1, currentTopMenu, GUILayout.MinWidth(280), GUILayout.MaxWidth(380));
        if (selection >= 0)

        // show search text field.
        if (currentTopMenuIndex == (int)ConsoleMenu.Search)
        {
            GUI.SetNextControlName("SearchTF");
            searchTerm = GUILayout.TextField(searchTerm);
            searchTerm = searchTerm.ToLower();
            
            GUI.FocusControl("SearchTF");
        }
            if (selection >= 0 && subMenuHandler != null)
            {
                subMenuHandler(selection);
            }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, maxwidthscreen);
        ConsoleLog log;
            log = cachedLogs[i];
            // todo, do the filtering in cachedLogs refresh phase.
            if (log.level >= viewingLevel 
                && (viewingChannels == null || Array.IndexOf(viewingChannels, log.channel) >= 0)
                && (searchTerm == "" || log.content.text.ToLower().Contains(searchTerm)))
            {
                GUILayout.Label(log.content, maxwidthscreen);
            }

    string[] SelectedStateArrayIndex(string[] array, int index, bool copy)
    {
        if (index < 0) return array;
        string[] result;
        if (copy)
        {
            result = new string[array.Length];
            Array.Copy(array, result, array.Length);
        }
        else result = array;
        result[index] = "["+ result[index]+"]";
        return result;
    }


public enum ConsoleLevel
{
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}

delegate void SubMenuHandler(int index);

enum ConsoleMenu
{
    Channels,
    Levels,
    Search,
    Menu,
    Hide
}

class ConsoleLog
{
    public ConsoleLevel level;
    public string channel;
    public GUIContent content;
    public float height;

    public ConsoleLog(ConsoleLevel level_, string channel_, string message)
    {
        level = level_;
        channel = channel_;
        content = new GUIContent(message);
    }

    public float GetHeightForWidth(float width)
    {
        if (height <= 0)
        {
            height = GUI.skin.label.CalcHeight(content, width);
        }
        return height;
    }
}