using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;

namespace HueLightDJ.Effects
{
  public class CustomBaseEffect
  {
    public readonly Random Random = new Random();
    public int Index = 0;
    public List<RGBColor> Colors = new List<RGBColor>();


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
  }

  public static class RGBColorPicker
  {
    public static RGBColor Pink { get; set; } = new RGBColor("#ff00e1");
    public static RGBColor Yellow { get; set; } = new RGBColor("#ff00e1");
    public static RGBColor Orange { get; set; } = new RGBColor("#ff6a00");

  }
}
