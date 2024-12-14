﻿Shader "Custom/Portal"
{
    Properties
    {
        _InactiveColour ("Inactive Colour", Color) = (1, 1, 1, 1)
        _DisplayMask ("Display Mask", Float) = 1
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _InactiveColour;
            float _DisplayMask;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;
                uv.y = 1.0 - uv.y; // Y eksenini ters çevir
                fixed4 portalCol = tex2D(_MainTex, uv);
                return portalCol * _DisplayMask + _InactiveColour * (1 - _DisplayMask);
            }
            ENDCG
        }
    }
    Fallback Off
}
