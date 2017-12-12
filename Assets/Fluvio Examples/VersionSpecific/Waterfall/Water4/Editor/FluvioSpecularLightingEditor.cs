using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FluvioSpecularLighting))]
public class FluvioSpecularLightingEditor : Editor 
{    
    private SerializedObject serObj;
    private SerializedProperty specularLight;
    
	public void OnEnable () {
		serObj = new SerializedObject (target); 
		specularLight = serObj.FindProperty("specularLight");   		
	}
	
    public override void OnInspectorGUI () 
    {
    	serObj.Update();
    	
    	GameObject go = ((FluvioSpecularLighting)serObj.targetObject).gameObject;
    	FluvioWaterBase wb = (FluvioWaterBase)go.GetComponent(typeof(FluvioWaterBase));
    	
    	if(!wb.sharedMaterial)
    		return;
    	
    	if(wb.sharedMaterial.HasProperty("_WorldLightDir")) {
    		GUILayout.Label ("Transform casting specular highlights", EditorStyles.miniBoldLabel);    		
    		EditorGUILayout.PropertyField(specularLight, new GUIContent("Specular light"));
    		
  			if(wb.sharedMaterial.HasProperty("_SpecularColor"))
				FluvioWaterEditorUtility.SetMaterialColor(
					"_SpecularColor", 
					EditorGUILayout.ColorField("Specular", 
					FluvioWaterEditorUtility.GetMaterialColor("_SpecularColor", wb.sharedMaterial)), 
					wb.sharedMaterial);
			if(wb.sharedMaterial.HasProperty("_Shininess"))
				FluvioWaterEditorUtility.SetMaterialFloat("_Shininess", EditorGUILayout.Slider(
					"Specular power", 
					FluvioWaterEditorUtility.GetMaterialFloat("_Shininess", wb.sharedMaterial), 
					0.0F, 500.0F), wb.sharedMaterial);		  		
    	}
    	else
    		GUILayout.Label ("The shader doesn't have the needed _WorldLightDir property.", EditorStyles.miniBoldLabel);
    	
    	serObj.ApplyModifiedProperties();
    }
    
}