# TagCloudGenerator
<img src="https://www.nuget.org/Content/gallery/img/logo-header.svg?sanitize=true" height="30px">

TagCloudGenerator is a C# library that allows you to generate a tag cloud bitmap image from a given set of strings and their corresponding weights. You can customize the appearance of the tag cloud by specifying various options, such as font size, font color, background color, and more.

### Download
TagCloudGenerator is available as [Nuget Package](https://www.nuget.org/packages/TagCloudGenerator/) now.

## Sample Images

Below are some sample images showing the output of the TagCloudGenerator:

<table>
  <tr>
    <td><img width=1000 align="center" src="https://github.com/ZjzMisaka/AnimeReport/blob/main/tags.bmp"/></td>
    <td><img width=1000 align="center" src="https://github.com/ZjzMisaka/AnimeReport/blob/main/companies.bmp"/></td>
  </tr>
</table>

## Getting Started

You can use the TagCloudGenerator library by following these steps:

1. Create a dictionary of strings and their corresponding weights. The weights can be set to the number of occurrences of each string.

   ```csharp
   Dictionary<string, float> tagDic = new Dictionary<string, float>();
   tagDic["J.C.STAFF"] = 32;
   ```

2. Create a `TagCloudOption` object, and set its properties as needed.

   ```csharp
   TagCloudOption tagCloudOption = new TagCloudOption();
   // Set tagCloudOption properties
   // tagCloudOption.Xxx = xxx;
   ```

3. If you want to specify special settings for individual characters, create a dictionary of `TagOption` objects.

   ```csharp
   Dictionary<string, TagOption> tagOptionDic = new Dictionary<string, TagOption>();
   // Set option for single text
   // tagOptionDic["J.C.STAFF"] = new TagOption() { Rotate = new Rotate(0) };
   ```

4. Create a `TagCloud` object using the dictionaries and the `TagCloudOption` object, and then call the `Get()` method to generate the bitmap image.

   ```csharp
   Bitmap bmp = new TagCloud(tagDic, tagCloudOption, tagOptionDic).Get();
   ```

   If you don't need to specify special settings for individual characters, you can omit the `tagOptionDic` parameter:

   ```csharp
   Bitmap bmp = new TagCloud(tagDic, tagCloudOption).Get();
   ```

## TagCloudOption Properties

The `TagCloudOption` object allows you to customize various aspects of the tag cloud. Below is a list of its properties and their descriptions:

|Property|Description|
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
|int Accuracy|Accuracy of TagSpacing check, default = 1, if =0 then check all pixel. It is used to increase operating efficiency|

## TagOption Properties

The `TagOption` object allows you to specify special settings for individual charactersin the tag cloud. Below is a list of its properties and their descriptions:

|Property|Description|
|--|--|
|Rotate Rotate|Rotate of tag|
|FontColor FontColor|FontColor of tag|
|FontFamily FontFamily|FontFamily of tag|

## License
This project is licensed under the [OMSPL](https://github.com/ZjzMisaka/TagCloudGenerator/blob/main/LICENSE).