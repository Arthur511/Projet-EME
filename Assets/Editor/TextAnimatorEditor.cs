
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TextAnimator))]
public class TextAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TextAnimator animator = (TextAnimator)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Text Animator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Dropdown de l'animation
        animator.CurrentTextAnimation = (TextAnimationType)EditorGUILayout.EnumPopup("Animation", animator.CurrentTextAnimation);

        EditorGUILayout.Space();

        // Affiche uniquement les paramètres utiles selon l'animation
        switch (animator.CurrentTextAnimation)
        {
            case TextAnimationType.Wave:
                EditorGUILayout.LabelField("Wave Settings", EditorStyles.boldLabel);
                animator._waveAmplitude = EditorGUILayout.Slider("Amplitude",  animator._waveAmplitude, 0f, 50f);
                animator._waveFrequency = EditorGUILayout.Slider("Frequency",  animator._waveFrequency, 0f, 10f);
                animator._waveSpeed     = EditorGUILayout.Slider("Speed",      animator._waveSpeed,     0f, 10f);
                break;

            case TextAnimationType.Shake:
                EditorGUILayout.LabelField("Shake Settings", EditorStyles.boldLabel);
                animator._shakeAmplitude = EditorGUILayout.Slider("Amplitude", animator._shakeAmplitude, 0f, 20f);
                break;

            /*case TextAnimationType.WaveShake:
                EditorGUILayout.LabelField("Wave Settings", EditorStyles.boldLabel);
                animator._waveAmplitude = EditorGUILayout.Slider("Amplitude",  animator._waveAmplitude, 0f, 50f);
                animator._waveFrequency = EditorGUILayout.Slider("Frequency",  animator._waveFrequency, 0f, 10f);
                animator._waveSpeed     = EditorGUILayout.Slider("Speed",      animator._waveSpeed,     0f, 10f);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Shake Settings", EditorStyles.boldLabel);
                animator._shakeAmplitude = EditorGUILayout.Slider("Amplitude", animator._shakeAmplitude, 0f, 20f);
                break;*/

            case TextAnimationType.Pulse:
                EditorGUILayout.LabelField("Pulse Settings", EditorStyles.boldLabel);
                animator._waveSpeed     = EditorGUILayout.Slider("Speed",     animator._waveSpeed,     0f, 10f);
                animator._waveFrequency = EditorGUILayout.Slider("Frequency", animator._waveFrequency, 0f, 10f);
                break;

            case TextAnimationType.Bounce:
                EditorGUILayout.LabelField("Bounce Settings", EditorStyles.boldLabel);
                animator._waveAmplitude = EditorGUILayout.Slider("Amplitude", animator._waveAmplitude, 0f, 50f);
                animator._waveFrequency = EditorGUILayout.Slider("Frequency", animator._waveFrequency, 0f, 10f);
                animator._waveSpeed     = EditorGUILayout.Slider("Speed",     animator._waveSpeed,     0f, 10f);
                break;

            /*case TextAnimationType.SharpRotate:
                EditorGUILayout.LabelField("Sharp Rotate Settings", EditorStyles.boldLabel);
                animator._sharpRotationTimer = EditorGUILayout.Slider("Interval (s)", animator._sharpRotationTimer, 0.1f, 5f);
                break;*/

            case TextAnimationType.Rainbow:
                EditorGUILayout.LabelField("Rainbow Settings", EditorStyles.boldLabel);
                animator._waveSpeed     = EditorGUILayout.Slider("Speed",     animator._waveSpeed,     0f, 5f);
                animator._waveFrequency = EditorGUILayout.Slider("Spread",    animator._waveFrequency, 0f, 1f);
                break;

            case TextAnimationType.None:
                EditorGUILayout.HelpBox("Aucune animation active.", MessageType.Info);
                break;
        }

        EditorGUILayout.Space();

        // Bouton de preview en éditeur
        if (GUILayout.Button("Reset paramètres"))
        {
            animator._waveAmplitude  = 5f;
            animator._waveFrequency  = 1f;
            animator._waveSpeed      = 2f;
            animator._shakeAmplitude = 2f;
        }

        // Marque l'objet comme modifié pour la sauvegarde
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}