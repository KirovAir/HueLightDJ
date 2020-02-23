using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Jesse Custom 1", Group = "Party", HasColorPicker = false)]
  public class RandomColorsCustom1Effect : CustomBaseEffect, IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      Colors.Add(new RGBColor(255, 0, 0)); // Red
      Colors.Add(RGBColor.Random());
      Colors.Add(new RGBColor(0, 0, 255)); // Blue
      Colors.Add(RGBColor.Random());
      Colors.Add(new RGBColor(0, 255, 0)); // Green
      Colors.Add(RGBColor.Random());

      while (!cancellationToken.IsCancellationRequested)
      {
        foreach (var light in layer)
        {
          //var rndColor = RGBColor.Random(_random);
          //var rndColor = colors[_random.Next(colors.Count)];
          var rndColor = GetNext();
          var copyColor = new RGBColor(rndColor.ToHex());
          light.SetState(cancellationToken, copyColor, waitTime() / 2, 1);

        }
        await Task.Delay(waitTime() * 2, cancellationToken);

      }
    }
  }
}
