﻿namespace Trigger.NET
{
    public interface IJob
    {
        void Execute(JobContext context);
    }
}