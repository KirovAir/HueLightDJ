using System;
using System.Threading;
using System.Threading.Tasks;
using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Jesse Custom 2", Group = "Party", HasColorPicker = false)]
  public class RandomColorsCustom2Effect : CustomBaseEffect, IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      Colors.Add(RGBColorPicker.Orange);
      Colors.Add(RGBColorPicker.Pink);
      Colors.Add(RGBColorPicker.Yellow);
      Colors.Add(new RGBColor(255, 0, 0)); // R
      Colors.Add(new RGBColor(0, 255, 0)); // G
      Colors.Add(new RGBColor(0, 0, 255)); // B
      Colors.Add(RGBColor.Random());

      while (!cancellationToken.IsCancellationRequested)
      {
        var x = 0;
        var rndColor = GetNext();
        foreach (var light in layer)
        {
          x++;
          if (x > 2)
          {
            x = 0;
            rndColor = GetNext();
          }
          //var rndColor = RGBColor.Random(_random);
          //var rndColor = colors[_random.Next(colors.Count)];
          var copyColor = new RGBColor(rndColor.ToHex());
          light.SetState(cancellationToken, copyColor, waitTime() / 2, 1);

        }
        await Task.Delay(waitTime() * 2, cancellationToken);
      }
    }
  }
}
