Shader "Stencil/StencilShaderInvert"
{
    Properties
    {
		_Color("Tint", Color) = (0, 0, 0, 1)

        _MainTex ("Texture", 2D) = "white" {}
		_CutOff("Alpha cutoff", Range(0,1)) = 0.5

		[IntRange] _StencilRef("Stencil Reference Value", Range(0,255)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+1"}
        LOD 100

        Pass
        {
			Stencil{
				Ref [_StencilRef]
				Comp NotEqual
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;

			uniform float _CutOff;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
				float4 color = tex2D(_MainTex, float2(i.uv.xy));
				if (color.a < _CutOff) discard;
				color *= _Color;
				return color;
            }
            ENDCG
        }
    }
}
