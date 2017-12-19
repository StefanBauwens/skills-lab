using UnityEngine;

[RequireComponent(typeof(FluvioWaterBase))]
[ExecuteInEditMode]
[AddComponentMenu("Fluvio Examples/Waterfall/Water4/Specular Lighting")]
public class FluvioSpecularLighting : MonoBehaviour 
{		
	public Transform specularLight;
	private FluvioWaterBase waterBase = null;
	
	public void Start () 
	{
		waterBase = (FluvioWaterBase)gameObject.GetComponent(typeof(FluvioWaterBase));		
	}	

	public void Update () 
	{
		if (!waterBase)
			waterBase = (FluvioWaterBase)gameObject.GetComponent(typeof(FluvioWaterBase));				
		
		if (specularLight && waterBase.sharedMaterial)
			waterBase.sharedMaterial.SetVector("_WorldLightDir", specularLight.transform.forward);
	}

}
