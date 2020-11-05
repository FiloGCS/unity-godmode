// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_Debug_Polka"
{
	Properties
	{
		_CircleSize("Circle Size", Range( 0 , 1)) = 0.2
		_Scale("Scale", Range( 0 , 10)) = 10
		_PolkaOpacity("Polka Opacity", Range( 0 , 1)) = 0.5
		_Texture0("Texture 0", 2D) = "white" {}
		_Color("Color", Color) = (0.9716981,0.6246133,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float4 _Color;
		uniform sampler2D _Texture0;
		uniform float _Scale;
		uniform float _CircleSize;
		uniform float _PolkaOpacity;


		inline float4 TriplanarSampling176( sampler2D topTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			half4 xNorm; half4 yNorm; half4 zNorm;
			xNorm = tex2D( topTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm = tex2D( topTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar176 = TriplanarSampling176( _Texture0, ase_worldPos, ase_worldNormal, 1.0, float2( 0.5,0.5 ), 1.0, 0 );
			float4 temp_output_179_0 = ( _Color * triplanar176 );
			float4 color128 = IsGammaSpace() ? float4(0.07092308,0.6886792,0,1) : float4(0.00608761,0.4320358,0,1);
			float3 temp_output_92_0 = ( ( ase_worldPos * _Scale ) + float3( 0.5,0.5,0.5 ) );
			float2 temp_cast_2 = (1.0).xx;
			float temp_output_62_0 = ( 1.0 - saturate( ( distance( float2( 0.5,0.5 ) , fmod( abs( (temp_output_92_0).xz ) , temp_cast_2 ) ) * 2.0 ) ) );
			float temp_output_63_0 = ceil( ( temp_output_62_0 - ( 1.0 - _CircleSize ) ) );
			float dotResult80 = dot( float3(0,1,0) , ase_worldNormal );
			float4 color130 = IsGammaSpace() ? float4(0.1273585,0.3729807,1,1) : float4(0.01480528,0.1147129,1,1);
			float2 temp_cast_3 = (1.0).xx;
			float temp_output_108_0 = ( 1.0 - saturate( ( distance( float2( 0.5,0.5 ) , fmod( abs( (temp_output_92_0).xy ) , temp_cast_3 ) ) * 2.0 ) ) );
			float temp_output_119_0 = ceil( ( temp_output_108_0 - ( 1.0 - _CircleSize ) ) );
			float dotResult115 = dot( float3(0,0,1) , ase_worldNormal );
			float4 color163 = IsGammaSpace() ? float4(0.8207547,0.2056761,0.1664738,1) : float4(0.6396053,0.03490093,0.02360303,1);
			float2 temp_cast_4 = (1.0).xx;
			float temp_output_145_0 = ( 1.0 - saturate( ( distance( float2( 0.5,0.5 ) , fmod( abs( (temp_output_92_0).yz ) , temp_cast_4 ) ) * 2.0 ) ) );
			float temp_output_154_0 = ceil( ( temp_output_145_0 - ( 1.0 - _CircleSize ) ) );
			float dotResult151 = dot( float3(1,0,0) , ase_worldNormal );
			float4 temp_output_126_0 = ( ( color128 * ( ( temp_output_63_0 * saturate( dotResult80 ) ) + ( ( temp_output_63_0 * ( 1.0 - ceil( ( temp_output_62_0 - ( 1.0 - ( _CircleSize - ( _CircleSize / 4.0 ) ) ) ) ) ) ) * saturate( ( dotResult80 * -1.0 ) ) ) ) ) + ( color130 * ( ( temp_output_119_0 * saturate( dotResult115 ) ) + ( ( temp_output_119_0 * ( 1.0 - ceil( ( temp_output_108_0 - ( 1.0 - ( _CircleSize - ( _CircleSize / 4.0 ) ) ) ) ) ) ) * saturate( ( dotResult115 * -1.0 ) ) ) ) ) + ( color163 * ( ( temp_output_154_0 * saturate( dotResult151 ) ) + ( ( temp_output_154_0 * ( 1.0 - ceil( ( temp_output_145_0 - ( 1.0 - ( _CircleSize - ( _CircleSize / 4.0 ) ) ) ) ) ) ) * saturate( ( dotResult151 * -1.0 ) ) ) ) ) );
			float4 break170 = temp_output_126_0;
			float4 lerpResult183 = lerp( temp_output_179_0 , ( ( temp_output_179_0 * ( 1.0 - saturate( ( break170.r + break170.g + break170.b ) ) ) ) + temp_output_126_0 ) , _PolkaOpacity);
			o.Emission = lerpResult183.rgb;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
235;73;1263;624;-360.3232;833.8882;2.158815;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;52;-2458.771,-1046.919;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;91;-2463.205,-850.8837;Inherit;False;Property;_Scale;Scale;1;0;Create;True;0;0;False;0;False;10;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-2235.205,-987.8837;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;165;-1894.149,-548.2217;Inherit;False;3022.644;1939.204;Polka;88;59;77;54;51;55;58;75;61;76;62;72;67;70;82;79;71;64;80;73;83;63;85;69;86;88;87;128;89;127;98;100;99;101;102;105;104;107;106;109;108;111;112;113;110;114;115;116;119;118;117;120;122;121;123;124;130;125;129;135;136;137;138;140;142;141;144;143;145;146;150;148;147;149;153;151;152;156;154;155;158;159;157;160;161;163;162;164;126;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;92;-2097.205,-967.8837;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.5,0.5,0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;135;-1839.239,828.7947;Inherit;False;False;True;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;98;-1844.149,175.4229;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;59;-1823.306,-473.7536;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-1660.159,298.9908;Inherit;False;Constant;_Float1;Float 1;1;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;100;-1640.149,178.4229;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;137;-1655.249,952.3627;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;54;-1619.306,-470.7536;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-1639.316,-350.1857;Inherit;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;136;-1635.239,831.7947;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FmodOpNode;51;-1444.359,-449.993;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FmodOpNode;138;-1479.832,876.0027;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FmodOpNode;101;-1484.742,222.631;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DistanceOpNode;140;-1281.04,851.4946;Inherit;False;2;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;102;-1285.95,198.1229;Inherit;False;2;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-1451.17,-952.7846;Inherit;False;Property;_CircleSize;Circle Size;0;0;Create;True;0;0;False;0;False;0.2;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;55;-1265.107,-451.0536;Inherit;False;2;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1091.106,-420.0536;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;105;-998.249,472.323;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;75;-977.4055,-176.8535;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-1111.949,229.1229;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;141;-993.3387,1125.695;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-1107.039,882.4946;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;107;-985.9491,230.1229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;144;-981.0388,883.4946;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;61;-965.1056,-419.0536;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;-859.6054,-179.9535;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;143;-875.5387,1122.595;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;-880.4489,469.223;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;108;-851.9489,230.1229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;62;-831.1054,-419.0536;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;72;-726.1049,-177.5535;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;109;-746.9484,471.623;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;145;-847.0387,883.4946;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;146;-742.0381,1124.995;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;111;-858.649,395.9232;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;110;-406.4374,416.6111;Inherit;False;Constant;_Vector1;Vector 1;1;0;Create;True;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;150;-401.5271,1069.983;Inherit;False;Constant;_Vector2;Vector 2;1;0;Create;True;0;0;False;0;False;1,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;147;-586.8382,977.1946;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;148;-853.7387,1049.295;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;149;-413.5271,1211.983;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;82;-385.5939,-232.5654;Inherit;False;Constant;_Vector0;Vector 0;1;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;112;-418.4374,558.6111;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;79;-397.5939,-90.56538;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;67;-837.8055,-253.2533;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;70;-570.905,-325.3532;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;113;-591.7485,323.8233;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;115;-198.4374,474.6111;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;151;-193.5271,1127.983;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;153;-585.3381,881.4946;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;114;-458.848,324.7233;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;64;-569.4049,-421.0536;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;116;-590.2484,228.1229;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;71;-438.0045,-324.4532;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;80;-177.5939,-174.5654;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;152;-453.9377,978.0946;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-70.43741,541.6111;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;73;-317.7042,-326.6532;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;155;-333.6374,975.8947;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;63;-427.4046,-419.9536;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-49.59394,-107.5654;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;154;-443.3378,882.5946;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;-65.52713,1194.983;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CeilOpNode;119;-448.2481,229.2229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;118;-338.5477,322.5233;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;120;65.5626,540.6111;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-174.3379,884.3947;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;121;-179.2482,231.023;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;159;-21.52712,1107.983;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;86;86.40613,-108.5654;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;157;70.47289,1193.983;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-158.4047,-418.1535;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;122;-26.43742,454.6111;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;85;-5.593913,-194.5654;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;123;260.5626,492.6111;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;161;157.4729,971.9827;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;152.5626,318.6111;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;173.4061,-330.5654;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;281.4061,-156.5654;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;265.4729,1145.983;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;128;306.7825,-498.2217;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;False;0.07092308,0.6886792,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;130;352.8187,198.4445;Inherit;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;False;0.1273585,0.3729807,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;125;369.5626,385.6111;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;162;374.4729,1038.983;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;163;357.729,851.8167;Inherit;False;Constant;_Color2;Color 2;4;0;Create;True;0;0;False;0;False;0.8207547,0.2056761,0.1664738,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;89;390.4061,-263.5654;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;164;655.0292,957.0726;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;552.2539,-346.6404;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;129;650.1189,303.701;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;126;974.4951,288.75;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;170;1664.84,239.7986;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;169;1944.84,238.7986;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;175;1287.9,-48.79399;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexturePropertyNode;177;1284.9,-289.794;Inherit;True;Property;_Texture0;Texture 0;3;0;Create;True;0;0;False;0;False;8eecd0e5d53784743ab303d6d161cfcc;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SaturateNode;171;2058.84,211.7986;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;176;1644.9,-240.794;Inherit;True;Spherical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT;1;False;3;FLOAT2;0.5,0.5;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;181;1760.346,-425.7826;Inherit;False;Property;_Color;Color;4;0;Create;True;0;0;False;0;False;0.9716981,0.6246133,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;172;2238.84,151.7986;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;179;2014.346,-303.7826;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;173;2400.84,57.79855;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;167;1967.509,555.3519;Inherit;False;Property;_PolkaOpacity;Polka Opacity;2;0;Create;True;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;174;2531.75,338.4148;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;183;2775.077,310.7188;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3002.977,333.0702;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SH_Debug_Polka;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;90;0;52;0
WireConnection;90;1;91;0
WireConnection;92;0;90;0
WireConnection;135;0;92;0
WireConnection;98;0;92;0
WireConnection;59;0;92;0
WireConnection;100;0;98;0
WireConnection;54;0;59;0
WireConnection;136;0;135;0
WireConnection;51;0;54;0
WireConnection;51;1;77;0
WireConnection;138;0;136;0
WireConnection;138;1;137;0
WireConnection;101;0;100;0
WireConnection;101;1;99;0
WireConnection;140;1;138;0
WireConnection;102;1;101;0
WireConnection;55;1;51;0
WireConnection;58;0;55;0
WireConnection;105;0;66;0
WireConnection;75;0;66;0
WireConnection;104;0;102;0
WireConnection;141;0;66;0
WireConnection;142;0;140;0
WireConnection;107;0;104;0
WireConnection;144;0;142;0
WireConnection;61;0;58;0
WireConnection;76;0;66;0
WireConnection;76;1;75;0
WireConnection;143;0;66;0
WireConnection;143;1;141;0
WireConnection;106;0;66;0
WireConnection;106;1;105;0
WireConnection;108;0;107;0
WireConnection;62;0;61;0
WireConnection;72;0;76;0
WireConnection;109;0;106;0
WireConnection;145;0;144;0
WireConnection;146;0;143;0
WireConnection;111;0;66;0
WireConnection;147;0;145;0
WireConnection;147;1;146;0
WireConnection;148;0;66;0
WireConnection;67;0;66;0
WireConnection;70;0;62;0
WireConnection;70;1;72;0
WireConnection;113;0;108;0
WireConnection;113;1;109;0
WireConnection;115;0;110;0
WireConnection;115;1;112;0
WireConnection;151;0;150;0
WireConnection;151;1;149;0
WireConnection;153;0;145;0
WireConnection;153;1;148;0
WireConnection;114;0;113;0
WireConnection;64;0;62;0
WireConnection;64;1;67;0
WireConnection;116;0;108;0
WireConnection;116;1;111;0
WireConnection;71;0;70;0
WireConnection;80;0;82;0
WireConnection;80;1;79;0
WireConnection;152;0;147;0
WireConnection;117;0;115;0
WireConnection;73;0;71;0
WireConnection;155;0;152;0
WireConnection;63;0;64;0
WireConnection;83;0;80;0
WireConnection;154;0;153;0
WireConnection;156;0;151;0
WireConnection;119;0;116;0
WireConnection;118;0;114;0
WireConnection;120;0;117;0
WireConnection;158;0;154;0
WireConnection;158;1;155;0
WireConnection;121;0;119;0
WireConnection;121;1;118;0
WireConnection;159;0;151;0
WireConnection;86;0;83;0
WireConnection;157;0;156;0
WireConnection;69;0;63;0
WireConnection;69;1;73;0
WireConnection;122;0;115;0
WireConnection;85;0;80;0
WireConnection;123;0;121;0
WireConnection;123;1;120;0
WireConnection;161;0;154;0
WireConnection;161;1;159;0
WireConnection;124;0;119;0
WireConnection;124;1;122;0
WireConnection;87;0;63;0
WireConnection;87;1;85;0
WireConnection;88;0;69;0
WireConnection;88;1;86;0
WireConnection;160;0;158;0
WireConnection;160;1;157;0
WireConnection;125;0;124;0
WireConnection;125;1;123;0
WireConnection;162;0;161;0
WireConnection;162;1;160;0
WireConnection;89;0;87;0
WireConnection;89;1;88;0
WireConnection;164;0;163;0
WireConnection;164;1;162;0
WireConnection;127;0;128;0
WireConnection;127;1;89;0
WireConnection;129;0;130;0
WireConnection;129;1;125;0
WireConnection;126;0;127;0
WireConnection;126;1;129;0
WireConnection;126;2;164;0
WireConnection;170;0;126;0
WireConnection;169;0;170;0
WireConnection;169;1;170;1
WireConnection;169;2;170;2
WireConnection;171;0;169;0
WireConnection;176;0;177;0
WireConnection;176;9;175;0
WireConnection;172;0;171;0
WireConnection;179;0;181;0
WireConnection;179;1;176;0
WireConnection;173;0;179;0
WireConnection;173;1;172;0
WireConnection;174;0;173;0
WireConnection;174;1;126;0
WireConnection;183;0;179;0
WireConnection;183;1;174;0
WireConnection;183;2;167;0
WireConnection;0;2;183;0
ASEEND*/
//CHKSM=26E85CB1D61B4E049755680D790240122EFE0127