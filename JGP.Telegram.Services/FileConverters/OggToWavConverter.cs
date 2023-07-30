using Concentus.Oggfile;
using Concentus.Structs;
using JGP.Telegram.Services.Builders;
using NAudio.Wave;

namespace JGP.Telegram.Services.FileConverters;

/// <summary>
///     Class ogg to wav converter
/// </summary>
public class OggToWavConverter
{
    /// <summary>
    ///     Converts the specified ogg audio file to wav.
    /// </summary>
    /// <param name="oggFilePath">The ogg file path</param>
    /// <param name="chatId">The chat id</param>
    /// <returns>The wav file path</returns>
    public string? ConvertToWav(string? oggFilePath, long chatId)
    {
        if (string.IsNullOrWhiteSpace(oggFilePath)) return null;

        var wavFilePath = BuildWavFilePath(directory: null, chatId);

        using var fileStream = new FileStream(oggFilePath, FileMode.Open, FileAccess.Read);
        using var memoryStream = new MemoryStream();
        var decoder = OpusDecoder.Create(48000, 1);
        var oggReadStream = new OpusOggReadStream(decoder, fileStream);

        WritePackets(oggReadStream, memoryStream);
        memoryStream.Position = 0;

        var sampleProvider = new RawSourceWaveStream(memoryStream, new WaveFormat(48000, 1))
            .ToSampleProvider();

        WaveFileWriter.CreateWaveFile16(wavFilePath, sampleProvider);

        return wavFilePath;
    }

    private static string BuildWavFilePath(string? directory, long chatId)
    {
        directory = string.IsNullOrWhiteSpace(directory)
            ? DirectoryBuilder.Build(chatId)
            : $"{directory}\\{chatId}";

        _ = Directory.CreateDirectory(directory);
        return Path.Combine(directory, $"{Guid.NewGuid()}.wav");
    }

    /// <summary>
    ///     Writes the packets using the specified ogg read stream
    /// </summary>
    /// <param name="oggReadStream">The ogg read stream</param>
    /// <param name="memoryStream">The memory stream</param>
    private static void WritePackets(OpusOggReadStream oggReadStream, Stream memoryStream)
    {
        while (oggReadStream.HasNextPacket)
        {
            var packet = oggReadStream.DecodeNextPacket();
            if (packet == null) continue;
            WritePacketsToMemoryStream(packet, memoryStream);
        }
    }

    /// <summary>
    ///     Writes the packets to memory stream using the specified packet
    /// </summary>
    /// <param name="packet">The packet</param>
    /// <param name="memoryStream">The memory stream</param>
    private static void WritePacketsToMemoryStream(IReadOnlyList<short> packet, Stream memoryStream)
    {
        for (var i = 0; i < packet.Count; i++)
        {
            var bytes = BitConverter.GetBytes(packet[i]);
            memoryStream.Write(bytes, 0, bytes.Length);
        }
    }
}