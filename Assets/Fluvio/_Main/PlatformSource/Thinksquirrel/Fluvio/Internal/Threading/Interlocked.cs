// Interlocked.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.
#if !UNITY_WEBGL && !UNITY_WINRT
namespace Thinksquirrel.Fluvio.Internal.Threading
{
    public class Interlocked : IInterlocked
    {
        public int Increment(ref int location)
        {
            return System.Threading.Interlocked.Increment(ref location);
        }
        public long Increment(ref long location)
        {
            return System.Threading.Interlocked.Increment(ref location);
        }
        public long Decrement(ref long location)
        {
            return System.Threading.Interlocked.Decrement(ref location);
        }
        public int CompareExchange(ref int location1, int value, int comparand)
        {
            return System.Threading.Interlocked.CompareExchange(ref location1, value, comparand);
        }
        public long CompareExchange(ref long location1, long value, long comparand)
        {
            return System.Threading.Interlocked.CompareExchange(ref location1, value, comparand);
        }
    }
}
#endif