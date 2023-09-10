using System.Diagnostics;
using ECS;
using FixedMath.NET;


public class GameRunner
{
    private readonly World world;
    private readonly Thread thread;
    private readonly long fixedDeltaTimeMilliseconds;
    private readonly Fix64 fixedDeltaTimeFix64;

    private bool isRunning;


    public GameRunner(World world, int tickRate)
    {
        this.world = world;
        this.thread = new Thread(UpdateThread);
        this.fixedDeltaTimeMilliseconds = 1000 / tickRate;
        this.fixedDeltaTimeFix64 = (Fix64)fixedDeltaTimeMilliseconds / (Fix64)1000;
    }


    public void Run()
    {
        Debug.Assert(!isRunning);

        isRunning = true;
        thread.Start();
    }

    public void Pause()
    {
        Debug.Assert(isRunning);

        isRunning = false;
        thread.Join();
    }

    private void UpdateThread()
    {        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        long lastUpdateTime = 0;

        while (isRunning)
        {
            long deltaTime = stopwatch.ElapsedMilliseconds - lastUpdateTime; // 밀리세컨드 단위
            long updateAmount = deltaTime / fixedDeltaTimeMilliseconds;

            for (long i = 0; i < updateAmount; i++)
            {
                world.Tick(fixedDeltaTimeFix64);
                lastUpdateTime += fixedDeltaTimeMilliseconds;
            }
        }

        stopwatch.Stop();
    }
}