Shader "Fluvio Examples/Projector/Light" {
  Properties {
  	  _Color ("Main Color", Color) = (1,1,1,1)  	
     _ShadowTex ("Cookie", 2D) = "" {}
     _FalloffTex ("FallOff", 2D) = "" {}
  }
  Subshader {
     Pass {
        ZWrite off
        Fog { Color (0, 0, 0) }
        Color [_Color]
        ColorMask RGB
        Blend One One
		Offset -1, -1
        SetTexture [_ShadowTex] {
		   combine texture * primary, ONE - texture
        }
        SetTexture [_FalloffTex] {
           constantColor (0,0,0,0)
           combine previous lerp (texture) constant
        }
     }
  }
}