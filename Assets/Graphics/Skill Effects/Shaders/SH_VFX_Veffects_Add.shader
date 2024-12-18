Shader "SH_VFX_Veffects_Add"
{
    Properties
    {
        _Texture("Texture", 2D) = "white" {}
        _EmissiveIntensity("Emissive Intensity", Float) = 1
        [Space(33)][Header(AR)][Space(13)]_Cull("Cull", Float) = 0
        _Src("Src", Float) = 1
        _Dst("Dst", Float) = 1
        _ZWrite("ZWrite", Float) = 0
        _ZTest("ZTest", Float) = 2
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent+0" 
            "IgnoreProjector" = "True" 
            "IsEmissive" = "true"
            "RenderPipeline" = "UniversalPipeline"
        }

        Cull [_Cull]
        ZWrite [_ZWrite]
        ZTest [_ZTest]
        Blend [_Src] [_Dst]

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _Texture_ST;
            float _EmissiveIntensity;
            float _ZWrite;
            float _ZTest;
            float _Src;
            float _Dst;
            float _Cull;
        CBUFFER_END

        TEXTURE2D(_Texture);
        SAMPLER(sampler_Texture);

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
            float4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            float4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };
        ENDHLSL

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _Texture);
                output.color = input.color;
                
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // Sample texture and get green channel (как в оригинальном шейдере)
                float texSample = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, input.uv).g;
                float3 emissive = saturate(texSample) * input.color.rgb * _EmissiveIntensity * input.color.a;
                
                return float4(emissive, 1);
            }
            ENDHLSL
        }
    }
    CustomEditor "ASEMaterialInspector"
}