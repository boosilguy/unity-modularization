using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingModuleConstValue
{
    public const string DEFAULT_TAG = "";
    
    public static string DEFAULT_FILENAME_EXTENSION = "Setting.json";
    public static string DEFAULT_EDITOR_SAVE_DIRECTORY = Path.Combine(Application.dataPath, "Resources", "UnityModularization");
    public static string DEFAULT_BUILD_SAVE_DIRECTORY = Path.Combine(Application.persistentDataPath, "UnityModularization");
}