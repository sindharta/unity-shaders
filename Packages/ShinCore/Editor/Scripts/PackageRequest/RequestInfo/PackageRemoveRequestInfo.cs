﻿using System;                               //Action

namespace Shin.Core.Editor {
internal class PackageRemoveRequestInfo{
    internal readonly string PackageName;
    internal readonly Action OnSuccessAction;
    internal readonly Action OnFailAction;

    internal PackageRemoveRequestInfo(string packageName,
        Action onSuccess, Action onFail)
    {
        PackageName = packageName;
        OnSuccessAction = onSuccess;
        OnFailAction = onFail;
    }
}

} //namespace Shin.Core
