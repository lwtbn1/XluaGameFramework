//极致	贴图描边，漫反射，高光，流光，场景其他灯光
//高	贴图描边，漫反射，高光
//中	描边，漫反射
//低	直接采样

Shader "AkeShader/VFShader/CharacterNew/VFToonChar_M"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MaskTexture("通道贴图 r 高光 g 阴影 b 流光",2D) = "white"{}
		_RampTex("ramp",2D) = "white"{}

		_OutlineColor("描边颜色",color) = (0,0,0,1)		
		_Outline("描边粗细",range(0,0.1))=0.01		// 挤出描边的粗细
		_BaseOutlineDistance("_BaseOutlineDistance",Range(0.1,30)) = 23
		_OutlineModifValue("_OutlineModifValue",Range(0,0.05)) = 0.007
		
		_DiffuseOffset("漫反射偏移系数",Range(-0.5,1.5)) = 0.5
		_DiffuseIntensity("光照强度修正系数",Range(0.5,1.5)) = 1

		_GrayFactor("GrayFactor",Range(0,1)) = 0
		_AddColorIntensity("抢篮板闪白",Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Pass
		{	//处理光照前的pass渲染
			Tags{ "LightMode" = "Always" }
			Name "SD_TOON_OUTLINE_M"
			Cull Front
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "../SceneShader/AkeRenderTools.cginc"
		
			float _Outline;
			fixed4 _OutlineColor;
			sampler2D _OutlineTex;
		
			half _OutlineModifValue;
			half _BaseOutlineDistance;
			half _GrayFactor;
		
			struct a2v
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal:NORMAL;
				float4 color:COLOR;
			};
		
			struct v2f 
			{
				float4 pos:SV_POSITION;
				float2 uv : TEXCOORD0;
		
			};
		
			v2f vert (a2v v)
			{
				v2f o;			
				float4 pos = mul(UNITY_MATRIX_MV, v.vertex); 
				float3 normal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);				
		
				float realOutline = pos.z/_BaseOutlineDistance;
				realOutline = abs(realOutline);
				realOutline = max(0,_Outline + (realOutline - 1) * _OutlineModifValue);
				normal.z = -0.5;
				pos = pos + float4(normalize(normal), 0) * realOutline * v.color.b;
				o.pos = mul(UNITY_MATRIX_P, pos);
				o.uv = v.texcoord;
				return o;
			}
			
			fixed4 frag(v2f i):COLOR
			{
				half4 texCol = _OutlineColor;//不使用贴图时使用颜色
				texCol.rgb = SetColorGrayZeroOrOne(texCol.rgb, _GrayFactor);
				return texCol;
			}
			ENDCG
		}//end of pass
		Pass
		{
			Tags { "LightMode"="ForwardBase" }
			Name "SD_TOON_MAIN_M"
			LOD 200
			Cull Back
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag


            #include "UnityCG.cginc"
            #include "../SceneShader/AkeRenderTools.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _MaskTexture;
			sampler2D _RampTex;

			float4 _Specular;
			half _Shininess;

			float _DiffuseIntensity;
			fixed _DiffuseOffset;

			half _GrayFactor;
			half _AddColorIntensity;

			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
				o.uv = v.texcoord;
                
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half4 texCol = tex2D(_MainTex,i.uv);

				fixed3 maskCol = tex2D(_MaskTexture,i.uv);
				fixed lightValue = maskCol.g;//漫反射	<1时漫反射暗部变多，用于调整局部的漫反射

				//漫反射
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.rgb);
				fixed rampUV = saturate((dot(worldNormal, worldLightDir) * 0.5 + _DiffuseOffset) * lightValue);
				texCol.rgb = tex2D(_RampTex, fixed2(rampUV, 0.25)) * texCol.rgb * _DiffuseIntensity;//漫反射颜色通过_RampTex控制，叠加灯光颜色和漫反射颜色

				texCol.rgb = DoLogicColorChanageZeroOrOne(texCol.rgb, _GrayFactor, _AddColorIntensity, fixed3(1, 1, 1));
				return texCol;
			}
			ENDCG
		}
		//Pass 
		//{  
		//    Tags { "LightMode"="ShadowCaster" }  
		//    Name "SD_TOON_SHADOW_CASTER"
		//    CGPROGRAM  
		//    #pragma vertex vert  
		//    #pragma fragment frag  
		//    #pragma multi_compile_shadowcaster  
		//    #include "UnityCG.cginc"  
		//  
		//    sampler2D _Shadow;  
		//  
		//    struct v2f{  
		//        V2F_SHADOW_CASTER;  
		//        float2 uv:TEXCOORD2;  
		//    };  
		//  
		//    v2f vert(appdata_base v){  
		//        v2f o;  
		//        //o.uv = v.texcoord.xy;  
		//        TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);  
		//        return o;  
		//    }  
		//  
		//    float4 frag( v2f i ) : SV_Target  
		//    {  
		//        //fixed alpha = tex2D(_Shadow, i.uv).a;  
		//        //clip(alpha - 0.5);  
		//        SHADOW_CASTER_FRAGMENT(i)  
		//    }  
		//  
		//    ENDCG  
		//}
	}
}
