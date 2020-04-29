Shader "Stencil/MaskShader"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry-1"}
        LOD 100

        Pass
        {
			Blend Zero One

			Stencil{
				Ref 2
				Comp Always		
				Pass Replace
			}

            
        }
    }
}
