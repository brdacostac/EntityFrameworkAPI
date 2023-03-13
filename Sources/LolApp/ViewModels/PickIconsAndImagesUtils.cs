using System;
//using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Graphics.Skia;

namespace LolApp.ViewModels
{
	public static class PickIconsAndImagesUtils
	{
		public async static Task<string> PickPhoto(float maxWidthAndHeight)
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();
            return photo != null ? await ToBase64(photo, maxWidthAndHeight) : null;
        }

        public async static Task<string> ToBase64(FileResult photo, float maxWidthAndHeight)
        {
            using (var stream = await photo.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                var image = SkiaImage.FromStream(memoryStream);
                //var image = PlatformImage.FromStream(stream);
                if(image != null)
                {
                    var newImage = image.Downsize(maxWidthAndHeight, true);
                    return newImage.AsBase64();
                }
            }
            return null;
        }
	}
}

