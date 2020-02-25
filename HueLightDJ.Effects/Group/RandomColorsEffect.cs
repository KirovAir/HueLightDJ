using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects.Group
{
    [HueEffect(Name = "Random colors", HasColorPicker = false)]
    public class RandomColorsEffect : IHueGroupEffect
    {
        public Task Start(IEnumerable<IEnumerable<EntertainmentLight>> layer, Func<TimeSpan> waitTime, RGBColor? color, IteratorEffectMode iteratorMode, IteratorEffectMode secondaryIteratorMode, CancellationToken cancellationToken)
        {
            if (iteratorMode != IteratorEffectMode.All)
            {
                if (secondaryIteratorMode == IteratorEffectMode.Bounce
                    || secondaryIteratorMode == IteratorEffectMode.Cycle
                    || secondaryIteratorMode == IteratorEffectMode.Random
                    || secondaryIteratorMode == IteratorEffectMode.RandomOrdered
                    || secondaryIteratorMode == IteratorEffectMode.Single)
                {
                    Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * layer.Count()) / layer.SelectMany(x => x).Count());

                    return layer.SetRandomColor(cancellationToken, iteratorMode, secondaryIteratorMode, customWaitMS);
                }
            }

            return layer.SetRandomColor(cancellationToken, iteratorMode, secondaryIteratorMode, waitTime);
        }
    }
}