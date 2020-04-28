Shader "Unlit/NewUnlitShader"
{
    Properties
    {
		_Color("Tint", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}

		[IntRange] _StencilRef("Stencil Reference Value", Range(0,255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry" }
        LOD 100

		Stencil{
			Ref[_StencilRef]
			Comp Equal
		}

        Pass
		{
            
        }
    }
}
