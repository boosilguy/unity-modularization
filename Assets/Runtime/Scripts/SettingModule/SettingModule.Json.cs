using Newtonsoft.Json;
using System.IO;
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

    private void Serialize<T>(T obj, Stream stream)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented, serializerSettings);
        StreamWriter writer = new StreamWriter(stream);
        writer.Write(json);
        writer.Flush();
    }

    private T Deserialize<T>(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        StreamReader reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json, serializerSettings);
    }
}
