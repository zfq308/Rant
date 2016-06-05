﻿using System.Collections.Generic;

namespace Rant.Internal.Engine.ObjectModel
{
    /// <summary>
    /// Stores global and local variables for a single engine instance.
    /// </summary>
    internal class ObjectTable
    {
        internal readonly Dictionary<string, RantObject> Globals = new Dictionary<string, RantObject>(); 

        public ObjectStack CreateLocalStack() => new ObjectStack(this);

        public RantObject this[string name]
        {
            get
            {
                if (!Util.ValidateName(name)) return null;
                RantObject obj;
                return Globals.TryGetValue(name, out obj) ? obj : null;
            }
            set
            {
                if (!Util.ValidateName(name)) return;
                if (value == null) Globals.Remove(name);
                Globals[name] = value;
            }
        }
    }
}