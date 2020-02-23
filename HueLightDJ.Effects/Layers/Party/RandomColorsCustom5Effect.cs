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
  [HueEffect(Name = "Jesse Custom 5", Group = "Party", HasColorPicker = false)]
  public class RandomColorsCustom5Effect : CustomBaseEffect, IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var center = EffectSettings.LocationCenter;
      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(center.X, center.Y)).ToList();
      
      Colors = RGBColorPicker.DiscoColors;
            
      while (!cancellationToken.IsCancellationRequested)
      {
        var rndColor = GetNext();

        for (var i = 0; i < layer.Count; i++)
        {
          foreach (var light in orderedByAngle)
          {
            if (light.State.RGBColor.ToHex() != rndColor.ToHex())
            {
              light.SetState(cancellationToken, rndColor, waitTime() / 2, 1);
              break;
            }
          }

          await Task.Delay(waitTime(), cancellationToken);
        }
      }
    }
  }
}
