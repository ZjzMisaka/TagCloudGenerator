# TagCloudGenerator
Generate a tagcloud image
### Getting started
``` csharp
TagCloudOption tagCloudOption = new TagCloudOption();
// Set tagCloudOption
// tagCloudOption.Xxx = xxx;

Bitmap bmp = new TagCloud(tagDic, tagCloudOption).Get();
```
``` csharp
TagCloudOption tagCloudOption = new TagCloudOption();
// Set tagCloudOption
// tagCloudOption.Xxx = xxx;

Dictionary<string, TagOption> tagOptionDic = new Dictionary<string, TagOption>();
// Set option for single text
// tagOptionDic["J.C.STAFF"] = new TagOption() { Rotate = 0 };

Bitmap bmp = new TagCloud(tagDic, tagCloudOption, tagOptionDic).Get();
```
### TagCloudOption
``` csharp
// Initial font rotate angle
// RotateType.FromList | RotateType.Random
RotateType RotateType
// If RotateType.Random, take random values from RandomRotateFrom and RandomRotateTo
float RandomRotateFrom
float RandomRotateTo
// If RotateType.FromList, take rotate from RotateList
List<int>? RotateList
// Minimum pixel spacing between adjacent tags
int TagSpacing
// Minimum horizontal outer margin
int HorizontalOuterMargin
// Minimum vertical outer margin
int VerticalOuterMargin
// Set possible font colors for bitmap tags
List<Color>? FontColorList
// Set background color for bitmap tags
Color? BackgroundColor
// Set font family for bitmap tags
FontFamily FontFamily
// Set minimum and maximum font sizes
(float, float) FontSizeRange
// Set angle step for spiral algorithm
double AngleStep
// Set radius step for spiral algorithm
double RadiusStep
// Angle step decreasing factor
double AngleDecreaseFactor
// Radius step decreasing factor
double RadiusDecreaseFactor
// Set if is random init angle or not for spiral algorithm
bool IsRandomInitAngle
// Horizontal canvas growth step
int HorizontalCanvasGrowthStep
// Vertical canvas growth step
int VerticalCanvasGrowthStep
// Output bitmap image size
ImgSize? OutputSize
// Init bitmap image size, the canvas will grow when the canvas is filled
ImgSize? InitSize
// If you need to generate a tag cloud through masking, set the path for the masking image
string? MaskPath
// Set whether to display mask image in generated bitmap image
bool ShowMask
// Set whether to stretch the mask to match the canvas
bool StretchMask
```