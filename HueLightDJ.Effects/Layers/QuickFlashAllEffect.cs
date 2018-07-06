using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
		  [HueEffect(Name = "Quick Flash on all lights")]
		  public class QuickFlashAllEffect : IHueEffect
		  {
					public Task Start(EntertainmentLayer layer, Ref<TimeSpan?> waitTime, RGBColor? color, CancellationToken cancellationToken)
					{
							  if (!color.HasValue)
										color = new Q42.HueApi.ColorConverters.RGBColor("FFFFFF");

							  return layer.To2DGroup().FlashQuick(color, IteratorEffectMode.All, waitTime: waitTime, cancellationToken: cancellationToken);
					}
		  }
}