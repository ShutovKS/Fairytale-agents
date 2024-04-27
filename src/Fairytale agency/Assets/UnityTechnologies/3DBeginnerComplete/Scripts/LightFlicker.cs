using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LightFlicker : MonoBehaviour
{
    public enum FlickerMode
    {
        Random,
        AnimationCurve
    }
    
    public Light flickeringLight;
    public Renderer flickeringRenderer;
    public FlickerMode flickerMode;
    public float lightIntensityMin = 1.25f;
    public float lightIntensityMax = 2.25f;
    public float flickerDuration = 0.075f;
    public AnimationCurve intensityCurve;

    private Material m_FlickeringMaterial;
    private Color m_EmissionColor;
    private float m_Timer;
    private float m_FlickerLightIntensity;

    private static readonly int k_EmissionColorID = Shader.PropertyToID (k_EmissiveColorName);

    private const string k_EmissiveColorName = "_EmissionColor";
    private const string k_EmissionName = "_Emission";
    private const float k_LightIntensityToEmission = 2f / 3f;

    private void Start()
    {
        m_FlickeringMaterial = flickeringRenderer.material;
        m_FlickeringMaterial.EnableKeyword(k_EmissionName);
        m_EmissionColor = m_FlickeringMaterial.GetColor(k_EmissionColorID);
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;

        if (flickerMode == FlickerMode.Random)
        {
            if (m_Timer >= flickerDuration)
            {
                ChangeRandomFlickerLightIntensity ();
            }
        }
        else if(flickerMode == FlickerMode.AnimationCurve)
        {
            ChangeAnimatedFlickerLightIntensity ();
        }
            
        flickeringLight.intensity = m_FlickerLightIntensity;
        m_FlickeringMaterial.SetColor (k_EmissionColorID, m_EmissionColor * m_FlickerLightIntensity * k_LightIntensityToEmission);
    }

    private void ChangeRandomFlickerLightIntensity ()
    {
        m_FlickerLightIntensity = Random.Range(lightIntensityMin, lightIntensityMax);

        m_Timer = 0f;
    }

    private void ChangeAnimatedFlickerLightIntensity ()
    {
        m_FlickerLightIntensity = intensityCurve.Evaluate (m_Timer);

        if (m_Timer >= intensityCurve[intensityCurve.length - 1].time)
            m_Timer = intensityCurve[0].time;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LightFlicker))]
public class LightFlickerEditor : Editor
{
    private SerializedProperty m_ScriptProp;
    private SerializedProperty m_FlickeringLightProp;
    private SerializedProperty m_FlickeringRendererProp;
    private SerializedProperty m_FlickerModeProp;
    private SerializedProperty m_LightIntensityMinProp;
    private SerializedProperty m_LightIntensityMaxProp;
    private SerializedProperty m_FlickerDurationProp;
    private SerializedProperty m_IntensityCurveProp;

    private void OnEnable ()
    {
        m_ScriptProp = serializedObject.FindProperty ("m_Script");
        m_FlickeringLightProp = serializedObject.FindProperty ("flickeringLight");
        m_FlickeringRendererProp = serializedObject.FindProperty ("flickeringRenderer");
        m_FlickerModeProp = serializedObject.FindProperty ("flickerMode");
        m_LightIntensityMinProp = serializedObject.FindProperty ("lightIntensityMin");
        m_LightIntensityMaxProp = serializedObject.FindProperty ("lightIntensityMax");
        m_FlickerDurationProp = serializedObject.FindProperty ("flickerDuration");
        m_IntensityCurveProp = serializedObject.FindProperty ("intensityCurve");
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();

        GUI.enabled = false;
        EditorGUILayout.PropertyField (m_ScriptProp);
        GUI.enabled = true;
        
        EditorGUILayout.PropertyField (m_FlickeringLightProp);
        EditorGUILayout.PropertyField (m_FlickeringRendererProp);
        EditorGUILayout.PropertyField (m_FlickerModeProp);

        if (m_FlickerModeProp.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField (m_LightIntensityMinProp);
            EditorGUILayout.PropertyField (m_LightIntensityMaxProp);
            EditorGUILayout.PropertyField (m_FlickerDurationProp);

        }
        else if (m_FlickerModeProp.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField (m_IntensityCurveProp);
        }

        serializedObject.ApplyModifiedProperties ();
    }

    /*public Light flickeringLight;
    public Renderer flickeringRenderer;
    public FlickerMode flickerMode;
    public float lightIntensityMin = 1.25f;
    public float lightIntensityMax = 2.25f;
    public float flickerDuration = 0.075f;
    public AnimationCurve intensityCurve;*/
}
#endif