using System;
using System.Collections.Generic;
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
            var doneIds = new List<byte>();

            while (!cancellationToken.IsCancellationRequested)
            {
                Colors.Clear();

                if (Random.Next(2) != 2)
                {
                    Colors.AddRange(RGBColorPicker.DoubleDisco);
                }
                else
                {
                    Colors.AddRange(RGBColorPicker.DiscoColors);
                    Shuffle(Colors);
                }

                for (var i = 0; i < 5; i++)
                {
                    var brightness = RandomBrightness();
                    var otherBrightness = HiLowBrightness();
                    var count = 0;

                    foreach (var light in layer.OrderBy(c => Guid.NewGuid()))
                    {
                        count++;

                        if (count <= ChangeAmount)
                        {
                            var rndColor = GetNext();
                            var copyColor = new RGBColor(rndColor.ToHex());
                            light.SetState(cancellationToken, copyColor, waitTime() / 2, brightness, waitTime() / 2);
                            doneIds.Add(light.Id);
                            continue;
                        }

                        foreach (var otherLight in layer.Where(c => !doneIds.Contains(c.Id)))
                        {
                            otherLight.SetBrightness(cancellationToken, otherBrightness, waitTime() / 2);
                        }

                        SetWhiteLight(true, otherBrightness);

                        doneIds.Clear();
                        await Task.Delay(waitTime(), cancellationToken);
                        count = 0;
                    }
                }
            }
        }
    }
}