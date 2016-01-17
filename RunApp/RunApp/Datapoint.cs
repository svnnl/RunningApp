using System;

using Android.Graphics; // For PointF

namespace RunApp
{
    /// <summary>
    /// Class to save track points
    /// </summary>
    class DataPoint
    {
        public PointF currentLocation;      // Variable for the current location
        public TimeSpan currentTime;        // Variable for the current time

        public DataPoint(PointF currentLocation, TimeSpan currentTime)
        {
            this.currentLocation = currentLocation;
            this.currentTime = currentTime;
        }
    }
}