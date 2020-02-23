using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Jesse Custom 4", Group = "Party", HasColorPicker = false)]
  public class RandomColorsCustom4Effect : CustomBaseEffect, IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var center = EffectSettings.LocationCenter;
      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(center.X, center.Y)).ToList();

      var collections = new List<List<EntertainmentLight>>();
      collections.Add(orderedByAngle);
      collections.Add(layer);

      Colors = RGBColorPicker.DiscoColors;

      while (!cancellationToken.IsCancellationRequested)
      {
        for (var index = 0; index < collections.Count; index++)
        {
          var lights = collections[index];
          var rndColor = GetNext();

          for (var i = 0; i < Math.Ceiling((double) layer.Count / 2); i++)
          {
            var brightness = RandomBrightness();
            foreach (var light in lights)
            {
              if (light.State.RGBColor.ToHex() != rndColor.ToHex())
              {
                light.SetState(cancellationToken, rndColor, waitTime() / 2, brightness, waitTime()/2);
                break;
              }
            }

            lights.Reverse();
            foreach (var light in lights)
            {
              if (light.State.RGBColor.ToHex() != rndColor.ToHex())
              {
                light.SetState(cancellationToken, rndColor, waitTime() / 2, brightness, waitTime() / 2);
                break;
              }
            }

            await Task.Delay(waitTime(), cancellationToken);
          }
        }
      }
    }
  }
}
