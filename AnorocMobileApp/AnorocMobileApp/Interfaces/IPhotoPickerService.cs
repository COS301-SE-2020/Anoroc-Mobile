using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AnorocMobileApp.Interfaces
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
        void SavePicture(string name, MemoryStream data, string location = "temp");
    }
}
