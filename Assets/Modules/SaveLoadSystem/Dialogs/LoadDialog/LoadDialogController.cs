using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using TMPro;

public class LoadDialogController : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject loadFileDialogItemPrefab;
    [SerializeField] TMP_InputField saveNameInputField;

    private void LoadSaveFileItems()
    {
        string path = Path.Combine(Application.persistentDataPath, "saves");
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles("*.json").OrderBy(f => f.LastWriteTime).Reverse().ToArray<FileInfo>();

        foreach (FileInfo file in files)
        {
            GameObject saveFileItemGameObject = Instantiate(loadFileDialogItemPrefab, content);
            LoadFileDialogItem saveFileItem = saveFileItemGameObject.GetComponent<LoadFileDialogItem>();
            saveFileItem.Setup(this, file);
        }
    }

    public void AutofillSaveName(string fileName)
    {
        saveNameInputField.text = fileName;
    }

    public void Load()
    {
        if (saveNameInputField.text != "")
        {
            SaveLoadManager.Instance.LoadGame(saveNameInputField.text);
            HideWindow();
        }
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
        LoadSaveFileItems();
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);

        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
