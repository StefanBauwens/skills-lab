using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Fluvio Examples/Waterfall/Water4/Water Tile")]
public class FluvioWaterTile : MonoBehaviour 
{
	public FluvioPlanarReflection reflection;
	public FluvioWaterBase waterBase;
	
	public void Start () 
	{
		AcquireComponents();
	}
	
	private void AcquireComponents() 
	{
		if (!reflection) {
			if (transform.parent)
				reflection = (FluvioPlanarReflection)transform.parent.GetComponent<FluvioPlanarReflection>();
			else
				reflection = (FluvioPlanarReflection)transform.GetComponent<FluvioPlanarReflection>();	
		}
		
		if (!waterBase) {
			if (transform.parent)
				waterBase = (FluvioWaterBase)transform.parent.GetComponent<FluvioWaterBase>();
			else
				waterBase = (FluvioWaterBase)transform.GetComponent<FluvioWaterBase>();	
		}
	}
	
#if UNITY_EDITOR
	public void Update () 
	{
		AcquireComponents();
	}
#endif
	
	public void OnWillRenderObject() 
	{
		if (reflection)
			reflection.WaterTileBeingRendered(transform, Camera.current);
		if (waterBase)
			waterBase.WaterTileBeingRendered(transform, Camera.current);		
	}
}
