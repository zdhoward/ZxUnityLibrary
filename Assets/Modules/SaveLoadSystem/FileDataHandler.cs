using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataSavesFolder = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataSavesFolder, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataSavesFolder = dataSavesFolder;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string dataFileName)
    {
        string fullPath = Path.Combine(dataDirPath, dataSavesFolder, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to Load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(string dataFileName, GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataSavesFolder, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
            };
            string dataToStore = JsonConvert.SerializeObject(data, settings);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
