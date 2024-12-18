// Made with Amplify Shader Editor
Shader "SH_VFX_Veffects_Slash"
{
    Properties
    {
        [Space(13)][Header(Slash)][Space(13)]_Slash_Texture("Slash_Texture", 2D) = "white" {}
        _Slash_Scale("Slash_Scale", Float) = 1
        _Slash_Speed("Slash_Speed", Float) = 1
        [Space(13)][Header(Slash Noise)][Space(13)]_Slash_Noise_Texture("Slash_Noise_Texture", 2D) = "white" {}
        _Slash_Noise_Scale("Slash_Noise_Scale", Vector) = (1,1,0,0)
        _Slash_Noise_Speed("Slash_Noise_Speed", Vector) = (-1,0.5,0,0)
        _Slash_Noise_Intensity("Slash_Noise_Intensity", Float) = 1
        [Space(13)][Header(Emissive)][Space(13)]_Emissive_Slash_Texture("Emissive_Slash_Texture", 2D) = "white" {}
        _Emissive_Slash_Scale("Emissive_Slash_Scale", Float) = 1
        _Emissive_Slash_Speed("Emissive_Slash_Speed", Float) = 1
        _Emissive_Intensity("Emissive_Intensity", Float) = 3
        [Space(13)][Header(Emissive Dissolve)][Space(13)]_Emissive_Dissolve_Texture("Emissive_Dissolve_Texture", 2D) = "white" {}
        _Emissive_Dissolve_Scale("Emissive_Dissolve_Scale", Vector) = (1,1,0,0)
        _Emissive_Dissolve_Speed("Emissive_Dissolve_Speed", Vector) = (1,1,0,0)
        [Space(13)][Header(Distortion)][Space(13)]_Distortion_Noise_Texture("Distortion_Noise_Texture", 2D) = "white" {}
        _Distortion_Noise_Scale("Distortion_Noise_Scale", Vector) = (1,1,0,0)
        _Distortion_Noise_Speed("Distortion_Noise_Speed", Vector) = (1,1,0,0)
        _Distortion_Intensity("Distortion_Intensity", Float) = 1
        [Space(13)][Header(Color Noise)][Space(13)]_Color_Noise_Texture("Color_Noise_Texture", 2D) = "white" {}
        _ColorNoise_Scale("ColorNoise_Scale", Vector) = (1,1,0,0)
        _ColorNoise_Speed("ColorNoise_Speed", Vector) = (1,1,0,0)
        _Color_Boost("Color_Boost", Float) = 1
        [Space(13)][Header(Opacity)][Space(13)]_Mask("Mask", 2D) = "white" {}
        _Opacity_Boost("Opacity_Boost", Float) = 1
        [Space(13)][Header(Colors)][Space(13)]_Color_1("Color_1", Color) = (1,0,0.6261435,0)
        _Color_2("Color_2", Color) = (0.06587124,0,1,0)
        _Emissive_Color("Emissive_Color", Color) = (1,0,0.6261435,0)
        [Space(33)][Header(AR)][Space(13)]_Cull("Cull", Float) = 0
        _Src("Src", Float) = 5
        _Dst("Dst", Float) = 10
        _ZWrite("ZWrite", Float) = 0
        _ZTest("ZTest", Float) = 2
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+0" "IgnoreProjector"="True" "IsEmissive"="true" "RenderPipeline"="UniversalPipeline" }
        
        Cull [_Cull]
        ZWrite [_ZWrite]
        ZTest [_ZTest]
        Blend [_Src] [_Dst]

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float _Slash_Scale;
            float _Slash_Speed;
            float _Slash_Noise_Intensity;
            float2 _Slash_Noise_Scale;
            float2 _Slash_Noise_Speed;
            float _Emissive_Slash_Scale;
            float _Emissive_Slash_Speed;
            float2 _Emissive_Dissolve_Scale;
            float2 _Emissive_Dissolve_Speed;
            float _Distortion_Intensity;
            float2 _Distortion_Noise_Scale;
            float2 _Distortion_Noise_Speed;
            float2 _ColorNoise_Scale;
            float2 _ColorNoise_Speed;
            float _Color_Boost;
            float _Opacity_Boost;
            float4 _Color_1;
            float4 _Color_2;
            float4 _Emissive_Color;
            float4 _Mask_ST;
            float _Cull;
            float _ZWrite;
            float _Src;
            float _Dst;
            float _ZTest;
            float _Emissive_Intensity;
        CBUFFER_END

        TEXTURE2D(_Slash_Texture);
        TEXTURE2D(_Slash_Noise_Texture);
        TEXTURE2D(_Emissive_Slash_Texture);
        TEXTURE2D(_Emissive_Dissolve_Texture);
        TEXTURE2D(_Distortion_Noise_Texture);
        TEXTURE2D(_Color_Noise_Texture);
        TEXTURE2D(_Mask);
        SAMPLER(sampler_Slash_Texture);
        SAMPLER(sampler_Slash_Noise_Texture);
        SAMPLER(sampler_Emissive_Slash_Texture);
        SAMPLER(sampler_Emissive_Dissolve_Texture);
        SAMPLER(sampler_Distortion_Noise_Texture);
        SAMPLER(sampler_Color_Noise_Texture);
        SAMPLER(sampler_Mask);

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 color : COLOR;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 color : COLOR;
            float4 uv0 : TEXCOORD0;
            float4 uv1 : TEXCOORD1;
            float3 positionWS : TEXCOORD2;
            float3 normalWS : TEXCOORD3;
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
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.color = input.color;
                output.uv0 = input.uv0;
                output.uv1 = input.uv1;
                
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // Color Noise
                float2 colorNoiseUV = input.uv0.xy * _ColorNoise_Scale + (_Time.y * _ColorNoise_Speed);
                float colorNoise = SAMPLE_TEXTURE2D(_Color_Noise_Texture, sampler_Color_Noise_Texture, colorNoiseUV).r;
                float3 baseColor = lerp(_Color_1.rgb, _Color_2.rgb, colorNoise);

                // Distortion
                float2 distortionUV = input.uv0.xy * _Distortion_Noise_Scale + (_Time.y * _Distortion_Noise_Speed);
                float distortion = SAMPLE_TEXTURE2D(_Distortion_Noise_Texture, sampler_Distortion_Noise_Texture, distortionUV).r * 0.1 * _Distortion_Intensity;
                float2 distortionOffset = float2(distortion, distortion);

                // Slash
                float2 slashUV = input.uv0.xy * float2(_Slash_Scale, 1.0) + (_Time.y * float2(_Slash_Speed, 0.0));
                float slash = SAMPLE_TEXTURE2D(_Slash_Texture, sampler_Slash_Texture, slashUV + distortionOffset).r;


                // Slash Noise
                float2 slashNoiseUV = input.uv0.xy * _Slash_Noise_Scale + (_Time.y * _Slash_Noise_Speed);
                float slashNoise = SAMPLE_TEXTURE2D(_Slash_Noise_Texture, sampler_Slash_Noise_Texture, slashNoiseUV).g;
                float slashFinal = saturate((slash * _Slash_Noise_Intensity) + slashNoise);

                // Mask
                float2 maskUV = input.uv0.xy * _Mask_ST.xy + _Mask_ST.zw;
                float mask = SAMPLE_TEXTURE2D(_Mask, sampler_Mask, maskUV).r;

                // Emissive
                float2 emissiveUV = input.uv0.xy * float2(_Emissive_Slash_Scale, 1.0) + 
                                   (_Time.y * float2(_Emissive_Slash_Speed, 0.0)) + distortionOffset;
                float emissive = SAMPLE_TEXTURE2D(_Emissive_Slash_Texture, sampler_Emissive_Slash_Texture, emissiveUV).g;

                float2 dissolveUV = input.uv0.xy * _Emissive_Dissolve_Scale + (_Time.y * _Emissive_Dissolve_Speed);
                float dissolve = SAMPLE_TEXTURE2D(_Emissive_Dissolve_Texture, sampler_Emissive_Dissolve_Texture, dissolveUV).r;

                // Final Color
                float3 finalColor = baseColor * _Color_Boost * mask * slashFinal;
                float3 emission = input.color.rgb * _Emissive_Color.rgb * emissive * dissolve * mask * _Emissive_Intensity;
                finalColor += emission;

                // Alpha
                float alpha = input.color.a * saturate((mask * slashFinal * _Opacity_Boost));
                alpha = saturate(alpha);

                return float4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "ASEMaterialInspector"
}