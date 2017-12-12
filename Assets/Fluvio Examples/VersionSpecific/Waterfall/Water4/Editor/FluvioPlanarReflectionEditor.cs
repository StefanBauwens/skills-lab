using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FluvioPlanarReflection))]
public class FluvioPlanarReflectionEditor : Editor
{
    private SerializedObject serObj;

    //private SerializedProperty wavesFrequency;

	// reflection
	private SerializedProperty reflectionMask;
	private SerializedProperty reflectSkybox;
	private SerializedProperty clearColor;

	bool showKidsWithReflectionHint = false;

	public void OnEnable () {
		serObj = new SerializedObject (target);

		reflectionMask = serObj.FindProperty("reflectionMask");
		reflectSkybox = serObj.FindProperty("reflectSkybox");
		clearColor = serObj.FindProperty("clearColor");
	}

    public override void OnInspectorGUI ()
    {
        GUILayout.Label ("Render planar reflections and use GrabPass for refractions", EditorStyles.miniBoldLabel);

#if !UNITY_5_5_OR_NEWER
		if(!SystemInfo.supportsRenderTextures)
			EditorGUILayout.HelpBox("Realtime reflections not supported", MessageType.Warning);
#endif

    	serObj.Update();

    	EditorGUILayout.PropertyField(reflectionMask, new GUIContent("Reflection layers"));
    	EditorGUILayout.PropertyField(reflectSkybox, new GUIContent("Use skybox"));
		EditorGUILayout.PropertyField(clearColor, new GUIContent("Clear color"));

        showKidsWithReflectionHint = EditorGUILayout.BeginToggleGroup("Show all tiles", showKidsWithReflectionHint);
        if (showKidsWithReflectionHint) {
        	int i = 0;
        	foreach(Transform t in ((FluvioPlanarReflection)target).transform) {
        		if (t.GetComponent<FluvioWaterTile>())	{
        			if(i%2==0)
        				EditorGUILayout.BeginHorizontal();
        			EditorGUILayout.ObjectField(t, typeof(Transform), true);
        			if(i%2==1)
        				EditorGUILayout.EndHorizontal();
        			i = (i+1)%2;
        		}
        	}
        	if(i>0)
				EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndToggleGroup();

    	serObj.ApplyModifiedProperties();
    }

}