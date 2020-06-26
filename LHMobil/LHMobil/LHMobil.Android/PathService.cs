using Android.App;
using LHMobil.Droid;


[assembly: Xamarin.Forms.Dependency(typeof(PathService))]
namespace LHMobil.Droid
{
    public class PathService : Interface.IPathService
    {
        public string InternalFolder
        {
            get
            {
                return Android.App.Application.Context.FilesDir.AbsolutePath;
            }
        }

        public string PublicExternalFolder
        {
            get
            {
                return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            }
        }

        public string PrivateExternalFolder
        {
            get
            {
                return Application.Context.GetExternalFilesDir(null).AbsolutePath;
            }
        }
    }
}