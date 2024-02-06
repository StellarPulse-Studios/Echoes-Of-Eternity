#ifndef MINIMAP_HELPER_INCLUDED
#define MINIMAP_HELPER_INCLUDED

float3 RGB2HSV(float3 value)
{
	float maxValue = max(max(value.r, value.g), value.b);
	float minValue = min(min(value.r, value.g), value.b);
	float delta = maxValue - minValue;
	float v = maxValue;
	float s = (maxValue > 0.0) ? (delta / maxValue) : 0.0;
	float h = 0.0;

	if (delta > 0.0)
	{
		if (maxValue == value.r) h = 60.0 * (value.g - value.b) / delta;
		else if (maxValue == value.g) h = 60.0 * (2.0 + (value.b - value.r) / delta);
		else if (maxValue == value.b) h = 60.0 * (4.0 + (value.r - value.g) / delta);

		if (h < 0.0) h += 360.0;
	}

	return float3(h, s, v);
}

float3 HSV2RGB(float3 hsv)
{
	float hue = hsv.x;
	float saturation = hsv.y;
	float value = hsv.z;

	float chroma = value * saturation;
	float hueDash = hue / 60.0;
	float x = chroma * (1.0 - abs(fmod(hueDash, 2.0) - 1.0));

	float3 rgb = 0.0;

	if (hueDash >= 0 && hueDash < 1)
		rgb = float3(chroma, x, 0);
	else if (hueDash >= 1 && hueDash < 2)
		rgb = float3(x, chroma, 0);
	else if (hueDash >= 2 && hueDash < 3)
		rgb = float3(0, chroma, x);
	else if (hueDash >= 3 && hueDash < 4)
		rgb = float3(0, x, chroma);
	else if (hueDash >= 4 && hueDash < 5)
		rgb = float3(x, 0, chroma);
	else if (hueDash >= 5 && hueDash < 6)
		rgb = float3(chroma, 0, x);

	float m = value - chroma;
	rgb += float3(m, m, m);

	return rgb;
}

float2 RotateUV_Degree(float2 uv, float2 center, float rotation)
{
	rotation = rotation * (3.1415926f / 180.0f);
	uv -= center;
	float s = sin(rotation);
	float c = cos(rotation);
	float2x2 mat = float2x2(c, -s, s, c);
	mat *= 0.5f;
	mat += 0.5f;
	mat = mat * 2.0f - 1.0f;
	uv.xy = mul(uv.xy, mat);
	uv += center;
	return uv;
}

float2 RotateUV_Radian(float2 uv, float2 center, float rotation)
{
	uv -= center;
	float s = sin(rotation);
	float c = cos(rotation);
	float2x2 mat = float2x2(c, -s, s, c);
	mat *= 0.5f;
	mat += 0.5f;
	mat = mat * 2.0f - 1.0f;
	uv.xy = mul(uv.xy, mat);
	uv += center;
	return uv;
}

#endif