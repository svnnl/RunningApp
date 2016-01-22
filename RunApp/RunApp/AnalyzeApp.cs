using Android.App;
using Android.Content;
using Android.OS;


namespace RunApp
{
    /// <summary>
    /// Activity for the analyze button
    /// </summary>

    [Activity(Label = "Track analysis")]
    public class AnalyzeApp : Activity
    {
        AnalyzeView analyzeView;

        public string message;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            message = Intent.GetStringExtra("message") ?? "";

            analyzeView = new AnalyzeView(this, message);

            SetContentView(analyzeView);
        }
    }
}