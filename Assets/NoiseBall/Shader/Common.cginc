#include "SimplexNoiseGrad3D.cginc"

float _Radius;
float _NoiseAmplitude;
float _NoiseFrequency;
float3 _NoiseOffset;

float3 displace(float3 p)
{
    float3 q = normalize(cross(p, float3(0, 1, 0)) + float3(0, 1e-5, 0));
    float3 r = cross(p, q);
    float3 n = snoise_grad(p * _NoiseFrequency + _NoiseOffset) * _NoiseAmplitude;
    return p * (_Radius + n.x) + q * n.y + r * n.z;
}
