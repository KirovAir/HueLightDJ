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
  [HueEffect(Name = "Jesse Custom 4", Group = "Party", HasColorPicker = false)]
  public class RandomColorsCustom4Effect : CustomBaseEffect, IHueEffect
  {
    public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      var center = EffectSettings.LocationCenter;
      var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(center.X, center.Y)).ToList();

      var baseColors = new List<RGBColor>();
      var baseIndex = 0;
      baseColors.Add(RGBColorPicker.Orange);
      baseColors.Add(RGBColorPicker.Pink);
      baseColors.Add(RGBColorPicker.Yellow);
      baseColors.Add(new RGBColor(255, 0, 0)); // R
      baseColors.Add(new RGBColor(0, 255, 0)); // G
      baseColors.Add(new RGBColor(0, 0, 255)); // B
      
      while (!cancellationToken.IsCancellationRequested)
      {
        Colors.Clear();
        var color1 = RGBColor.Random();
        var color2 = GetNext(baseColors, ref baseIndex);
        
        Colors.Add(color1);
        for (var i = 0; i < 7; i++)
        {
          //if (i % 3 == 0)
          //{
          //  Colors.Add(color1);
          //  continue;
          //}

          Colors.Add(color2);
        }

        for (var i = 0; i < layer.Count; i++)
        {
          foreach (var light in orderedByAngle)
          {
            var rndColor = GetNext();

            var copyColor = new RGBColor(rndColor.ToHex());
            if (light.State.RGBColor.ToHex() != rndColor.ToHex())
            {
              light.SetState(cancellationToken, copyColor, waitTime() / 2, 1);
            }
          }
          await Task.Delay(waitTime(), cancellationToken);

        }
      }
    }
  }
}
