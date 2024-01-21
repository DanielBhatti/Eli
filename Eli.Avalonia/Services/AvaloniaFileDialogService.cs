using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eli.Avalonia.Services;

public class AvaloniaFileDialogService : FileDialogService
{
    private Window Target { get; }

    public AvaloniaFileDialogService(Window target) => Target = target;

    public async Task<IStorageFile?> OpenFileAsync()
    {
        var files = await Target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select File",
            AllowMultiple = false
        });

        return files.Count >= 1 ? files[0] : null;
    }

    public async Task<ICollection<IStorageFile>?> OpenMultipeFilesAsync()
    {
        var files = await Target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select Files",
            AllowMultiple = true
        });

        return files.Count >= 1 ? files.ToList() : null;
    }

    public async Task<IStorageFile?> SaveFileAsync() =>
        await Target.StorageProvider.SaveFilePickerAsync(
            new FilePickerSaveOptions() { Title = "Save File" });
}
