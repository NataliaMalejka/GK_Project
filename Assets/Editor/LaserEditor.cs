using UnityEditor;
using UnityEngine;

/**
 * Custom editor for the Laser component.
 * Groups and manages fields for better inspector UI.
 * Disables the interval field if constant laser mode is enabled.
 *
 * @author Krzysztof Gach
 * @version 1.3
 */
[CustomEditor(typeof(Laser))]
public class LaserEditor : Editor
{
    private bool showLaserSettings = true;
    private bool showLaserMode = true;
    private bool showReferences = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Laser Settings Foldout
        showLaserSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showLaserSettings, "Laser Settings");
        if (showLaserSettings)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("defDistanceRay"), new GUIContent("Default Distance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ignoredLayers"), new GUIContent("Ignored Layers"));
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

            EditorGUILayout.PropertyField(serializedObject.FindProperty("isActive"), new GUIContent("Active"));
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("explosionController"), new GUIContent("Explosion Tilemap Controller"));
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
