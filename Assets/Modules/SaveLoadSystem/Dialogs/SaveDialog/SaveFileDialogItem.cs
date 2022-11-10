using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveFileDialogItem : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI label;

    private SaveDialogController saveDialogController;

    private string fileName;

    private void Awake()
    {
        button = GetComponent<Button>();
        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(SaveDialogController saveDialogController, FileInfo file)
    {
        this.saveDialogController = saveDialogController;

        fileName = Path.GetFileNameWithoutExtension(file.Name);
        string lastModified = file.LastWriteTime.ToShortDateString();

        label.text = fileName + '\n' + "Last Modified: " + lastModified;

        button.onClick.AddListener(AutofillSaveName);
    }

    private void AutofillSaveName()
    {
        saveDialogController.AutofillSaveName(fileName);
    }
}
