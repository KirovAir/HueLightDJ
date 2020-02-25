using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Jesse Custom 3", Group = "Party", HasColorPicker = false)]
  public class RandomColorsCustom3Effect : CustomBaseEffect, IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var center = EffectSettings.LocationCenter;
      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(center.X, center.Y)).ToList();

      var baseColors = RGBColorPicker.DiscoColors;
      var baseIndex = 0;

      while (!cancellationToken.IsCancellationRequested)
      {
        Colors.Clear();
        var color1 = RGBColor.Random();
        var color2 = GetNext(baseColors, ref baseIndex);

        for (var i = 0; i < ChangeAmount; i++)
          Colors.Add(color1);

        for (var i = 0; i < layer.Count; i++)
        {
          Colors.Add(color2);
        }

        var brightness = RandomBrightness();

        for (var i = 0; i < layer.Count; i++)
        {
          var bri = HiLowBrightness();
          foreach (var light in orderedByAngle)
          {
            var rndColor = GetNext();
            
            if (light.State.RGBColor.ToHex() != rndColor.ToHex())
            {
              light.SetState(cancellationToken, rndColor, waitTime() / 2, brightness, waitTime() / 2);
            }
            else
            {
              light.SetBrightness(cancellationToken, bri, waitTime() / 2);
            }
          }
          await Task.Delay(waitTime(), cancellationToken);

        }

        Index++;
      }
    }
  }
}
