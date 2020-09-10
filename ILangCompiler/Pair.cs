namespace ILangCompiler
{
    
    public class Pair<T, R> {
        public Pair() {
        }

        public Pair(T first, R second) {
          this.First = first;
          this.Second = second;
        }

        public T First { get; set; }
        public R Second { get; set; }
    }
    
}