namespace CouchTo5kTracker.Classes
{
    class Mathf
    {
        /////////////////////////////////////////
        /// <summary>
        /// Clamps then returns the given value between the min and max values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        #region Clamp
        ///Clamp Function provided by Mike McGabe under CPOL 1.02 <see cref=Properties.Resources.CPOL/>
        public static T Clamp<T>(T value, T min, T max)
        where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
        #endregion

        /////////////////////////////////////////
    }
}
