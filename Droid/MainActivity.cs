using Android.App;
using Android.Widget;
using Android.OS;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Wicket.Droid {

  [Activity(Label = "Wicket", MainLauncher = true, Icon = "@mipmap/icon")]
  public class MainActivity : Activity {
    int count = 1;

    [DllImport("libnative-lib.so")]
    public extern static int TestMethod();

    [DllImport("libnative-lib.so")]
    public extern static void TestCrash();

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      AppCenter.Start("099d3894-18cc-4433-a269-fb7f3efdc0d8", typeof(Analytics), typeof(Crashes));

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      // Get our button from the layout resource,
      // and attach an event to it
      Button button = FindViewById<Button>(Resource.Id.myButton);
      button.Click += delegate { button.Text = $"{count++} clicks!"; };

      var task = Task.Run(async () => await Crashes.IsEnabledAsync());
      bool enabled = task.Result;
      if (!enabled) {
        Console.WriteLine("crashes not enabled!");
      }

      Crashes.ShouldProcessErrorReport = (ErrorReport report) => {
        // Check the report in here and return true or false depending on the ErrorReport.
        Console.WriteLine("Here1");
        return true;
      };

      var task2 = Task.Run(async () => await Crashes.HasCrashedInLastSessionAsync());
      bool crashed = task.Result;
      if (crashed) {
        Console.WriteLine("crashed last session!");
      }

      Crashes.SendingErrorReport += (sender, e) => {
        // Your code, e.g. to present a custom UI.
        Console.WriteLine("Here2");
      };

      Crashes.SentErrorReport += (sender, e) => {
        // Your code, e.g. to hide the custom UI.
        Console.WriteLine("Here3");
      };

      Crashes.FailedToSendErrorReport += (sender, e) => {
        // Your code goes here.
        Console.WriteLine("Here4");
      };

      Button crashButton1 = FindViewById<Button>(Resource.Id.crashButton1);
      crashButton1.Click += delegate {
        CrashException();
      };

      Button crashButton2 = FindViewById<Button>(Resource.Id.crashButton2);
      crashButton2.Click += delegate {
        CrashStackOverflow();
      };

      Button crashButton3 = FindViewById<Button>(Resource.Id.crashButton3);
      crashButton3.Click += delegate {
        CrashNative();
      };
    }

    void CrashException() {
      Crashes.GenerateTestCrash();
    }

    int CrashStackOverflow() {
      return CrashStackOverflow();
    }

    void CrashNative() {
      TestCrash();
    }
  }
}

