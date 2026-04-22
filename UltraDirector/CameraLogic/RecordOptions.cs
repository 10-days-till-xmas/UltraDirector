using FFMediaToolkit.Encoding;

namespace UltraDirector.CameraLogic;

public record struct RecordOptions(
    string FilePath,
    VideoOptions VideoOptions,
    AudioOptions AudioOptions);
public record struct VideoOptions(
    byte Fps = 60,
    int CRF = 18,
    EncoderPreset EncoderPreset = EncoderPreset.VeryFast) : IRecordOptions;
public record struct AudioOptions(bool RecordAudio = true) : IRecordOptions;

public interface IRecordOptions;