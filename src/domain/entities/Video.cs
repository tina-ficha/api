public class Video {
    public readonly Stream Stream;

    private Video(Stream stream) {
        this.Stream = stream;
    }

    public static Video FromStream(Stream stream) {
        return new Video(stream);
    }
}