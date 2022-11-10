#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using TMPro;

[CustomEditor(typeof(LoadingManager))]
public class LoadingManagerEditor : Editor
{
    private AnimBool enableTextBool;
    private SerializedProperty enableTextProperty;
    private SerializedProperty textProperty;
    private SerializedProperty textSizeProperty;
    private SerializedProperty textColorProperty;
    private SerializedProperty textFontProperty;

    private AnimBool enableProgressBarBool;
    private SerializedProperty enableProgressBarProperty;
    private SerializedProperty progressBarBackgroundProperty;
    private SerializedProperty progressBarBackgroundColorProperty;
    private SerializedProperty progressBarFilledProperty;
    private SerializedProperty progressBarFilledColorProperty;

    private AnimBool enableFadeInOutBool;
    private AnimBool enableFadeInBool;
    private AnimBool enableFadeOutBool;
    private SerializedProperty enableFadeInOutProperty;
    private SerializedProperty fadesInProperty;
    private SerializedProperty fadeInTimeProperty;
    private SerializedProperty fadesOutProperty;
    private SerializedProperty fadeOutTimeProperty;

    private AnimBool enableBackgroundImageBool;
    private SerializedProperty enableBackgroundImageProperty;
    private SerializedProperty backgroundImageProperty;
    private SerializedProperty backgroundColorProperty;

    private bool isInitialized = false;

    private void IntitializeIfNeeded(bool force = false)
    {
        if (isInitialized || force)
        {
            CheckForTextMeshPro();
            CheckForLeanTween();
        }
    }

    public void OnEnable()
    {
        IntitializeIfNeeded(true);

        // Properties Setup
        enableTextProperty = serializedObject.FindProperty("enableText");
        enableTextBool = new AnimBool(enableTextProperty.boolValue);
        enableTextBool.valueChanged.AddListener(Repaint);
        textProperty = serializedObject.FindProperty("text");
        textSizeProperty = serializedObject.FindProperty("textSize");
        textColorProperty = serializedObject.FindProperty("textColor");
        textFontProperty = serializedObject.FindProperty("textFont");

        enableProgressBarProperty = serializedObject.FindProperty("enableProgressBar");
        enableProgressBarBool = new AnimBool(enableProgressBarProperty.boolValue);
        enableProgressBarBool.valueChanged.AddListener(Repaint);
        progressBarBackgroundProperty = serializedObject.FindProperty("progressBarBackground");
        progressBarBackgroundColorProperty = serializedObject.FindProperty("progressBarBackgroundColor");
        progressBarFilledProperty = serializedObject.FindProperty("progressBarFilled");
        progressBarFilledColorProperty = serializedObject.FindProperty("progressBarFilledColor");

        enableFadeInOutProperty = serializedObject.FindProperty("enableFadeInOut");
        fadesInProperty = serializedObject.FindProperty("fadesIn");
        fadesOutProperty = serializedObject.FindProperty("fadesOut");
        enableFadeInOutBool = new AnimBool(enableFadeInOutProperty.boolValue);
        enableFadeInBool = new AnimBool(fadesInProperty.boolValue);
        enableFadeOutBool = new AnimBool(fadesOutProperty.boolValue);
        enableFadeInOutBool.valueChanged.AddListener(Repaint);
        enableFadeInBool.valueChanged.AddListener(Repaint);
        enableFadeOutBool.valueChanged.AddListener(Repaint);
        fadeInTimeProperty = serializedObject.FindProperty("fadeInTime");
        fadeOutTimeProperty = serializedObject.FindProperty("fadeOutTime");

        enableBackgroundImageProperty = serializedObject.FindProperty("enableBackgroundImage");
        enableBackgroundImageBool = new AnimBool(enableBackgroundImageProperty.boolValue);
        enableBackgroundImageBool.valueChanged.AddListener(Repaint);
        backgroundImageProperty = serializedObject.FindProperty("backgroundImage");
        backgroundColorProperty = serializedObject.FindProperty("backgroundColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Loading Manager Config", GetTitleLabelStyle());

        GUIHorizontalLine();

        // Check For Packages
        if (!HasAllRequiredPackages())
        {
            string helpMessage = "";
            helpMessage += "You do not have all the required packages: \n";
            if (!hasTextMeshPro)
                helpMessage += "TextMeshPro is missing: com.unity.textmeshpro \n";
            if (!hasLeanTween)
                helpMessage += "LeanTween is missing";

            EditorGUILayout.HelpBox(helpMessage, MessageType.Error);
            if (GUILayout.Button("Re-Initialize Loading Manager"))
            {
                IntitializeIfNeeded(true);
            }
            return;
        }

        // TEXT
        CreateAnimatedBoolField(enableTextProperty, enableTextBool);
        if (EditorGUILayout.BeginFadeGroup(enableTextBool.faded))
        {
            EditorGUI.indentLevel++;
            CreateStringField(textProperty);
            CreateFloatField(textSizeProperty);
            CreateColorField(textColorProperty);
            CreateObjectField(textFontProperty, typeof(TMP_FontAsset));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        GUIHorizontalLine();

        // PROGRESS BAR
        CreateAnimatedBoolField(enableProgressBarProperty, enableProgressBarBool);
        if (EditorGUILayout.BeginFadeGroup(enableProgressBarBool.faded))
        {
            EditorGUI.indentLevel++;
            CreateObjectField(progressBarBackgroundProperty, typeof(Sprite));
            CreateColorField(progressBarBackgroundColorProperty);
            CreateObjectField(progressBarFilledProperty, typeof(Sprite));
            CreateColorField(progressBarFilledColorProperty);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        GUIHorizontalLine();

        // FADE IN/OUT
        CreateAnimatedBoolField(enableFadeInOutProperty, enableFadeInOutBool);
        if (EditorGUILayout.BeginFadeGroup(enableFadeInOutBool.faded))
        {
            EditorGUI.indentLevel++;
            CreateAnimatedBoolField(fadesInProperty, enableFadeInBool);
            if (EditorGUILayout.BeginFadeGroup(enableFadeInBool.faded))
            {
                EditorGUI.indentLevel++;
                CreateFloatField(fadeInTimeProperty);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            CreateAnimatedBoolField(fadesOutProperty, enableFadeOutBool);
            if (EditorGUILayout.BeginFadeGroup(enableFadeOutBool.faded))
            {
                EditorGUI.indentLevel++;
                CreateFloatField(fadeOutTimeProperty);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        GUIHorizontalLine();

        // BACKGROUND
        CreateAnimatedBoolField(enableBackgroundImageProperty, enableBackgroundImageBool);
        EditorGUI.indentLevel++;
        if (EditorGUILayout.BeginFadeGroup(enableBackgroundImageBool.faded))
        {
            CreateObjectField(backgroundImageProperty, typeof(Sprite));
        }
        EditorGUILayout.EndFadeGroup();
        CreateColorField(backgroundColorProperty);
        EditorGUI.indentLevel--;

        // APPLY CHANGES
        serializedObject.ApplyModifiedProperties();
    }

    void GUIHorizontalLine(int i_height = 1)
    {
        EditorGUILayout.Separator();
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Separator();
    }

    GUIStyle GetTitleLabelStyle()
    {
        GUIStyle labelStyle = new GUIStyle(EditorStyles.inspectorDefaultMargins);
        labelStyle.normal.textColor = Color.white;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 20;

        return labelStyle;
    }

    private Color CreateColorField(SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(property.displayName);
        property.colorValue = EditorGUILayout.ColorField(property.colorValue);
        EditorGUILayout.EndHorizontal();

        return property.colorValue;
    }

    private bool CreateBoolField(SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(property.displayName);
        property.boolValue = EditorGUILayout.Toggle(property.boolValue);
        EditorGUILayout.EndHorizontal();

        return property.boolValue;
    }

    private bool CreateAnimatedBoolField(SerializedProperty property, AnimBool animBool)
    {
        property.boolValue = EditorGUILayout.ToggleLeft(property.displayName, animBool.target);
        animBool.target = property.boolValue;

        return property.boolValue;
    }

    private float CreateFloatField(SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(property.displayName);
        property.floatValue = EditorGUILayout.FloatField(property.floatValue);
        EditorGUILayout.EndHorizontal();

        return property.floatValue;
    }

    private string CreateStringField(SerializedProperty property)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(property.displayName);
        property.stringValue = EditorGUILayout.TextField(property.stringValue);
        EditorGUILayout.EndHorizontal();

        return property.stringValue;
    }

    private object CreateObjectField(SerializedProperty property, Type typeOfObject)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(property.displayName);
        property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeOfObject, false);
        EditorGUILayout.EndHorizontal();

        return property.objectReferenceValue;
    }

    ////////// PACKAGE CHECK //////////
    ListRequest Request;
    bool hasTextMeshPro = false;
    bool hasLeanTween = false;

    bool HasAllRequiredPackages()
    {
        if (!hasTextMeshPro)
            return false;

        if (!hasLeanTween)
            return false;

        return true;
    }

    void CheckForLeanTween()
    {
        var leanTween = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         where type.Name == "LeanTween"
                         select type).FirstOrDefault();

        if (leanTween == null)
            hasLeanTween = false;
        else
            hasLeanTween = true;
    }

    void CheckForTextMeshPro()
    {
        Request = Client.List(true, false);    // List packages installed for the project
        EditorApplication.update += Progress;
    }

    void Progress()
    {
        if (Request.IsCompleted)
        {
            if (Request.Status == StatusCode.Success)
            {
                foreach (var package in Request.Result)
                {
                    if (package.name == "com.unity.textmeshpro")
                    {
                        hasTextMeshPro = true;
                        break;
                    }
                }
            }
            else if (Request.Status >= StatusCode.Failure)
            {
                hasTextMeshPro = false;
                Debug.Log(Request.Error.message);
            }
            EditorApplication.update -= Progress;
        }
    }
}
#endif
