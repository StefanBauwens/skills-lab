using UnityEngine;
using System.Collections;

namespace Thinksquirrel.FluvioExamples
{
	[RequireComponent(typeof(Projector))]
	[ExecuteInEditMode]
	[AddComponentMenu("Fluvio Examples/Waterfall/Animated Projector")]	
	public class AnimatedProjector : MonoBehaviour
	{
		public float fps = 30.0f;
		public Material material;
		public Texture2D[] frames;
		
		float m_LastTime;
		float m_Accumulator;
		int m_FrameIndex;
		Material m_Material;
		Projector m_Projector;
		
		void OnEnable()
		{
			m_Projector = GetComponent<Projector>();
			if (material)
			{
				m_Material = new Material(material);
				m_Material.hideFlags = HideFlags.HideAndDontSave;
				m_Projector.material = m_Material;
			}
		}
		
		void Update()
		{
			if (fps <= 0) return;
			
			var dt = Time.realtimeSinceStartup - m_LastTime;
			dt *= Time.timeScale;
			
			var t =  1.0f / fps;
			
			m_LastTime = Time.realtimeSinceStartup;
			
			m_Accumulator += dt;
			
			while(m_Accumulator >= t)
			{
				if (m_Material && m_FrameIndex < frames.Length) m_Material.SetTexture("_ShadowTex", frames[m_FrameIndex]);
				m_FrameIndex = (m_FrameIndex + 1) % frames.Length;
			    m_Accumulator -= t;
			}
		}
		
		void OnDisable()
		{
			if (m_Material != null)
			{
				m_Projector.material = null;
				DestroyImmediate(m_Material);
				m_Material = null;
			}
		}
	}
}