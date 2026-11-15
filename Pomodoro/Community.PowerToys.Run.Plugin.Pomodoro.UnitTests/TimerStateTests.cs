using System.Threading.Tasks;
using Community.PowerToys.Run.Plugin.Pomodoro.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Community.PowerToys.Run.Plugin.Pomodoro.UnitTests
{
    [TestClass]
    public class TimerStateTests
    {
        [TestMethod]
        public async Task PauseFreezesRemainingAndResumePreservesItAcrossRepeatedCycles()
        {
            using var service = new TickCounterApiService(typeof(TimerStateTests));
            var timer = await service.CreateTimerAsync("test", 120);
            Assert.IsNotNull(timer);
            var id = timer.TimerId;
            for (int i = 0; i < 2; i++)
            {
                Assert.IsTrue(await service.PauseTimerAsync(id));
                var paused = await service.GetTimerStatusAsync(id);
                Assert.IsNotNull(paused);
                Assert.IsTrue(paused.IsPaused);
                Assert.IsTrue(paused.RemainingSeconds > 0);
                Assert.IsFalse(await service.PauseTimerAsync(id));
                await Task.Delay(1100);
                Assert.AreEqual(paused.RemainingSeconds, (await service.GetTimerStatusAsync(id))!.RemainingSeconds);
                Assert.IsTrue(await service.ResumeTimerAsync(id));
                var resumed = await service.GetTimerStatusAsync(id);
                Assert.IsNotNull(resumed);
                Assert.IsFalse(resumed.IsPaused);
                Assert.IsTrue(resumed.RemainingSeconds >= paused.RemainingSeconds - 1);
                Assert.IsFalse(await service.ResumeTimerAsync(id));
            }
        }
        [TestMethod]
        public async Task PausingNearExpiryNeverLeavesACompletedTimerPaused()
        {
            using var service = new TickCounterApiService(typeof(TimerStateTests));
            var expired = (await service.CreateTimerAsync("expired", 0))!.TimerId;
            Assert.IsFalse(await service.PauseTimerAsync(expired));
            Assert.IsTrue((await service.GetTimerStatusAsync(expired))!.IsCompleted);
            var ids = new System.Collections.Generic.List<string>();
            for (int i = 0; i < 5000; i++)
                ids.Add((await service.CreateTimerAsync("test", 1))!.TimerId);

            await Task.Delay(990);
            foreach (var id in ids)
            {
                await service.PauseTimerAsync(id);
                var status = (await service.GetTimerStatusAsync(id))!;
                Assert.IsFalse(status.IsPaused && status.RemainingSeconds == 0);
            }
        }

        [TestMethod]
        public void MissingTimerClosesWindowWithoutCompletingSession()
        {
            System.Exception? failure = null;
            var thread = new System.Threading.Thread(() =>
            {
                try
                {
                    if (System.Windows.Application.Current == null)
                        _ = new System.Windows.Application();
                    var main = new Main();
                    using var service = new TickCounterApiService(typeof(TimerStateTests));
                    var timer = service.CreateTimerAsync("test", 60).GetAwaiter().GetResult()!;
                    var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                    typeof(Main).GetProperty("ApiService", flags)!.SetValue(main, service);
                    var window = new PomodoroResultWindow(main);
                    var session = new Community.PowerToys.Run.Plugin.Pomodoro.Models.PomodoroSession
                    {
                        TimerId = timer.TimerId, Type = "Pomodoro", LengthMinutes = 1
                    };
                    typeof(Main).GetProperty("CurrentSession", flags)!.SetValue(main, session);
                    window.SetSession(session);
                    bool closed = false;
                    window.Closed += (_, _) => closed = true;
                    window.Show();
                    Assert.IsTrue(service.StopTimerAsync(timer.TimerId).GetAwaiter().GetResult());
                    typeof(PomodoroResultWindow).GetMethod("Timer_Tick", flags)!
                        .Invoke(window, new object[] { null, System.EventArgs.Empty });
                    Assert.IsTrue(closed, "A removed timer must not leave a frozen window.");
                    Assert.AreSame(session, typeof(Main).GetProperty("CurrentSession", flags)!.GetValue(main),
                        "Stopping must not invoke completion.");
                }
                catch (System.Exception ex) { failure = ex; }
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join();
            if (failure != null) throw failure;
        }
    }
}
