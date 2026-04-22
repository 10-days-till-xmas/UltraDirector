#if false // Unused for now
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace UltraDirector.CameraLogic.Audio;

public class AudioTapSink
{
    private readonly BlockingCollection<float[]> queue;
    private readonly Thread worker;

    private readonly WavWriter writer;

    public AudioTapSink(string path, int sampleRate, int channels, int capacity = 1024)
    {
        queue = new BlockingCollection<float[]>(capacity);
        writer = new WavWriter(path, sampleRate, channels);

        worker = new Thread(WorkerLoop);
        worker.Start();
    }

    public void Enqueue(ReadOnlySpan<float> data)
    {
        // NEVER block audio thread
        if (queue.IsAddingCompleted
         && !queue.TryAdd(data.ToArray()))
            LogError("Queue is already full");
    }

    private void WorkerLoop()
    {
        foreach (var frame in queue.GetConsumingEnumerable())
            writer.Write(frame);
    }

    public void Stop()
    {
        queue.CompleteAdding(); // signal end
        worker.Join();
        writer.Dispose();
        queue.Dispose();
    }
}
#endif