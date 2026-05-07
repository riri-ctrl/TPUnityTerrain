Shader "Custom/Terrain" {
    Properties {
        _RockColour ("Rock Colour", Color) = (.26,.23,.17,1)
 
        [Header(Grass)]
        _GrassColour ("Grass Colour", Color) = (0.28,.6,.07,1)
        _GrassSlopeThreshold ("Grass Slope Threshold", Range(0,1)) = .5
        _GrassBlendAmount ("Grass Blend Amount", Range(0,1)) = .5

        [Header(Forest)]
        _ForestColour ("Forest Colour", Color) = (0.09,0.179,0.019,1)
        _ForestHeight("Forest Height", Float) = 1.3
        _ForestBlend("Forest Blend", Float) = 0.1

        [Header(Snow)]
        _SnowColour ("Snow Colour", Color) = (1,1,1,1)
        _SnowHeight("Snow Height", Float) = 2
        _SnowBlend("Snow Blend", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input {
            float3 worldPos;
            float3 worldNormal;
        };

        fixed4 _GrassColour;
        half _GrassSlopeThreshold;
        half _GrassBlendAmount;

        fixed4 _ForestColour;
        half _ForestHeight;
        half _ForestBlend;

        fixed4 _SnowColour;
        half _SnowHeight;
        half _SnowBlend;

        fixed4 _RockColour;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            float slope = 1-IN.worldNormal.y; // slope = 0 when terrain is completely flat

            //Grass
            float grassBlendHeight = _GrassSlopeThreshold * (1-_GrassBlendAmount);
            float grassWeight = 1-saturate((slope-grassBlendHeight)/(_GrassSlopeThreshold-grassBlendHeight));

            //Forest
            float forest = smoothstep(_ForestHeight - _ForestBlend, _ForestHeight,IN.worldPos.y);

            //Snow
            float snow = smoothstep(_SnowHeight - _SnowBlend, _SnowHeight,IN.worldPos.y * (1-slope));

           // o.Albedo = lerp(_GrassColour * grassWeight + _RockColour * (1-grassWeight);
           fixed4 outCol =  lerp(_RockColour, _GrassColour, grassWeight);
           outCol = lerp(outCol, _ForestColour, forest * grassWeight);
           outCol = lerp(outCol, _SnowColour, snow);
           o.Albedo = outCol;
        }
        ENDCG
    }
}