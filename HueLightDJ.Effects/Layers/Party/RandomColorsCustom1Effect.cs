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
      while (!cancellationToken.IsCancellationRequested)
      {
        Colors.Clear();
        Colors.AddRange(RGBColorPicker.DiscoColors);
        Colors.Add(RGBColor.Random());
        Colors.Add(RGBColor.Random());
        Colors.Add(RGBColor.Random());
        Shuffle(Colors);

        for (var i = 0; i < 5; i++)
        {
          var brightness = RandomBrightness();
          foreach (var light in layer.OrderBy(c => Guid.NewGuid()).Take(2))
          {
            //if (Random.Next(4) != 1)
            //{
            //  continue;
            //}
            var rndColor = GetNext();
            var copyColor = new RGBColor(rndColor.ToHex());
            light.SetState(cancellationToken, copyColor, waitTime() / 2, brightness, waitTime() / 2);
          }

          await Task.Delay(waitTime(), cancellationToken);
        }
      }
    }
  }
}
