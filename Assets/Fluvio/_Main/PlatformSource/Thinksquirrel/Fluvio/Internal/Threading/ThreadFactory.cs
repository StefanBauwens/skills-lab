// ThreadFactory.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.
#if !UNITY_WEBGL && !UNITY_WINRT
using System;
using System.Threading;

namespace Thinksquirrel.Fluvio.Internal.Threading
{
    public class ThreadFactory : IThreadFactory
    {
        public IThread Create(Action threadStart)
        {
            return new Thread(threadStart);
        }
        public IThread Create(Action<object> parameterizedThreadStart)
        {
            return new Thread(parameterizedThreadStart);
        }

        class Thread : IThread
        {
            readonly System.Threading.Thread m_Thread;

            public Thread(Action threadStart)
            {
                m_Thread = new System.Threading.Thread(new ThreadStart(threadStart))
                {
                    Priority = ThreadPriority.Normal,
                    IsBackground = false
                };
            }

            public Thread(Action<object> parameterizedThreadStart)
            {
                m_Thread = new System.Threading.Thread(new ParameterizedThreadStart(parameterizedThreadStart));
            }
            public void Start()
            {
                m_Thread.Start();
            }
            public void Start(object parameter)
            {
                m_Thread.Start(parameter);
            }
            public void Join()
            {
                m_Thread.Join();
            }
            public bool hasNotStarted { get { return m_Thread.ThreadState == ThreadState.Unstarted; } }
        }        
    }
}
#endif