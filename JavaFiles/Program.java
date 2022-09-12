public class Program {

    public static int main(int a, int b) {
        return a + b;
    }

    public static void main(String[] args) {
        main(3000000000L, 5000000000L);
    }

    public static void main(long a, long b) {
        long c = a + b;
    }
}