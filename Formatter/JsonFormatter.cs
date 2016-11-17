﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using DbRepository.Classes.Keys;
using GraphicsModule.Geometry.Objects;

namespace Formatter
{
    public static class JsonFormatter
    {
        public static void WriteObjectsToJson(Collection<IObject> coll)
        {
            var fullPath = GetPathToJsonFile();
            List<GraphicKey> listGraphObjects = new List<GraphicKey>();
            foreach (var item in coll)
            {
                listGraphObjects.Add(new GraphicKey
                {
                    Guid = Guid.NewGuid(),
                    GraphicObject = item
                });
            }
            string json = JsonConvert.SerializeObject(listGraphObjects.ToArray(), Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            });
            System.IO.File.WriteAllText($@"{fullPath}\GraphicObjects.json", json);
        }

        public static List<GraphicKey> GetGraphicKeysFromJson()
        {
            var fullPath = GetPathToJsonFile();
            using (StreamReader r = new StreamReader($@"{fullPath}\GraphicObjects.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<GraphicKey>>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            }
        }

        private static string GetPathToJsonFile()
        {
            var path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase))));
            return new Uri($@"{path}\Data\JsonObjects").LocalPath;
        }
    }
}