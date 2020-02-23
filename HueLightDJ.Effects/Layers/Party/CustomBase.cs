using System;
using System.Collections.Generic;
using Q42.HueApi.ColorConverters;

namespace HueLightDJ.Effects
{
  public class CustomBaseEffect
  {
    public readonly Random Random = new Random();
    public int Index = 0;
    public List<RGBColor> Colors = new List<RGBColor>();

    public double RandomBrightness()
    {
      var brightness = (double)Random.Next(85, 100) / 100;
      return brightness;
    }

    public RGBColor GetNext()
    {
      return GetNext(Colors, ref Index);
    }

    public T GetNext<T>(List<T> list, ref int index)
    {
      index++;

      if (index >= list.Count)
        index = 0;
      return list[index];
    }

    public void Shuffle<T>(IList<T> list)
    {
      var n = list.Count;
      while (n > 1)
      {
        n--;
        var k = Random.Next(n + 1);
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }
  }

  public static class RGBColorPicker
  {
    public static RGBColor Pink { get; set; } = new RGBColor("#ff00e1");
    public static RGBColor Orange { get; set; } = new RGBColor("#ff6a00");
    public static RGBColor Aqua { get; set; } = new RGBColor("#01cdfe");
    public static RGBColor Purple { get; set; } = new RGBColor("#9400D3");


    public static List<RGBColor> DiscoColors { get; } = new List<RGBColor>
    {
      Orange,
      new RGBColor(255, 0, 0), // R
      Pink,
      new RGBColor(0, 200, 0), // G
      new RGBColor(0, 0, 255), // B
      Aqua,
      Purple
    };
  }
}
