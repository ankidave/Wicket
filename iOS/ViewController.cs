using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UIKit;

namespace Wicket.iOS {
  public partial class ViewController : UIViewController {
    int count = 1;

    [DllImport("__Internal")]
    public extern static int TestMethod();

    [DllImport("__Internal")]
    public extern static void TestCrash();

    public ViewController(IntPtr handle) : base(handle) {
    }

    public override void ViewDidLoad() {
      base.ViewDidLoad();

      AppCenter.Start("d5c816c2-de4e-4089-9924-e376730f2c8a", typeof(Analytics), typeof(Crashes));

      // Perform any additional setup after loading the view, typically from a nib.
      Button.AccessibilityIdentifier = "myButton";
      Button.TouchUpInside += delegate {
        var title = string.Format("{0} clicks!", count++);
        Button.SetTitle(title, UIControlState.Normal);
      };

      CrashButton1.TouchUpInside += delegate {
        CrashException();
      };

      CrashButton2.TouchUpInside += delegate {
        CrashNative();
      };

      CrashButton3.TouchUpInside += delegate {
        CrashStackOverflow();
      };

      var task = Task.Run(async () => await Crashes.IsEnabledAsync());
      bool enabled = task.Result;
      if (!enabled) {
        Console.WriteLine("crashes not enabled!");
      }

      Crashes.ShouldProcessErrorReport = (ErrorReport report) => {
        // Check the report in here and return true or false depending on the ErrorReport.
        Console.WriteLine("asking if we should process error report");
        return true;
      };

      var task2 = Task.Run(async () => await Crashes.HasCrashedInLastSessionAsync());
      bool crashed = task.Result;
      if (crashed) {
        Console.WriteLine("crashed last session!");
      }

      Crashes.SendingErrorReport += (sender, e) => {
        // Your code, e.g. to present a custom UI.
        Console.WriteLine("on sending error report");
      };

      Crashes.SentErrorReport += (sender, e) => {
        // Your code, e.g. to hide the custom UI.
        Console.WriteLine("on sent error report");
      };

      Crashes.FailedToSendErrorReport += (sender, e) => {
        // Your code goes here.
        Console.WriteLine("failed to send error report");
      };

      Console.WriteLine($"native test: {TestMethod()}");
    }

    public override void DidReceiveMemoryWarning() {
      base.DidReceiveMemoryWarning();
      // Release any cached data, images, etc that aren't in use.    
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
