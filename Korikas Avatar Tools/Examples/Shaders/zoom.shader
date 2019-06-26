//Shader by Neen, if you have any questions forward them to me on discord.
//Object xyz scale needs to be set at or below (.0001) for magnification to work as intended - Weird results else.
//Version 1.2
//If you've got this without purchasing it, that's not nice :( *but who cares this whole site is full of pirated assets and shit so dont cry little boi*  using shaders you didn't purchase is like stealing avatars!(people still steal avatars? how big does your ego have to be to get mad over something like that lmao) Contact me at Neen#2693 if you found this posted somewhere for a reward!(ugh k)

    Shader "KAT/zoom" {

        Properties {
        	[Header(Screen Shift)]
                _intensity ("Vertical Shift", Range(-50, 50)) = 0
                _intensity2 ("Horizontal Shift", Range(-50, 50)) = 0

                _yper ("Y-period", Range(-200, 200)) = 1
                _xper ("X-period", Range(-200, 200)) = 1

        	[Space(20)]
        	[Header(Magnification)]
          [Space(5)]
          [Toggle(MAGNIFIED_ON)]_MG("Magnified Toggle", Float) = 0.0 //Toggle magnification on or off
          [Toggle(MAGNIFY_SMOOTH1)]_MGS1("Magnification Smoothing", Float) = 0.0 //Toggle magnification smoothing within vert
          [Toggle(MAGNIFY_SMOOTH)]_MGS2("Magnification Smoothing Alternative", Float) = 0.0 //Toggle magnification smoothing within frag
          [Space(5)]
                _mag("Magnification", Range (1.0, 50.0)) = 8

          [Space(20)]
          [Header(Blur)]
          [Space(5)]
          [Toggle(BLUR_ENABLED)]_BLUR("Blur Toggle", Float) = 0.0 //Toggle blur on or off
          [Toggle(BLUR_FALLOFF)]_BLURFO("Blur Falloff Toggle", Float) = 0.0 //Toggle blur falloff on or off
          [Space(5)]
                  _blurfactor ("Blur Factor", Range(0, 5)) = 1.0

          [Space(20)]
          [Header(Falloff)]
          [Space(5)]
                _max ("Max Range", Range (0.0, 20.0)) = 3.0
                _min ("Minimum Range", Range (0.0, 20.0)) = 0.0


          [Space(20)]
          [Header(Color Tint)]
          [Space(5)]
                _redvalue ("Red Tint", Range (0.0, 25.0)) = 0.3
                _greenvalue ("Green Tint", Range (0.0, 25.0)) = 0.0
                _bluevalue ("Blue Tint", Range (0.0, 25.0)) = 0.0
                [Toggle(COLOR_BLACK_OUT)]_blackout ("Color Fade", Float) = 0.0 //Toggle color to fade with black tint
                _blackvalue ("Black Tint", Range (0.0, 1.0)) = 0.0

          [Space (20)]
                [Toggle(COLOR_INVERT)]_CINV ("Invert Colors", Float) = 0.0 //Toggles Color Inversion
                [Toggle(COLIN_FO)]_CIFO ("Inversin Falloff Toggle", Float) = 0.0 //Toggles Color Inversion falloff
                _redinv("Red Invert Value", Range (0.0, 25.0)) = 1.0
                _greeninv("Green Invert Value", Range (0.0, 25.0)) = 1.0
                _blueinv("Blue Invert Value", Range (0.0, 25.0)) = 1.0

          [Header(Ignore me OwO)]
                _slider ("Slider ", Range (0.0, 100.0)) = 16.0

        }

        SubShader {

                Tags { "Queue"="Overlay+4001" "RenderType"="Transparent" }
                //Blend SrcAlpha OneMinusSrcAlpha
                Cull Off
                ZTest Off
                ZWrite On

        GrabPass {
                Tags { "Queue"="Overlay+1" }
                "_GP2"
			}

        Pass {
                Tags { "Queue"="Overlay+1" }


                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma shader_feature COLOR_BLACK_OUT
                #pragma shader_feature MAGNIFIED_ON
                #pragma shader_feature MAGNIFY_SMOOTH1
                #pragma shader_feature MAGNIFY_SMOOTH2
                #pragma shader_feature INVERT_VIEW
                #pragma shader_feature BLUR_ENABLED
                #pragma shader_feature BLUR_FALLOFF
                #pragma shader_feature COLOR_INVERT
                #pragma shader_feature COLIN_FO
                #include "UnityCG.cginc"

                struct v2f {
                    float4 pos : SV_POSITION;
                    half4 grabPos : TEXCOORD0;
                    float4 worldPos : TEXCOORD1;
                    float4 dist: TEXCOORD2;
                    float4 uv : TEXCOORD3;
                    float3 viewDir : TEXCOORD4;
                    float ang : TEXCOORD5;
                };

                    sampler2D _GP2;
                    float4 _GP2_TexelSize;

                    half _intensity;
                    half _intensity2;

                    half _yper;
                    half _xper;

                    half _max;
                    half _min;

                    half _blurfactor;

                    half _mag;

                    half _redvalue;
                    half _greenvalue;
                    half _bluevalue;
                    half _blackvalue;

                    half _redinv;
                    half _greeninv;
                    half _blueinv;


                    //half _slider;

                v2f vert(float4 pos : POSITION, appdata_base v) 
				{
                    v2f o;
                    //cameraPos =  mul(unity_WorldToObject, cameraPos);


                      float4 abspos = pos;
                      float4 worldPos2  = mul(unity_CameraToWorld, abspos);   // Transform View to World
					            float4 objectPos2 = mul(unity_WorldToObject, worldPos2); //Transform Object to screen dimensions


                      o.dist = distance(_WorldSpaceCameraPos, mul(unity_ObjectToWorld, float4(0,0,0,1)));
                      o.pos = UnityObjectToClipPos(objectPos2);
                      o.pos = o.pos - o.pos*step(_max, o.dist);
                      //o.pos = UnityObjectToClipPos(v.vertex);
                      o.grabPos = ComputeGrabScreenPos(o.pos);
                      o.worldPos = worldPos2;
				              o.viewDir = mul(_WorldSpaceCameraPos, float4(0,0,1,1));

                      half falloff2 = pow(min(1, 1 - (1/(_max - _min))*(o.dist - _min)), 2);

                      //float4 uv_center = ComputeGrabScreenPos(UnityObjectToClipPos(float4(0, 0, 0, 1)));
                      half4 uv_center = ComputeGrabScreenPos(UnityObjectToClipPos(v.vertex));
                      half4 UVDif = ComputeGrabScreenPos(o.pos) - uv_center;

                      //float3 forward = normalize(o.viewDir.xyz);
                      //float4 forward = mul(unity_CameraToWorld, float4(0,0,1,0));
                      //float4 verttocam = mul(unity_ObjectToWorld, float4(0,0,1,0)) - forward;

                      //float DotDiff = abs(dot(forward,verttocam));

                      #if UNITY_SINGLE_PASS_STEREO
                        float4 cameraPos = float4((unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1])*0.5, 1);
                      #else
                        float4 cameraPos = float4(_WorldSpaceCameraPos,1);
                      #endif

                      #if UNITY_SINGLE_PASS_STEREO
                        float3 p1 =  mul(unity_StereoCameraToWorld[0], float4(0,0,1,1));
                        float3 p2 =  mul(unity_StereoCameraToWorld[0], float4(0,0,0,1));
                      #else
                        float3 p1 =  mul(unity_CameraToWorld, float4(0,0,1,1));
                        float3 p2 =  mul(unity_CameraToWorld, float4(0,0,0,1));
                      #endif

					            float4 wpos = mul(unity_ObjectToWorld, float4(0,0,0,1));
                      float3 forward = normalize(p2-p1);
                      float3 cameraDir = normalize(cameraPos.xyz - wpos.xyz);

                  		o.ang = saturate(dot(forward, cameraDir));
                      float dotang1 = saturate(1.5*cos(clamp(acos(o.ang)*10*max(1,o.dist*0.25), 0, 1.58)));

                      //UVDif /= lerp(_mag,(1-DotDiff),.5);
                      UVDif /= _mag;
                      #ifdef MAGNIFY_SMOOTH1
                        o.uv = (uv_center + UVDif)*falloff2*dotang1;
                      #else
                        o.uv = (uv_center + UVDif)*falloff2;
                      #endif
                      //o.uv = lerp(uv_center,(uv_center + UVDif),1);
                      //o.uv = (uv_center + UVDif);





                      return o;
                }



                half4 frag(v2f i) : COLOR {
                    #if UNITY_SINGLE_PASS_STEREO
                      int factor = 250;
                    #else
                      int factor = 125;
                    #endif

                    float dotang2 = saturate(1.5*cos(clamp(acos(i.ang)*10*max(1,i.dist*0.25), 0, 1.58)));
                    #if MAGNIFY_SMOOTH2
                      i.uv = lerp(i.grabPos,i.uv,dotang2);
                    #endif

                    half falloff = pow(min(1, 1 - (1/(_max - _min))*(i.dist - _min)), 2);
                    i.grabPos.y = i.grabPos.y + sin(_Time[1]*_yper)*(_intensity/factor)*falloff;
                    i.grabPos.x = i.grabPos.x + sin(_Time[1]*_xper)*(_intensity2/factor)*falloff*(_ScreenParams[1]/_ScreenParams[0]);
                    //i.grabPos.y = i.grabPos.y / (_intensity);
                    //i.grabPos.x = i.grabPos.x / (_intensity2)*(_ScreenParams[1]/_ScreenParams[0]);

                    //float3 CamDir = normalize(i.viewDir.xyz);
                    //float3 ObjCen = normalize(i.dist.xyz-i.viewDir.xyz);
                    //float4 DotDif = dot(CamDir,ObjCen);


                    #ifdef MAGNIFIED_ON
                      float4 shakeZoom = lerp(i.uv,i.grabPos,.05);
                    #else
                      float4 shakeZoom = i.grabPos;
                    #endif

                    //float4 shakeZoom = lerp(i.uv,i.grabPos,.05);
                    //float4 shakeZoomG = shakeZoom + float4(texdisp2.xy,0,0);


                    float4 color1 = tex2Dproj(_GP2, UNITY_PROJ_COORD(shakeZoom));
                    float4 color = lerp(float4(0,0,0,0),color1,(1-_blackvalue));
                    //float4 color = lerp(color2,color1,.2);
                    //float color2 = tex2Dproj(_GP2, UNITY_PROJ_COORD(shakeZoom)) * float4(0,0,0,0);
                    //float4 color = lerp(color1,color2,float4(.51,0,0,0));
                    //float4 color = lerp(float4 (sp.a, color2.g,color1.r,0),nK.rgb,.5);
                    //float4 color = lerp(color1,tex2Dproj(_GP2, UNITY_PROJ_COORD(shakeZoomG)),0.4);


                    //float4 color = tex2Dproj(_GP2, UNITY_PROJ_COORD(shakeZoom));
                    half4 invertcol = float4(_redinv,_greeninv,_blueinv,1);

                    //Add in color
                    #ifdef COLOR_BLACK_OUT
                      color.rgb += lerp (float3(0,0,0),float3(_redvalue*falloff,_greenvalue*falloff,_bluevalue*falloff),((1-_blackvalue)));
            				#else
                      color.rgb += float3(_redvalue*falloff,_greenvalue*falloff,_bluevalue*falloff);
            				#endif

                    //float4 color = tex2Dproj(_GP2, UNITY_PROJ_COORD(i.uv)) + float4 (.10,0,0,0);
                    //float4 color = (N,e,e,n)
                    //return tex2Dproj(_GP2, UNITY_PROJ_COORD(i.grabPos));

                      #ifdef BLUR_ENABLED
                        #ifdef BLUR_FALLOFF
                          #define ADDPIXEL(weight,kernelX) tex2Dproj(_GP2, UNITY_PROJ_COORD(float4(shakeZoom.x + _GP2_TexelSize.x * kernelX * _blurfactor, shakeZoom.y, shakeZoom.z, shakeZoom.w))) * weight * falloff * _blackvalue
                        #else
                          #define ADDPIXEL(weight,kernelX) tex2Dproj(_GP2, UNITY_PROJ_COORD(float4(shakeZoom.x + _GP2_TexelSize.x * kernelX * _blurfactor, shakeZoom.y, shakeZoom.z, shakeZoom.w))) * weight
                        #endif
                        color += ADDPIXEL(0.05, 4.0);
                        color += ADDPIXEL(0.09, 3.0);
                        color += ADDPIXEL(0.12, 2.0);
                        color += ADDPIXEL(0.15, 1.0);
                        color += ADDPIXEL(0.18, 0.0);
                        color += ADDPIXEL(0.15, -1.0);
                        color += ADDPIXEL(0.12, -2.0);
                        color += ADDPIXEL(0.09, -3.0);
                        color += ADDPIXEL(0.05, -4.0);
                        #ifdef COLOR_INVERT
                          #ifdef COLIN_FO
                            return ((invertcol-color)*falloff)+(color*(1-falloff));
                          #else
                            return (invertcol-color);
                          #endif
                        #else
                          return color;
                        #endif
                      #else
                        #ifdef COLOR_INVERT
                          #ifdef COLIN_FO
                            return ((invertcol-color)*falloff)+(color*(1-falloff));
                          #else
                            return (invertcol-color);
                          #endif
                        #else
                        return color;
                      #endif
                    #endif

                }
                ENDCG
            }
        }
        FallBack "Diffuse"
}
