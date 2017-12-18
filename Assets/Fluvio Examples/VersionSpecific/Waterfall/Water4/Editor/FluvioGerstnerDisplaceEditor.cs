using UnityEngine;
using System;
using UnityEditor;

[CustomEditor(typeof(FluvioGerstnerDisplace))]
public class FluvioGerstnerDisplaceEditor : Editor 
{    
    private SerializedObject serObj;

	public void OnEnable () 
	{
		serObj = new SerializedObject (target);
	}
	
    public override void OnInspectorGUI () 
    {
    	serObj.Update(); 
    	
    	GameObject go = ((FluvioGerstnerDisplace)serObj.targetObject).gameObject;
    	FluvioWaterBase wb = (FluvioWaterBase)go.GetComponent(typeof(FluvioWaterBase));    	
    	Material sharedWaterMaterial = wb.sharedMaterial;
    	
        GUILayout.Label ("Animates vertices using up 4 generated waves", EditorStyles.miniBoldLabel);    
        
		if(sharedWaterMaterial) 
		{			
			Vector4 amplitude = FluvioWaterEditorUtility.GetMaterialVector("_GAmplitude", sharedWaterMaterial);
			Vector4 frequency = FluvioWaterEditorUtility.GetMaterialVector("_GFrequency", sharedWaterMaterial);
			Vector4 steepness = FluvioWaterEditorUtility.GetMaterialVector("_GSteepness", sharedWaterMaterial);
			Vector4 speed = FluvioWaterEditorUtility.GetMaterialVector("_GSpeed", sharedWaterMaterial);
			Vector4 directionAB = FluvioWaterEditorUtility.GetMaterialVector("_GDirectionAB", sharedWaterMaterial);
			Vector4 directionCD = FluvioWaterEditorUtility.GetMaterialVector("_GDirectionCD", sharedWaterMaterial);
			
			amplitude = EditorGUILayout.Vector4Field("Amplitude (Height offset)", amplitude);
			frequency = EditorGUILayout.Vector4Field("Frequency (Tiling)", frequency);
			steepness = EditorGUILayout.Vector4Field("Steepness", steepness);
			speed = EditorGUILayout.Vector4Field("Speed", speed);
			directionAB = EditorGUILayout.Vector4Field("Direction scale (Wave 1 (X,Y) and 2 (Z,W))", directionAB);
			directionCD = EditorGUILayout.Vector4Field("Direction scale (Wave 3 (X,Y) and 4 (Z,W))", directionCD);
			
			if (GUI.changed) {
				FluvioWaterEditorUtility.SetMaterialVector("_GAmplitude", amplitude, sharedWaterMaterial);
				FluvioWaterEditorUtility.SetMaterialVector("_GFrequency", frequency, sharedWaterMaterial);
				FluvioWaterEditorUtility.SetMaterialVector("_GSteepness", steepness, sharedWaterMaterial);
				FluvioWaterEditorUtility.SetMaterialVector("_GSpeed", speed, sharedWaterMaterial);
				FluvioWaterEditorUtility.SetMaterialVector("_GDirectionAB", directionAB, sharedWaterMaterial);
				FluvioWaterEditorUtility.SetMaterialVector("_GDirectionCD", directionCD, sharedWaterMaterial);
			}
			
			/*
			 
			Vector4 animationTiling = WaterEditorUtility.GetMaterialVector("_AnimationTiling", sharedWaterMaterial);
			Vector4 animationDirection = WaterEditorUtility.GetMaterialVector("_AnimationDirection", sharedWaterMaterial);
			
			float firstTilingU = animationTiling.x*100.0F;
			float firstTilingV = animationTiling.y*100.0F;
			float firstDirectionU = animationDirection.x;
			float firstDirectionV = animationDirection.y;

			float secondTilingU = animationTiling.z*100.0F;
			float secondTilingV = animationTiling.w*100.0F;
			float secondDirectionU = animationDirection.z;
			float secondDirectionV = animationDirection.w;
						
			
			EditorGUILayout.BeginHorizontal ();
			firstTilingU = EditorGUILayout.FloatField("First Tiling U", firstTilingU);
			firstTilingV = EditorGUILayout.FloatField("First Tiling V", firstTilingV);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			secondTilingU = EditorGUILayout.FloatField("Second Tiling U", secondTilingU);
			secondTilingV = EditorGUILayout.FloatField("Second Tiling V", secondTilingV);
			EditorGUILayout.EndHorizontal ();			
			
			EditorGUILayout.BeginHorizontal ();
			firstDirectionU = EditorGUILayout.FloatField("1st Animation U", firstDirectionU);
			firstDirectionV = EditorGUILayout.FloatField("1st Animation V", firstDirectionV);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			secondDirectionU = EditorGUILayout.FloatField("2nd Animation U", secondDirectionU);
			secondDirectionV = EditorGUILayout.FloatField("2nd Animation V", secondDirectionV);
			EditorGUILayout.EndHorizontal ();
		
			animationDirection = new Vector4(firstDirectionU,firstDirectionV, secondDirectionU,secondDirectionV);
			animationTiling = new Vector4(firstTilingU/100.0F,firstTilingV/100.0F, secondTilingU/100.0F,secondTilingV/100.0F);				
			
			WaterEditorUtility.SetMaterialVector("_AnimationTiling", animationTiling, sharedWaterMaterial);
			WaterEditorUtility.SetMaterialVector("_AnimationDirection", animationDirection, sharedWaterMaterial);
			
			EditorGUILayout.Separator ();		
			
	    	GUILayout.Label ("Displacement Strength", EditorStyles.boldLabel);				
			
			float heightDisplacement = WaterEditorUtility.GetMaterialFloat("_HeightDisplacement", sharedWaterMaterial);
			
			heightDisplacement = EditorGUILayout.Slider("Height", heightDisplacement, 0.0F, 5.0F);
			WaterEditorUtility.SetMaterialFloat("_HeightDisplacement", heightDisplacement, sharedWaterMaterial);	
			*/	
		}
		
    	serObj.ApplyModifiedProperties();
    }
}