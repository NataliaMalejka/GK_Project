using UnityEditor;
using UnityEngine;

/**
 * Custom editor for the Laser component.
 * Groups and manages fields for better inspector UI.
 * Disables the interval field if constant laser mode is enabled.
 *
 * @author Krzysztof Gach
 * @version 1.0
 */
[CustomEditor(typeof(Laser))]
public class LaserEditor : Editor
{
    // Foldout states (persist per editor instance)
    private bool showLaserSettings = true;
    private bool showLaserMode = true;
    private bool showReferences = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Using the built-in EditorGUILayout.Foldout with EditorStyles.foldoutHeader 
        // which includes the arrow and bold text by default in Unity 2019+
        
        // Laser Settings Foldout
        showLaserSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showLaserSettings, "Laser Settings");
        if (showLaserSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defDistanceRay"), new GUIContent("Default Distance"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // Laser Mode Foldout
        showLaserMode = EditorGUILayout.BeginFoldoutHeaderGroup(showLaserMode, "Laser Mode");
        if (showLaserMode)
        {
            EditorGUI.indentLevel++;
            SerializedProperty isConstantProp = serializedObject.FindProperty("isConstantLaser");
            EditorGUILayout.PropertyField(isConstantProp, new GUIContent("Is Constant Laser"));

            using (new EditorGUI.DisabledScope(isConstantProp.boolValue))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("intervalMs"), new GUIContent("Interval (ms)"));
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        // References Foldout
        showReferences = EditorGUILayout.BeginFoldoutHeaderGroup(showReferences, "References");
        if (showReferences)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("firePoint"), new GUIContent("Fire Point"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lineRenderer"), new GUIContent("Line Renderer"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}