Shader "Custom/HeightmapOverlay"
{
    Properties
    {
        _MinHeight("Min Height", Float) = 0
        _MaxHeight("Max Height", Float) = 500
        _LineSpacing("Line Spacing", Float) = 5
        _Color1("Low Elevation Color", Color) = (0, 0, 1, 1)
        _Color2("High Elevation Color", Color) = (1, 0, 0, 1)
        _LineColor("Contour Line Color", Color) = (0,0,0,1)
    }
        SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float height : TEXCOORD0;
            };

            float _MinHeight;
            float _MaxHeight;
            float _LineSpacing;
            float4 _Color1;
            float4 _Color2;
            float4 _LineColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.height = (v.vertex.y - _MinHeight) / (_MaxHeight - _MinHeight);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float contour = abs(fmod(i.height * (_MaxHeight - _MinHeight), _LineSpacing) - (_LineSpacing / 2)) < 0.5;
                fixed4 color = lerp(_Color1, _Color2, i.height);
                return contour ? _LineColor : color;
            }
            ENDCG
        }
    }
}
