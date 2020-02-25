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
            var dDisco = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                dDisco = !dDisco;
                Colors.Clear();
                Colors.AddRange(dDisco || ChangeAmount < 2 ? RGBColorPicker.DoubleDisco : RGBColorPicker.DiscoColors);
                if (ChangeAmount == 1)
                {
                    Colors.RemoveAt(Colors.Count - 1);
                }

                for (var i = 0; i < 4; i++)
                {
                    var bri = HiLowBrightness();
                    var x = 0;
                    var rndColor = GetNext();
                    foreach (var light in layer)
                    {
                        x++;
                        if (x > 4)
                        {
                            x = 0;
                            rndColor = GetNext();
                        }

                        light.SetState(cancellationToken, rndColor, waitTime() / 2, bri, waitTime() / 2);
                    }

                    await Task.Delay(waitTime(), cancellationToken);

                    for (var ch = 0; ch < ChangeAmount; ch++)
                        HiLowBrightness();

                    bri = HiLowBrightness();
                    foreach (var light in layer)
                    {
                        light.SetBrightness(cancellationToken, bri, waitTime() / 2);
                    }

                    await Task.Delay(waitTime(), cancellationToken);
                }
            }
        }
    }
}