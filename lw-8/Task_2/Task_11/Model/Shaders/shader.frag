#version 330 core
out vec4 FragColor;

uniform float time;
uniform vec2 resolution;

uniform vec3 cameraPos;
uniform vec3 cameraFront;
uniform vec3 cameraUp;

struct HitInfo {
    float distance;
    vec3 position;
    vec3 normal;
    int material;
};

#define MATERIAL_GROUND 0
#define MATERIAL_SPHERE 1
#define MATERIAL_METAL 2

float sdSphere(vec3 p, float r) {
    return length(p) - r;
}

float sdPlane(vec3 p, vec3 n, float h) {
    return dot(p, n) + h;
}

HitInfo scene(vec3 p) {
    HitInfo hit;
    hit.distance = 1e10;
    hit.material = MATERIAL_GROUND;
    
    float ground = sdPlane(p, vec3(0, 1, 0), 0.0);
    if (ground < hit.distance) {
        hit.distance = ground;
        hit.material = MATERIAL_GROUND;
    }
    
    vec3 spherePos = vec3(sin(time * 0.5) * 3.0, 0.5, cos(time * 0.5) * 3.0 - 5.0);
    float sphere = sdSphere(p - spherePos, 0.3);
    if (sphere < hit.distance) {
        hit.distance = sphere;
        hit.material = MATERIAL_SPHERE;
    }
    
    vec3 cubePos = vec3(0.0, 0.5, -5.0);
    vec3 q = abs(p - cubePos) - vec3(0.5);
    float cube = length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
    if (cube < hit.distance) {
        hit.distance = cube;
        hit.material = MATERIAL_METAL;
    }
    
    return hit;
}

vec3 calcNormal(vec3 p) {
    const float eps = 0.001;
    vec2 e = vec2(eps, 0);
    return normalize(vec3(
        scene(p + e.xyy).distance - scene(p - e.xyy).distance,
        scene(p + e.yxy).distance - scene(p - e.yxy).distance,
        scene(p + e.yyx).distance - scene(p - e.yyx).distance
    ));
}

vec3 getMaterialColor(int material, vec3 pos) {
    if (material == MATERIAL_GROUND) {
        
        float tile = mod(floor(pos.x) + floor(pos.z), 2.0);
        return mix(vec3(0.8), vec3(0.3), tile);
    } else if (material == MATERIAL_SPHERE) {
        return vec3(0.2, 0.5, 0.8);
    } else if (material == MATERIAL_METAL) {
        return vec3(0.8, 0.8, 0.9);
    }
    return vec3(1.0, 0.0, 1.0); 
}

HitInfo raymarch(vec3 ro, vec3 rd) {
    HitInfo hit;
    hit.distance = 0.0;
    
    for (int i = 0; i < 150; i++) { 
        vec3 p = ro + rd * hit.distance;
        HitInfo info = scene(p);
        
        float threshold = max(0.001, 0.0005 * hit.distance);
        if (info.distance < threshold) {
            hit.position = p;
            hit.normal = calcNormal(p);
            hit.material = info.material;
            return hit;
        }
        
        if (hit.distance > 20.0) break;
        
        hit.distance += info.distance;
    }
    
    hit.distance = 1e10;
    return hit;
}

void main() {
    vec2 uv = (gl_FragCoord.xy - 0.5 * resolution.xy) / resolution.y;
    vec3 ro = cameraPos;
    
    vec3 target = vec3(0.0, 0.5, -5.0);
    vec3 forward = normalize(target - ro);
    vec3 right = normalize(cross(forward, cameraUp));
    vec3 up = cross(right, forward);
    
    vec3 rd = normalize(forward + right * uv.x + up * uv.y);
    
    HitInfo hit = raymarch(ro, rd);
    
    if (hit.distance < 200.0) {
        vec3 normal = calcNormal(hit.position);
        
       // vec3 lightDir = normalize(vec3(1.0, 1.0, 0.5));
       // float diff = max(dot(normal, lightDir), 0.0);
        
        vec3 color = getMaterialColor(hit.material, hit.position);
       // color *= diff + 0.2; 
        
        if (hit.material == MATERIAL_METAL) {
            vec3 reflectDir = reflect(rd, normal);
            HitInfo reflHit = raymarch(hit.position + normal * 0.1, reflectDir);
            if (reflHit.distance < 200.0) {
                vec3 reflColor = getMaterialColor(reflHit.material, reflHit.position);
                color = mix(color, reflColor, 0.8);
            } else {
                color = vec3(0.5, 0.6, 1.0);
            }
        }
        
        FragColor = vec4(color, 1.0);
    } else {
        vec3 sky = vec3(0.5, 0.7, 1.0);
        FragColor = vec4(sky, 1.0);
    }
}