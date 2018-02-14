using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;

namespace Wicket.Droid {
  [Activity(Label = "Wicket", MainLauncher = true, Icon = "@mipmap/icon")]
  public class MainActivity : Activity {
    int count = 1;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      AppCenter.Start("099d3894-18cc-4433-a269-fb7f3efdc0d8", typeof(Analytics), typeof(Crashes));

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      // Get our button from the layout resource,
      // and attach an event to it
      Button button = FindViewById<Button>(Resource.Id.myButton);

      button.Click += delegate { button.Text = $"{count++} clicks!"; };

      Button crashButton = FindViewById<Button>(Resource.Id.crashButton);

      crashButton.Click += delegate {
        Console.WriteLine("Attempting to crash our own app!");
        CrashMe2(null);
      };
    }

    int CrashMe() {
      return CrashMe();
    }

    void CrashMe2(string testStr) {
      Console.WriteLine(testStr.Length);
    }
  }
}

