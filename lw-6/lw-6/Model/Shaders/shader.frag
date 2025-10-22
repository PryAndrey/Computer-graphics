#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec3 Color;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform vec3 cameraPos;

uniform int figureCount;
uniform vec3 figureCentres[10];
uniform float figureSizes[10];

float sphereDistance(vec3 p, vec3 center, float radius)
{
    return length(p - center) - radius;
}

float distanceToNearestObject(vec3 p)
{
    float minDistance = 10000.0;
    for (int i = 0; i < figureCount; i++)
    {
        float distance = sphereDistance(p, figureCentres[i], figureSizes[i]);
        minDistance = min(minDistance, distance);
    }
    return minDistance;
}

bool isInShadow(vec3 startPoint, vec3 direction)
{
    float step = 0.2;
    float maxDistance = length(lightPos - startPoint);
    float minDistance = 0.001;
    float softShadow = 1.0;

    for(int i = 0; i < 100; i++)
    {
        vec3 p = startPoint + direction * step;
        float distance = distanceToNearestObject(p);
        
        if (distance < minDistance)
        {
            return true;
        }
        
        softShadow = min(softShadow, 0.5 * distance / (step * 0.1));
        step += distance;
        
        if (step >= maxDistance) 
        {
            break;
        }
    }
    return false;
}

void main()
{
    vec3 result = 0.3 * lightColor;    
    
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);

    vec3 offsetByNormal = norm * 0.001;
    bool shadowed = isInShadow(FragPos + offsetByNormal, lightDir);
    
    if(!shadowed)
    {
        // Diffuse
        float diff = max(dot(norm, lightDir), 0.0);
        result += diff * lightColor;
        
        // Specular
        vec3 viewDir = normalize(cameraPos - FragPos);
        vec3 reflectDir = reflect(-lightDir, norm);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
        result += 0.5 * spec * lightColor;
    }
    
    result *= Color;
    FragColor = vec4(result, 1.0);
}