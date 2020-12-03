Shader "LHY/SimpleWater"
{
    Properties
    {
        _Color ("WaterColor", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Pass
        {
            Tags 
            { 
                "RenderPipeline"="UniversalPipeline"
                "RenderType"="Transparent"
                "Queue"="Transparent" 
            }

            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            Cull Back
            ZTest LEqual
            ZWrite Off

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            CBUFFER_END
            
            TEXTURE2D(_CameraOpaqueTexture); 
            SAMPLER(sampler_CameraOpaqueTexture);

            struct Attributes
            {
                half4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                half4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half4 screenPos : TEXCOORD1;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                output.positionCS = vertexInput.positionCS;
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.uv = input.uv;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float4 color = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.screenPos.xy / input.screenPos.w);

                return float4(color.rgb * _Color, 1);
                //return half4(_Color.rgb * color, _Color.a);
            }

            ENDHLSL
        }
    }

}
