Shader "Unlit/Player 1"
{
	Properties {
		_Color ("Main Color", Color) = (0,0,0,0.5)
		_SpecColor ("Spec Color", Color) = (0,0,0,1)
		_Emission ("Emissive Color", Color) = (0,0,0,0)
		_Shininess ("Shininess", Range (0.1,1)) = 0.7
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}

	SubShader {
		Pass {
			Material {
				Diffuse [_Color]
				Ambient [_Color]
				Shininess [_Shininess]
				Specular [_SpecColor]
				Emission [_Emission]
			}
			Lighting On
			SeparateSpecular On
			SetTexture [_MainTex] {
				constantColor [_Color]
				Combine texture * primary DOUBLE, texture * constant
			}
		}
	}
}