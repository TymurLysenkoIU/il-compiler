import sys
import os

target_namespace = sys.argv[1]
target_path = target_namespace.replace(".", "/")

base_class = sys.argv[2]

if not os.path.exists(target_path):
    os.makedirs(target_path)

for keyword in sys.argv[3:]:
  class_name = ''.join(keyword.title().split(' ')) + base_class
  code = """namespace %s
{
  public class %s : %s
  {
    public override string Lexeme => "%s";

    public %s(
      uint absolutePosition,
      uint lineNumber,
      uint positionInLine
    ) : base(absolutePosition, lineNumber, positionInLine)
    {

    }
  }
}
""" % (target_namespace, class_name, base_class, keyword, class_name)

  with open(f"../{target_path}/{class_name}.cs", "w") as f:
    f.write(code)
