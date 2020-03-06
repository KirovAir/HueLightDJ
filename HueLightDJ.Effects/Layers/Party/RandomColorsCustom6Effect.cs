using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;

namespace HueLightDJ.Effects
{
    [HueEffect(Name = "Jesse SwitchUp", Group = "Party", HasColorPicker = false)]
    public class RandomColorsCustom6Effect : CustomBaseEffect, IHueEffect
    {
        public async Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
        {
            Colors = RGBColorPicker.DiscoColors;
            var center = EffectSettings.LocationCenter;
            var orderedByAngle = layer.OrderBy(x => x.LightLocation.Angle(center.X, center.Y)).ToList();

            while (!cancellationToken.IsCancellationRequested)
            {
                Shuffle(Colors);
                var rndColor = GetNext();
                var rndColor2 = GetNext();

                var lSwitch = false;
                var lastSwitch = false;

                for (var i = 0; i < layer.Count - ChangeAmount; i++)
                {
                    var bri = HiLowBrightness();

                    if (lastSwitch == lSwitch) // Do another flip.
                        lSwitch = !lSwitch;
                    
                    lastSwitch = lSwitch;

                    SetWhiteLight(true, HighLowBriIndex);
                    foreach (var light in orderedByAngle)
                    {
                        var theColor = lSwitch ? rndColor : rndColor2;
                        if (light.State.RGBColor.ToHex() != rndColor.ToHex())
                        {
                            light.SetState(cancellationToken, theColor, waitTime() / 2, bri, waitTime() / 2);
                            break;
                        }
                        lSwitch = !lSwitch;
                    }

                    await Task.Delay(waitTime(), cancellationToken);

                    bri = HiLowBrightness();
                    SetWhiteLight(true, HighLowBriIndex);
                    foreach (var light in orderedByAngle)
                    {
                        light.SetBrightness(cancellationToken, bri, waitTime() / 2);
                    }
                    
                    await Task.Delay(waitTime(), cancellationToken);
                }
            }
        }
    }
}