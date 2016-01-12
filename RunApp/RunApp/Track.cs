using System;

using Android.Graphics; // For PointF

namespace RunApp
{
    // Class to save track points
    class Track
    {
        public PointF currentLocation;      // Variable for the current location
        public TimeSpan currentTime;        // Variable for the current time

        public Track(PointF currentLocation, TimeSpan currentTime)
        {
            this.currentLocation = currentLocation;
            this.currentTime = currentTime;
        }
    }
}