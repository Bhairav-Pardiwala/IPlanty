using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace BackgroundApplicationRelay
{
    public sealed class SnapshotMod
    {
        MediaCapture _mediaCapture;
        CaptureElement ele;
        bool modulePwr = false;
        private async Task Init()
        {
            if(modulePwr)
            {
                try
                {
                    // ele = new CaptureElement();
                    //
                    //_mediaCapture = new MediaCapture();
                    //await _mediaCapture.InitializeAsync();
                    //ele.Source = _mediaCapture;
                    //await _mediaCapture.StartPreviewAsync();

                    ///this is done as my camera is logitech webcam c210 which takes time to warm up 
                    ///The WInrt background task although can take pictures tries to take it immediately without waiting for camera to warm up
                    ///This causes black photos to appear 
                    ///Also Workaround of starting a preview to warm up camera seems to be only supported in UWP headed app hence starting a headed app 
                    ///which does my job and exits :-)
                    await LaunchAppAsync("launchsnapshotiot://");
                    // await CapturePhoto();
                    //await _mediaCapture.StopPreviewAsync();
                }
                catch (Exception e)
                {


                }
            }
            
        }
        public IAsyncAction TakePic()
        {
            return Task.Run(Init).AsAsyncAction();
        }

        private async Task<String> CapturePhoto()
        {
            var myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
            StorageFile file = await myPictures.SaveFolder.CreateFileAsync("photo.jpg", CreationCollisionOption.GenerateUniqueName);


            using (var captureStream = new InMemoryRandomAccessStream())
            {

                await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), captureStream);

                using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var decoder = await BitmapDecoder.CreateAsync(captureStream);
                    var encoder = await BitmapEncoder.CreateForTranscodingAsync(fileStream, decoder);

                    var properties = new BitmapPropertySet {
            { "System.Photo.Orientation", new BitmapTypedValue(PhotoOrientation.Normal, PropertyType.UInt16) }
        };
                    await encoder.BitmapProperties.SetPropertiesAsync(properties);

                    await encoder.FlushAsync();
                    


                }
              
            }

           
           

            
            return "";
        }
        private async Task LaunchAppAsync(string uriStr)
        {
            Uri uri = new Uri(uriStr);
            var promptOptions = new Windows.System.LauncherOptions();
            promptOptions.TreatAsUntrusted = false;

            bool isSuccess = await Windows.System.Launcher.LaunchUriAsync(uri, promptOptions);

            //if (!isSuccess)
            //{
            //    string msg = "Launch failed";
            //   // await new MessageDialog(msg).ShowAsync();
            //}
            //else
            //{

            //}
        }

        private async Task<string> StorageFileToBase64(StorageFile file)
        {
            string Base64String = "";

            if (file != null)
            {
                IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
                var reader = new DataReader(fileStream.GetInputStreamAt(0));
                await reader.LoadAsync((uint)fileStream.Size);
                byte[] byteArray = new byte[fileStream.Size];
                reader.ReadBytes(byteArray);
                Base64String = Convert.ToBase64String(byteArray);
            }

            return Base64String;
        }
    }
}
