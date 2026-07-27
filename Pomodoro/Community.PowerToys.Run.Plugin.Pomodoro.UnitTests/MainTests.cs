using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Community.PowerToys.Run.Plugin.Pomodoro.Models;
using Community.PowerToys.Run.Plugin.Pomodoro.Services;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.Pomodoro.UnitTests
{
    [TestClass]
    public class MainTests
    {
        private Main main;

        [TestInitialize]
        public void TestInitialize()
        {
            main = new Main();
        }

        [TestMethod]
        public void Query_should_return_results()
        {
            var results = main.Query(new("search"));
            Assert.IsNotNull(results.First());
        }

        [TestMethod]
        public void LoadContextMenus_should_return_results()
        {
            var results = main.LoadContextMenus(new Result { ContextData = "search" });
            Assert.IsNotNull(results.First());
        }

        [TestMethod]
        public void Plugin_should_have_correct_id()
        {
            Assert.AreEqual("6884550EBA0A4A82B090AA19C01F9B38", Main.PluginID);
        }

        [TestMethod]
        public void AdditionalOptions_should_include_media_and_hook_settings()
        {
            var options = main.AdditionalOptions.ToList();

            // Media control options
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.MediaPlayOnSessionStart)),
                "MediaPlayOnSessionStart option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.MediaPauseOnSessionEnd)),
                "MediaPauseOnSessionEnd option should be present");

            // CLI hook options
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnPomodoroStart)),
                "HookOnPomodoroStart option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnPomodoroEnd)),
                "HookOnPomodoroEnd option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnBreakStart)),
                "HookOnBreakStart option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnBreakEnd)),
                "HookOnBreakEnd option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnPause)),
                "HookOnPause option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnResume)),
                "HookOnResume option should be present");
            Assert.IsTrue(options.Any(o => o.Key == nameof(Main.HookOnStop)),
                "HookOnStop option should be present");
        }
    }

    [TestClass]
    public class PomodoroEventTests
    {
        [TestMethod]
        public void PomodoroEvent_should_default_empty()
        {
            var evt = new PomodoroEvent();
            Assert.AreEqual(string.Empty, evt.EventName);
            Assert.AreEqual(string.Empty, evt.SessionType);
            Assert.AreEqual(0, evt.LengthMinutes);
        }

        [TestMethod]
        public void PomodoroEvent_should_set_properties()
        {
            var evt = new PomodoroEvent
            {
                EventName = "start",
                SessionType = "Pomodoro",
                LengthMinutes = 25
            };

            Assert.AreEqual("start", evt.EventName);
            Assert.AreEqual("Pomodoro", evt.SessionType);
            Assert.AreEqual(25, evt.LengthMinutes);
        }
    }

    [TestClass]
    public class HookServiceTests
    {
        [TestMethod]
        public void ExecuteHook_empty_command_should_not_throw()
        {
            var svc = new HookService(typeof(HookServiceTests));
            svc.ExecuteHook("", new PomodoroEvent());
            svc.ExecuteHook(null, new PomodoroEvent());
            svc.ExecuteHook("   ", new PomodoroEvent());
        }
    }
}
