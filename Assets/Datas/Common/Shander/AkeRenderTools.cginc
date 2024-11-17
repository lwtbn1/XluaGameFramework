
#ifndef AKERENDERTOOLS_INCLUDED
#define AKERENDERTOOLS_INCLUDED

fixed3 calculateAmbientLight(half3 normalWorld)
{
	//Flat ambient is just the sky color
	fixed3 ambient = unity_AmbientSky.rgb * 0.75;    
	//Magic constants used to tweak ambient to approximate pixel shader spherical harmonics 
	fixed3 worldUp = fixed3(0,1,0);
	float skyGroundDotMul = 2.5;
	float minEquatorMix = 0.5;
	float equatorColorBlur = 0.33;

	float upDot = dot(normalWorld, worldUp);

	//Fade between a flat lerp from sky to ground and a 3 way lerp based on how bright the equator light is.
	//This simulates how directional lights get blurred using spherical harmonics

	//Work out color from ground and sky, ignoring equator
	float adjustedDot = upDot * skyGroundDotMul;
	fixed3 skyGroundColor = lerp(unity_AmbientGround, unity_AmbientSky, saturate((adjustedDot + 1.0) * 0.5));

	//Work out equator lights brightness
	float equatorBright = saturate(dot(unity_AmbientEquator.rgb, unity_AmbientEquator.rgb));

	//Blur equator color with sky and ground colors based on how bright it is.
	fixed3 equatorBlurredColor = lerp(unity_AmbientEquator, saturate(unity_AmbientEquator + unity_AmbientGround + unity_AmbientSky), equatorBright * equatorColorBlur);

	//Work out 3 way lerp inc equator light
	float smoothDot = pow(abs(upDot), 1);
	fixed3 equatorColor = lerp(equatorBlurredColor, unity_AmbientGround, smoothDot) * step(upDot, 0) + lerp(equatorBlurredColor, unity_AmbientSky, smoothDot) * step(0, upDot);

	return lerp(skyGroundColor, equatorColor, saturate(equatorBright + minEquatorMix));
}

half3 SetColorGray(half3 baseCol,half grayFactor)
{
	fixed3 grayColor = baseCol;
	grayColor.rgb = dot(grayColor.rgb, half3(0.22, 0.707, 0.071));
	fixed3 texCol = lerp(baseCol,grayColor,grayFactor);
	return texCol;
}

half3 SetColorGrayZeroOrOne(half3 baseCol, half grayFactor)
{
	if (grayFactor < 0.5)
		return baseCol;
	else
	{
		baseCol.rgb = dot(baseCol.rgb, half3(0.22, 0.707, 0.071));
		return baseCol;
	}
}

half3 DoLogicColorChanage(half3 baseCol, half grayFactor, half addColorIntensity, fixed3 color)
{
	fixed3 grayColor = baseCol;
	grayColor.rgb = dot(grayColor.rgb, half3(0.22, 0.707, 0.071));
	fixed3 texCol = lerp(baseCol, grayColor, grayFactor);
	texCol = texCol + color * addColorIntensity;
	return texCol;
}

half3 DoLogicColorChanageZeroOrOne(half3 baseCol, half grayFactor, half addColorIntensity, fixed3 color)
{
	baseCol = SetColorGrayZeroOrOne(baseCol, grayFactor);

	baseCol += color * addColorIntensity;
	return baseCol;
}

//perlin
float2 hash21(float2 p) 
{
    float h = dot(p, float2(127.1, 311.7));
    return -1.0 + 2.0 * frac(sin(h) * 43758.5453123);
}
float2 hash22(float2 p) 
{
    p = float2(dot(p,float2(127.1,311.7)), dot(p,float2(269.5, 183.3)));
    return -1.0 + 2.0 * frac(sin(p) * 43758.5453123);
}
float perlin_noise(float2 p) 
{				
    float2 pi = floor(p);
    float2 pf = p - pi;
    float2 w = pf * pf * (3.0 - 2.0 * pf);
    return lerp(lerp(dot(hash22(pi + float2(0.0, 0.0)), pf - float2(0.0, 0.0)), dot(hash22(pi + float2(1.0, 0.0)), pf - float2(1.0, 0.0)), w.x),
                lerp(dot(hash22(pi + float2(0.0, 1.0)), pf - float2(0.0, 1.0)), dot(hash22(pi + float2(1.0, 1.0)), pf - float2(1.0, 1.0)), w.x), w.y);
}
#endif