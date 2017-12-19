// ThreadHandler.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.

#if !UNITY_WEBGL && !UNITY_WINRT
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Thinksquirrel.Fluvio.Internal.Threading
{
    public class ThreadHandler : IThreadHandler
    {
        // Use some better thread handling on Windows
        #if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        [DllImport("kernel32.dll")]
        static extern bool SwitchToThread();

        [DllImport("winmm.dll")]
        internal static extern uint timeBeginPeriod(uint period);

        [DllImport("winmm.dll")]
        internal static extern uint timeEndPeriod(uint period);
        #else
        static bool SwitchToThread()
        {
            Thread.Sleep(0);
            return true;
        }
        static uint timeBeginPeriod(uint period)
        {
            return period;
        }
        static uint timeEndPeriod(uint period)
        {
            return period;
        }
        #endif
        public void Sleep(int millisecondsTimeout)
        {
            var ms = (uint) millisecondsTimeout;
            timeBeginPeriod(ms);
            Thread.Sleep(millisecondsTimeout);
            timeEndPeriod(ms);
        }
        public void Yield()
        {
            SwitchToThread();
        }
        public void SpinWait(int iterations)
        {
            Thread.SpinWait(iterations);
        }
    }
}
#endif