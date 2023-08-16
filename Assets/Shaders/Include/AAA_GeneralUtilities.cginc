#ifndef __AAA_GENERALUTILITIES_CGINC
#define __AAA_GENERALUTILITIES_CGINC

// ===== Common Functions =====

/*
Description: Gets a depth value as a float4 colour. The depth value returned is converted to
a 0-1 range with 1 being closest and 0 being farthest.
Parameters: The depth texture to be sampled, and UV coordinates.
Creator: Charlotte C. Brown
Creation Date: Oct. 7th 2016
Extra Notes: *you may have to flip the V coord depending on depth texture source.
*/
float ColourAtPoint_Depth(sampler2D aDepthTexture, float aU, float aV)
{
    float depth = 1.0 - Linear01Depth(tex2D(aDepthTexture, float2(aU, aV)));

    return float4(depth, depth, depth, depth);
}

/*
Description: Gets a colour from a given texture. Wrapper for tex2D().
Parameters: The texture to sample, and the UV coordinates.
Creator: Charlotte C. Brown
*/
float4 ColourAtPoint_Texture(sampler2D aTexture, float aU, float aV)
{
    return tex2D(aTexture, float2(aU, aV));
}

// ===== Value Comparison Functions =====

/*
Description: Gets the lowest component of the given vector
Parameters: The vector to check
Creator: Charlotte C. Brown
*/
float GetLowestValue(float3 aVector)
{
    // Get the lower value between X and Y.
    float lowestXY = lerp(aVector.x, aVector.y, step(aVector.x, aVector.y));

    // Return the lower value between the previous two and Z.
    return lerp(lowestXY, aVector.z, step(lowestXY, aVector.z));
}

/*
Description: Gets the highest component of the given vector
Parameters: The vector to check
Creator: Charlotte C. Brown
*/
float GetHighestValue(float3 aVector)
{
    // Get the higher value between X and Y.
    float highestXY = lerp(aVector.x, aVector.y, step(aVector.y, aVector.x));

    // Return the higher value between the previous two and Z.
    return lerp(highestXY, aVector.z, step(aVector.z, highestXY));
}

/*
Description: Returns the similarity percent between two values (1 if identical, 0 if outside the tolerance range)
Parameters: The two values and the tolerance amount to consider values similar
Creator: Charlotte C. Brown
*/
float Float_IsSimilar(float aValue1, float aValue2, float aTolerance)
{
    // Get the difference between the Values.
    float difference12 = aValue1 - aValue2;
    float difference21 = aValue2 - aValue1;
    float delta = lerp(difference12, difference21, step(difference12, difference21));

    // Return if the difference is within the given tolerance range.
    return saturate((aTolerance - delta) / aTolerance);
}

/*
Description: Returns the similarity percent between two colour saturations (1 if identical, 0 if outside the tolerance range)
Parameters: The two colours and the tolerance amount to consider saturations similar
Creator: Charlotte C. Brown
*/
float Colour_IsSimilar_Saturation(float3 aColour1, float3 aColour2, float aTolerance)
{
    // Get the saturation amount of the first colour.
    float colour1Lowest = GetLowestValue(aColour1);
    float colour1Highest = GetHighestValue(aColour1);
    float colour1Saturation = colour1Highest - colour1Lowest;

    // Get the saturation amount of the second colour.
    float colour2Lowest = GetLowestValue(aColour2);
    float colour2Highest = GetHighestValue(aColour2);
    float colour2Saturation = colour2Highest - colour2Lowest;

    // Return if the saturations are similar.
    return Float_IsSimilar(colour1Saturation, colour2Saturation, aTolerance);
}

/*
Description: Returns the similarity percent between two colour brightnesses (1 if identical, 0 if outside the tolerance range)
Parameters: The two colours and the tolerance amount to consider brightnesses similar
Creator: Charlotte C. Brown
*/
float Colour_IsSimilar_Brightness(float3 aColour1, float3 aColour2, float aTolerance)
{
    // Get the average brightness of the first colour.
    float colour1Brightness = (aColour1.x + aColour1.y + aColour1.z) / 3.0;

    // Get the average brightness of the second colour.
    float colour2Brightness = (aColour2.x + aColour2.y + aColour2.z) / 3.0;

    // Return if the brighnesses are similar.
    return Float_IsSimilar(colour1Brightness, colour2Brightness, aTolerance);
}

/*
Description: Returns the similarity percent between two colour hues (1 if identical, 0 if outside the tolerance range)
Parameters: The two colours and the tolerance amount to consider hues similar
Creator: Charlotte C. Brown
*/
float Colour_IsSimilar_Hue(float3 aColour1, float3 aColour2, float aTolerance)
{
    // Step directions will be used to track how much the hue has shifted between the two colours.
    // Get the step direction between the components of the first colour.
    float colour1StepRG = aColour1.r - aColour1.g;
    float colour1StepGB = aColour1.g - aColour1.b;
    float colour1StepBR = aColour1.b - aColour1.r;

    // Get the step direction between the components of the second colour.
    float colour2StepRG = aColour2.r - aColour2.g;
    float colour2StepGB = aColour2.g - aColour2.b;
    float colour2StepBR = aColour2.b - aColour2.r;

    // How similar the colours are as a percentage from 0-1.
    float similarityPercent = 0.0;

    // Here is where we'll look at the step directions to determine how much the hue has changed.
    // Are the step deltas within an exceptable range in order to be considered as having "not changed"?
    similarityPercent += Float_IsSimilar(colour1StepRG, colour2StepRG, aTolerance);
    similarityPercent += Float_IsSimilar(colour1StepGB, colour2StepGB, aTolerance);
    similarityPercent += Float_IsSimilar(colour1StepBR, colour2StepBR, aTolerance);
    similarityPercent /= 3.0;

    // Return whether or not the hues are considered similar or different.
    return similarityPercent;
}

/*
Description: Returns the similarity percent between two colour combined HSB values (1 if identical, 0 if outside the tolerance range)
Parameters: The two colours and the tolerance amount to consider combined HSB values similar
Creator: Charlotte C. Brown
*/
float Colour_IsSimilar_HSB(float3 aColour1, float3 aColour2, float aTolerance)
{
    // How similar the colours are as a percentage from 0-1.
    float similarityPercent = 0.0;

    // Get if the two given colours have similar hues, saturations, and brightnesses.
    similarityPercent += Colour_IsSimilar_Hue(aColour1, aColour2, aTolerance);
    similarityPercent += Colour_IsSimilar_Saturation(aColour1, aColour2, aTolerance);
    similarityPercent += Colour_IsSimilar_Brightness(aColour1, aColour2, aTolerance);
    similarityPercent /= 3.0;

    // Return how similar the colours are to each other.
    return similarityPercent;
}

/*
Description: Returns a desaturated version of the given colour.
Parameters: The colour to desaturate.
Creator: Charlotte C. Brown
*/
float3 Colour_Desaturate(float3 aColour)
{
	// The average of all components.
	float average = aColour.r + aColour.g + aColour.b;
	average /= 3.0;

	// Return the desaturated colour.
	return float3(average, average, average);
}

/*
Description: Returns a colour that is of the same brightness and hue as the given colour, but its saturation increased.
Parameters: The colour to increase saturation and the amount to increase it by.
Creator: Charlotte C. Brown
*/
float3 Colour_Saturate(float3 aColour, float aAmount)
{
	// The average of all components.
	float average = aColour.r + aColour.g + aColour.b;
	average /= 3.0;

	// Calculate how far each component is from the average.
	float deltaRed = aColour.r - average;
	float deltaGreen = aColour.g - average;
	float deltaBlue = aColour.b - average;

	// Increase these deltas by the given amount.
	deltaRed *= aAmount;
	deltaGreen *= aAmount;
	deltaBlue *= aAmount;

	// Create the new colour and make sure its values are within a 0-1 range.
	float3 saturatedColour = float3(average + deltaRed, average + deltaGreen, average + deltaBlue);
	saturatedColour = saturate(saturatedColour);

	// Return the colour.
	return saturatedColour;
}

/* Simple hashing function found at http://stackoverflow.com/questions/12964279/whats-the-origin-of-this-glsl-rand-one-liner
*/
float HashCoord(float2 aCoord)
{
    return frac(sin(dot(aCoord, float2(12.9898, 78.233))) * 43758.5453);
}

#endif