using System;
using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using BA_MobileGPS.Core.Droid.DependencyServices;
using BA_MobileGPS.Core.Interfaces;
using Java.IO;
using Java.Util;
using Plugin.CurrentActivity;
using Plugin.Media;
using Xamarin.Forms;
using static Android.Provider.MediaStore;

[assembly: Dependency(typeof(CameraSnapShotServices))]

namespace BA_MobileGPS.Core.Droid.DependencyServices
{
    public class CameraSnapShotServices : ICameraSnapShotServices
    {

        public string GetFolderPath()
        {
            return CrossCurrentActivity.Current.AppContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).Path;
        }


        public bool SaveSnapShotToGalery(string oldPath)
        {
            try
            {
                BitmapFactory.Options bmOptions = new BitmapFactory.Options();
                Bitmap bitmap = BitmapFactory.DecodeFile(oldPath, bmOptions);

                var name = DateTime.Now.ToString("yyyyMMddHHmmss");
                var IMAGES_FOLDER_NAME = "BAGPS_CAMERA";

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                {
                    ContentResolver resolver = CrossCurrentActivity.Current.Activity.ContentResolver;
                    ContentValues contentValues = new ContentValues();
                    contentValues.Put(MediaColumns.DisplayName, name);
                    contentValues.Put(MediaColumns.MimeType, "image/jpg");
                    contentValues.Put(MediaColumns.RelativePath, "DCIM/" + IMAGES_FOLDER_NAME);
                    var imageUri = resolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
                    var fos = resolver.OpenOutputStream(imageUri);
                    var saved = bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fos);
                    fos.Flush();
                    fos.Close();
                }
                else
                {
                    var imagesDir = Android.OS.Environment.GetExternalStoragePublicDirectory(
                            Android.OS.Environment.DirectoryDcim).ToString() + "/" + IMAGES_FOLDER_NAME;
               
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);
                    var filePath = System.IO.Path.Combine(imagesDir, name);
                    var stream = new FileStream(filePath, FileMode.Create);
                    bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
      
            return true;
           
        }


    }
}