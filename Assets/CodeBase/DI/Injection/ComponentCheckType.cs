using System;

namespace CodeBase.DI
{
    [Flags]
    public enum ComponentCheckType
    {
        None,
        ThisObject,
        ParentObjects,
        ChildrenObjects,
        All = ThisObject | ParentObjects | ChildrenObjects
    }
}
