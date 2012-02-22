using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace kwm
{
    public class KwmMisc
    {
        /// <summary>
        /// Return the resource image associated to the given key.
        /// </summary>
        public static Image GetKwmStateImageFromKey(Color key)
        {
            if (key == Color.Gray)
                return kwm.Properties.Resources.connecting_3;
            else if (key == Color.Green)
                return kwm.Properties.Resources.connected2;
            else
                return kwm.Properties.Resources.error;
        }
    }
}
