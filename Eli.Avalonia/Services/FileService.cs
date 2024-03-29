﻿using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eli.Avalonia.Services;

public interface FileDialogService
{
    Task<IStorageFile?> OpenFileAsync();

    Task<ICollection<IStorageFile>?> OpenMultipeFilesAsync();

    Task<IStorageFile?> SaveFileAsync();
}