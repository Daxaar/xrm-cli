﻿using System.Collections.Generic;

namespace Octono.Xrm.Tasks
{
    public interface ILog
    {
        void Write(string message, bool withTimestamp=true);
        string Prompt(string message);
        List<string> History { get; }
    }
}