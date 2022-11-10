using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveDialogController : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] GameObject saveFileDialogItemPrefab;
    [SerializeField] TMP_InputField saveNameInputField;

    private void LoadSaveFileItems()
    {
        string path = Path.Combine(Application.persistentDataPath, "saves");
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles("*.json").OrderBy(f => f.LastWriteTime).Reverse().ToArray<FileInfo>();

        foreach (FileInfo file in files)
        {
            GameObject saveFileItemGameObject = Instantiate(saveFileDialogItemPrefab, content);
            SaveFileDialogItem saveFileItem = saveFileItemGameObject.GetComponent<SaveFileDialogItem>();
            saveFileItem.Setup(this, file);
        }
    }

    public void AutofillSaveName(string fileName)
    {
        saveNameInputField.text = fileName;
    }

    public void Save()
    {
        if (saveNameInputField.text != "")
        {
            SaveLoadManager.Instance.SaveGame(saveNameInputField.text);
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
