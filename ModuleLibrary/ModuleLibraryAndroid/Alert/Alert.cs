using System;
using Android.App;
using Android.Content;

namespace ModuleLibraryAndroid.Alert
{
    public static class Alert
    {
        public static void DisplayAlert(this Context context, string title, string body, string positiv, Action positiveAction, string negative = null)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetTitle(title);
            if(!string.IsNullOrEmpty(body))
                alert.SetMessage(body);
            alert.SetPositiveButton(positiv, (senderAlert, args) => {
                positiveAction();
            });

            if(negative != null)
                alert.SetNegativeButton(negative, (senderAlert, args) => {
                    //Toast.MakeText(Context, "Cancelled!", ToastLength.Short).Show();
                });

            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}
