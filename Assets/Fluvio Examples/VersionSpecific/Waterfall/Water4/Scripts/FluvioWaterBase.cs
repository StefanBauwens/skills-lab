using UnityEngine;

public enum FluvioWaterQuality {
		High = 2,
		Medium = 1,
		Low = 0,
}

[ExecuteInEditMode]
[AddComponentMenu("Fluvio Examples/Waterfall/Water4/Water Base")]
public class FluvioWaterBase : MonoBehaviour 
{
	public Material sharedMaterial;
	public FluvioWaterQuality waterQuality = FluvioWaterQuality.High;
	public bool edgeBlend = true;
	
	public void UpdateShader() 
	{		
		if(waterQuality > FluvioWaterQuality.Medium)
			sharedMaterial.shader.maximumLOD = 501;
		else if(waterQuality> FluvioWaterQuality.Low)
			sharedMaterial.shader.maximumLOD = 301;
		else 
			sharedMaterial.shader.maximumLOD = 201;	
		
		// If the system does not support depth textures (ie. NaCl), turn off edge bleeding, 
		// as the shader will render everything as transparent if the depth texture is not valid.
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
			edgeBlend = false;

		if(edgeBlend) 
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_ON");
			Shader.DisableKeyword("WATER_EDGEBLEND_OFF");		
			// just to make sure (some peeps might forget to add a water tile to the patches)
			if (Camera.main)
				Camera.main.depthTextureMode |= DepthTextureMode.Depth;		
		} 
		else 
		{
			Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
			Shader.DisableKeyword("WATER_EDGEBLEND_ON");	
		}
	}
	
	public void WaterTileBeingRendered (Transform tr, Camera currentCam) 
	{
		if (currentCam && edgeBlend)
			currentCam.depthTextureMode |= DepthTextureMode.Depth;	
	}
	
	public void Update () 
	{				
		if(sharedMaterial)		
			UpdateShader();
	}	
}