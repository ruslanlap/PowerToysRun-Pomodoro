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
    }
}
