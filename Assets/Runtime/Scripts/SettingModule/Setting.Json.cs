using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SettingModuleManager : ModuleFoundation
{
    private JsonSerializerSettings serializerSettings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    };
}
