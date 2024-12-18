Shader "SH_VFX_Veffects_Add"
{
    Properties
    {
        _Texture("Texture", 2D) = "white" {}
        _EmissiveIntensity("Emissive Intensity", Float) = 1
        _CamOffet_Amount("Camera Offset Amount", Float) = 0
        [Space(33)][Header(AR)][Space(13)]_Cull("Cull", Float) = 0
        _Src("Src", Float) = 5
        _Dst("Dst", Float) = 10
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
        Blend SrcAlpha OneMinusSrcAlpha

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _Texture_ST;
            float _EmissiveIntensity;
            float _CamOffet_Amount;
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

        Varyings vert(Attributes input)
        {
            Varyings output = (Varyings)0;
            
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_TRANSFER_INSTANCE_ID(input, output);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

            // Смещение вершин относительно камеры
            float3 worldPos = TransformObjectToWorld(input.positionOS.xyz);
            float3 offset = (worldPos - _WorldSpaceCameraPos) * (_CamOffet_Amount * 0.01);
            float3 adjustedPos = worldPos + offset;

            // Трансформируем позицию в clip space
            output.positionCS = TransformWorldToHClip(adjustedPos);
            output.uv = TRANSFORM_TEX(input.uv, _Texture);
            output.color = input.color;

            return output;
        }

        float4 frag(Varyings input) : SV_Target
        {
            UNITY_SETUP_INSTANCE_ID(input);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

            // Сэмплирование текстуры
            float texSample = SAMPLE_TEXTURE2D(_Texture, sampler_Texture, input.uv).g;

            // Прозрачность и эмиссивность
            float alpha = texSample * input.color.a;
            float3 emissive = saturate(texSample) * input.color.rgb * _EmissiveIntensity;

            return float4(emissive, alpha); // Возвращаем эмиссивный цвет с прозрачностью
        }
        ENDHLSL

        Pass
        {
            Name "Forward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            ENDHLSL
        }
    }
    CustomEditor "ASEMaterialInspector"
}
