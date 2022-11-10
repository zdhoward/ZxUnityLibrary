using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadFileDialogItem : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI label;

    private LoadDialogController loadDialogController;

    private string fileName;

    private void Awake()
    {
        button = GetComponent<Button>();
        label = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(LoadDialogController loadDialogController, FileInfo file)
    {
        this.loadDialogController = loadDialogController;

        fileName = Path.GetFileNameWithoutExtension(file.Name);
        string lastModified = file.LastWriteTime.ToShortDateString();

        label.text = fileName + '\n' + "Last Modified: " + lastModified;

        button.onClick.AddListener(AutofillSaveName);
    }

    private void AutofillSaveName()
    {
        loadDialogController.AutofillSaveName(fileName);
    }
}
