using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battery
{
   public class AnimationEffectTools
    {
        public static double EaseInSine(double ratio)
                {
                    return 1.0 - Math.Cos(ratio * (Math.PI / 2.0));
                }
        public static double EaseInSquare(double ratio)
        {
            return ratio*ratio;
        }

        public static double EaseInQuad(double ratio)
        {
            return ratio * ratio * ratio * ratio;
        }


    }
}
