Shader "GOL/LifeShader"
{
    Properties
    {
        _AliveColor ("Alive Color", Color) = (1, 1, 1, 1)
        _DeadColor ("Dead Color", Color) = (0, 0, 0, 1)
        _Thickness ("Grid Thickness", Range(0, 10)) = 4.0
        _GridColor ("Grid Color", Color) = (0.5, 0.5, 0.5, 0.5)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            StructuredBuffer<int> _CellStateBuffer;
            float2 _Res;

            float4 _AliveColor;
            fixed4 _DeadColor;

            float4 _GridColor;
            float _Thickness;
            float _ShowGrid;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                int2 pos = i.uv * _Res;
                int state = _CellStateBuffer[pos.y * _Res.x + pos.x];

                fixed4 col = state ? _AliveColor : _DeadColor;

                if (_ShowGrid > 0.5)
                {
                    float2 gridUV = i.uv * _Res;

                    float2 distToEdge = abs(frac(gridUV - 0.5) - 0.5);
                    float2 pixelSize = fwidth(gridUV);

                    float2 pixelDist = distToEdge / pixelSize;
                    float lineWeight = min(pixelDist.x, pixelDist.y);

                    float halfWidth = _Thickness * 0.5;
                    float isLine = 1.0 - smoothstep(halfWidth - 0.5, halfWidth + 0.5, lineWeight);

                    float2 cellPixels = 1.0 / pixelSize;
                    float minCellPixels = min(cellPixels.x, cellPixels.y);
                    float gridVisibility = smoothstep(6.0, 8.0, minCellPixels);

                    float finalLine = isLine * gridVisibility;
                    col.rgb = lerp(col.rgb, _GridColor.rgb, finalLine);
                }

                return col;
            }
            ENDCG
        }
    }
}
