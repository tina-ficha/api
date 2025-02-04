namespace TinaFicha.Ports.In.Publish;

public record PublishVideoCommand(
    Video video,
    HashSet<string> platforms
);