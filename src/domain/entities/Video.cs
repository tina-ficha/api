public class Video {
    private Stream? stream;

    private Video(Stream stream) {
        this.stream = stream;
    }

    public static Video FromStream(Stream stream) {
        return new Video(stream);
    }
}