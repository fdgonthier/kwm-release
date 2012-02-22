using System;
using System.Collections.Generic;
using System.Text;

namespace kwm.Utils
{
    /// <summary>
    /// Exception class raised when receiving a kanp failure with KANP_RES_FAIL_RESOURCE_QUOTA as the reason
    /// </summary>

    public class LicenseRestrictionException : Exception
    {
        private UInt32 resriction;
        private string serverMsg;

        public UInt32 Restriction
        {
            get
            {
                return resriction;
            }
        }


        public LicenseRestrictionException(string errorMessage, UInt32 resriction)
            : base(errorMessage)
        {
            this.resriction = resriction;
            this.serverMsg = errorMessage;
        }

    }
}
