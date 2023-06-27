# TagCloudGenerator
Generate a tagcloud bitmap image
### Sample
<table>
  <tr>
    <td><img width=1000 align="center" src="https://github.com/ZjzMisaka/AnimeReport/blob/main/tags.bmp"/></td>
    <td><img width=1000 align="center" src="https://github.com/ZjzMisaka/AnimeReport/blob/main/companies.bmp"/></td>
  </tr>
</table>

### Getting started
``` csharp
// Set the string dictionary used to output tagcloud
// Value is the weight of the string, which can generally be set to the number of occurrences of the string
Dictionary<string, float> tagDic = new Dictionary<string, float>();
tagDic["J.C.STAFF"] = 32;
```
- If you don't need to specify special settings for individual characters
``` csharp
TagCloudOption tagCloudOption = new TagCloudOption();
// Set tagCloudOption
// tagCloudOption.Xxx = xxx;

Bitmap bmp = new TagCloud(tagDic, tagCloudOption).Get();
```
- If you want to specify special settings for individual characters
``` csharp
TagCloudOption tagCloudOption = new TagCloudOption();
// Set tagCloudOption
// tagCloudOption.Xxx = xxx;

Dictionary<string, TagOption> tagOptionDic = new Dictionary<string, TagOption>();
// Set option for single text
// tagOptionDic["J.C.STAFF"] = new TagOption() { Rotate = new Rotate(0) };

Bitmap bmp = new TagCloud(tagDic, tagCloudOption, tagOptionDic).Get();
```
### TagCloudOption
|Option|Introduce|
|--|--|
|RotateType RotateType|Initial font rotate angle<br>RotateType.FromList \| RotateType.Random|
|float RandomRotateFrom|If RotateType.Random, take random values from RandomRotateFrom and RandomRotateTo|
|float RandomRotateTo|If RotateType.Random, take random values from RandomRotateFrom and RandomRotateTo|
|List\<int> RotateList|If RotateType.FromList, take rotate from RotateList|
|int TagSpacing|Minimum pixel spacing between adjacent tags|
|int HorizontalOuterMargin|Minimum horizontal outer margin|
|int VerticalOuterMargin|Minimum vertical outer margin|
|List\<Color> FontColorList|Set possible font colors for bitmap tags|
|ColorOption BackgroundColor|Set background color for bitmap tags|
|FontFamily FontFamily|Set font family for bitmap tags|
|(float, float) FontSizeRange|Set minimum and maximum font sizes|
|double AngleStep|Set angle step for spiral algorithm|
|double RadiusStep|Set radius step for spiral algorithm|
|double AngleDecreaseFactor|Angle step decreasing factor|
|double RadiusDecreaseFactor|Radius step decreasing factor|
|bool IsRandomInitAngle|Set if is random init angle or not for spiral algorithm|
|int HorizontalCanvasGrowthStep|Horizontal canvas growth step|
|int VerticalCanvasGrowthStep|Vertical canvas growth step|
|ImgSize OutputSize|Output bitmap image size|
|ImgSize InitSize|Init bitmap image size, the canvas will grow when the canvas is filled|
|string MaskPath|If you need to generate a tag cloud through masking, set the path for the masking image|
|bool ShowMask|Set whether to display mask image in generated bitmap image|
|bool StretchMask|Set whether to stretch the mask to match the canvas|

### TagOption
|Option|Introduce|
|--|--|
|Rotate Rotate|Rotate of tag|
|FontColor FontColor|FontColor of tag|
|FontFamily FontFamily|FontFamily of tag|