using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstScreenList
{
    static readonly string DirectoryPath = "Screen";
    public enum ScreenType
    {
        None,
        Title,
        Main,
        GameOver,
    }


    public static readonly Dictionary<ScreenType, string> ScreenPaths = new Dictionary<ScreenType, string>()
    {
        { ScreenType.Title, $"{DirectoryPath}/TitleScreen" },
        { ScreenType.Main, $"{DirectoryPath}/MainScreen" },
        { ScreenType.GameOver, $"{DirectoryPath}/GameOverScreen" },
    };
}
