Shader "Stencil/MaskShader"
{
    Properties
    {
		[IntRange] _StencilRef("Stencil Reference Value", Range(0,255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry"}
        LOD 100

        Pass
        {
			Blend Zero One
			ZWrite Off

			Stencil{
				Ref [_StencilRef]
				Comp Always		
				Pass Replace
			}

            
        }
    }
}
