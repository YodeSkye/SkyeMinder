using Android.OS;
using Android.Widget;

namespace SkyeMinder.Services
{
    public static class ToastHelper
    {
        public static void ShowToast(string message, ToastLength length = ToastLength.Short)
        {
            var context = Platform.AppContext;
            var looper = Looper.MainLooper;
            if (context != null && looper != null)
            {
                var handler = new Handler(looper);
                handler.Post(() =>
                {
                    Toast.MakeText(context, message, length)?.Show();
                });
            }
        }
    }
}