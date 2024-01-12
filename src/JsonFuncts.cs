namespace JsonFunctsNS;

using System.Text.Json;
using CustomExceptions;

class JsonFuncts {
    public static string readJson(string fileName){;
        string jsonString;
        try {
            jsonString = File.ReadAllText(fileName);
        } catch {
            throw new JsonReadException($"Could not read Json data from file '{fileName}'");
        }

        return jsonString;
    }

    public static bool tryDeserialize<T>(string inputString, ref T output){
        try {
            T? deserialized = JsonSerializer.Deserialize<T>(inputString);
            if (deserialized != null){
                output = deserialized;
                return true;
            }
        } catch {
            throw new DeserializationException($"Could not deserialize string into object of type '{typeof(T)}'");
        }

        return false;
    }

    public static bool writeJson<T>(T data, string outFileName){
        string json;
        try {
            json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        } catch {
            throw new SerializationException($"Could not serialize object of type '{typeof(T)}'");
        }

        if (writeJson(json, outFileName)){
            return true;
        } else {
            return false;
        }
    }

    public static bool writeJson(string jsonData, string outFileName){
        try {
            File.WriteAllText(outFileName, jsonData);
            return true;
        } catch {
            throw new JsonWriteException($"Could not write Json data to file '{outFileName}'");
        }
    }
}