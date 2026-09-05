using System;
using UnityEngine;
/* 
* Most functions taken from Tween.js - Licensed under the MIT license
* at https://github.com/sole/tween.js
* Quadratic.Bezier by @fonserbc - Licensed under WTFPL license
*/
public delegate float EasingFunction(float k);



public class Easing
{
	public static float EasingMode(float alpha, EasingMode easingMode)
	{
		return EasingMode(alpha, easingMode.easingType, easingMode.interpolationType);
	}

	public static float EasingMode(float alpha, EasingType easeType, InterpolationType interpolationType)
	{
		switch (easeType)
		{
			case EasingType.Linear: return Linear(alpha);

			case EasingType.Sine:
				switch (interpolationType)
				{
					case InterpolationType.In: return Sinusoidal.In(alpha);
					case InterpolationType.Out: return Sinusoidal.Out(alpha);
					case InterpolationType.InOut: return Sinusoidal.InOut(alpha);
					default: return Sinusoidal.InOut(alpha);
				}
			case EasingType.Cubic:
				switch (interpolationType)
				{
					case InterpolationType.In: return Cubic.In(alpha);
					case InterpolationType.Out: return Cubic.Out(alpha);
					case InterpolationType.InOut: return Cubic.InOut(alpha);
					default: return Cubic.InOut(alpha);
				}
			case EasingType.Quad:
				switch (interpolationType)
				{
					case InterpolationType.In: return EaseInQuad(alpha);
					case InterpolationType.Out: return EaseOutQuad(alpha);
					case InterpolationType.InOut: return EaseInOutQuad(alpha);
					default: return EaseInOutQuad(alpha);
				}
			case EasingType.Quint:
				switch (interpolationType)
				{
					case InterpolationType.In: return Quintic.In(alpha);
					case InterpolationType.Out: return Quintic.Out(alpha);
					case InterpolationType.InOut: return Quintic.InOut(alpha);
					default: return Quintic.InOut(alpha);
				}
			case EasingType.Exponential:
				switch (interpolationType)
				{
					case InterpolationType.In: return Exponential.In(alpha);
					case InterpolationType.Out: return Exponential.Out(alpha);
					case InterpolationType.InOut: return Exponential.InOut(alpha);
					default: return Exponential.InOut(alpha);
				}
			case EasingType.Circular:
				switch (interpolationType)
				{
					case InterpolationType.In: return Circular.In(alpha);
					case InterpolationType.Out: return Circular.Out(alpha);
					case InterpolationType.InOut: return Circular.InOut(alpha);
					default: return Circular.InOut(alpha);
				}
			case EasingType.Back:
				switch (interpolationType)
				{
					case InterpolationType.In: return Back.In(alpha);
					case InterpolationType.Out: return Back.Out(alpha);
					case InterpolationType.InOut: return Back.InOut(alpha);
					default: return Back.InOut(alpha);
				}
			case EasingType.Elastic:
				switch (interpolationType)
				{
					case InterpolationType.In: return Elastic.In(alpha);
					case InterpolationType.Out: return Elastic.Out(alpha);
					case InterpolationType.InOut: return Elastic.InOut(alpha);
					default: return Elastic.InOut(alpha);
				}
			case EasingType.Bounce:
				switch (interpolationType)
				{
					case InterpolationType.In: return Bounce.In(alpha);
					case InterpolationType.Out: return Bounce.Out(alpha);
					case InterpolationType.InOut: return Bounce.InOut(alpha);
					default: return Bounce.InOut(alpha);
				}
			default: return Linear(alpha);
		}
	}

	public static float Linear(float k)
	{
		return k;
	}

	public static float EaseInQuad(float k) { return k * k; }

	public static float EaseOutQuad(float k) { return k * (2 - k); }

	public static float EaseInOutQuad(float k) { return k < .5f ? 2 * k * k : -1 + (4 - 2 * k) * k; }

	public static float EaseInCubic(float k) { return k * k * k; }

	public static float EaseOutCubic(float k) { return (--k) * k * k + 1; }

	public static float EaseInOutCubic(float k) { return k < .5 ? 4 * k * k * k : (k - 1) * (2 * k - 2) * (2 * k - 2) + 1; }

	public static float EaseInQuart(float k) { return k * k * k * k; }

	public static float EaseOutQuart(float k) { return 1 - (--k) * k * k * k; }

	public static float EaseInOutQuart(float k) { return k < .5 ? 8 * k * k * k * k : 1 - 8 * (--k) * k * k * k; }

	public static float EaseInQuint(float k) { return k * k * k * k * k; }

	public static float EaseOutQuint(float k) { return 1 + (--k) * k * k * k * k; }

	public static float EaseInOutQuint(float k) { return k < .5 ? 16 * k * k * k * k * k : 1 + 16 * (--k) * k * k * k * k; }

	public class Quadratic
	{
		public static float In(float k)
		{
			return k * k;
		}

		public static float Out(float k)
		{
			return k * (2f - k);
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return 0.5f * k * k;
			return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
		}

		/* 
			* Quadratic.Bezier(k,0) behaves like Quadratic.In(k)
			* Quadratic.Bezier(k,1) behaves like Quadratic.Out(k)
			*
			* If you want to learn more check Alan Wolfe's post about it http://www.demofox.org/bezquad1d.html
			*/
		public static float Bezier(float k, float c)
		{
			return c * 2 * k * (1 - k) + k * k;
		}
	};

	public class Cubic
	{
		public static float In(float k)
		{
			return k * k * k;
		}

		public static float Out(float k)
		{
			return 1f + ((k -= 1f) * k * k);
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return 0.5f * k * k * k;
			return 0.5f * ((k -= 2f) * k * k + 2f);
		}
	};

	public class Quartic
	{
		public static float In(float k)
		{
			return k * k * k * k;
		}

		public static float Out(float k)
		{
			return 1f - ((k -= 1f) * k * k * k);
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
			return -0.5f * ((k -= 2f) * k * k * k - 2f);
		}
	};

	public class Quintic
	{
		public static float In(float k)
		{
			return k * k * k * k * k;
		}

		public static float Out(float k)
		{
			return 1f + ((k -= 1f) * k * k * k * k);
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
			return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
		}
	};

	public class Sinusoidal
	{
		public static float In(float k)
		{
			return 1f - Mathf.Cos(k * Mathf.PI / 2f);
		}

		public static float Out(float k)
		{
			return Mathf.Sin(k * Mathf.PI / 2f);
		}

		public static float InOut(float k)
		{
			return 0.5f * (1f - Mathf.Cos(Mathf.PI * k));
		}
	};

	public class Exponential
	{
		public static float In(float k)
		{
			return k == 0f ? 0f : Mathf.Pow(1024f, k - 1f);
		}

		public static float Out(float k)
		{
			return k == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * k);
		}

		public static float InOut(float k)
		{
			if (k == 0f) return 0f;
			if (k == 1f) return 1f;
			if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
			return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
		}
	};

	public class Circular
	{
		public static float In(float k)
		{
			return 1f - Mathf.Sqrt(1f - k * k);
		}

		public static float Out(float k)
		{
			return Mathf.Sqrt(1f - ((k -= 1f) * k));
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return -0.5f * (Mathf.Sqrt(1f - k * k) - 1);
			return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
		}
	};

	public class Elastic
	{
		public static float In(float k)
		{
			if (k == 0) return 0;
			if (k == 1) return 1;
			return -Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
		}

		public static float Out(float k)
		{
			if (k == 0) return 0;
			if (k == 1) return 1;
			return Mathf.Pow(2f, -10f * k) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
			return Mathf.Pow(2f, -10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
		}
	};

	public class Back
	{
		static float s = 1.70158f;
		static float s2 = 2.5949095f;

		public static float In(float k)
		{
			return k * k * ((s + 1f) * k - s);
		}

		public static float Out(float k)
		{
			return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
		}

		public static float InOut(float k)
		{
			if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
			return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
		}
	};

	public class Bounce
	{
		public static float In(float k)
		{
			return 1f - Out(1f - k);
		}

		public static float Out(float k)
		{
			if (k < (1f / 2.75f))
			{
				return 7.5625f * k * k;
			}
			else if (k < (2f / 2.75f))
			{
				return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
			}
			else if (k < (2.5f / 2.75f))
			{
				return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
			}
			else
			{
				return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
			}
		}

		public static float InOut(float k)
		{
			if (k < 0.5f) return In(k * 2f) * 0.5f;
			return Out(k * 2f - 1f) * 0.5f + 0.5f;
		}
	};
}