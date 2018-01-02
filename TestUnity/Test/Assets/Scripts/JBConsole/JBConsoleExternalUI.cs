﻿
public delegate void ExternalUIToolbarButtonPressed(ConsoleMenu? newConsoleMenu);
public delegate void ExternalUIMenuButtonPressed(JBConsoleStateMenuItem menuItem);

public interface JBConsoleExternalUI
{
    void SetActive(bool shouldEnable, JBConsoleState jbConsoleState);
    void AddToolbarButtonListener(ExternalUIToolbarButtonPressed listener);
    void RemoveToolbarButtonListener(ExternalUIToolbarButtonPressed listener);
    void AddMenuButtonListener(ExternalUIMenuButtonPressed listener);
    void RemoveMenuButtonListener(ExternalUIMenuButtonPressed listener);
    void StateChanged(JBConsoleState state);
}