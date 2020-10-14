using System;
using System.Reflection;
using System.Reflection.Emit;

public class Example
{
    private delegate int ProgramDelegate(Int32 num);

    public static void Main()
    {
        Type[] methodArgs = { typeof(Example), typeof(int) };

        DynamicMethod dm = new DynamicMethod(
            "",
            typeof(int),
            methodArgs,
            typeof(Example));
        

        ILGenerator il = dm.GetILGenerator();

        // Console.WriteLine function in the msil. Can be used as il.EmitCall(OpCodes.Call, writeString, null);
        // Type[] writeStringArgs = {typeof(string)};
        // MethodInfo writeString = typeof(Console).GetMethod("WriteLine",
        //     writeStringArgs);
        
        // Local variable declaration
        // LocalBuilder a = il.DeclareLocal(typeof(Int32));
        
        // Field declaration
        // FieldInfo testInfo = typeof(Example).GetField("test",
        //     BindingFlags.NonPublic | BindingFlags.Instance);
        
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldc_I4, 3);
        il.Emit(OpCodes.Mul);
        il.Emit(OpCodes.Ret);
        
        ProgramDelegate invoke =
            (ProgramDelegate) dm.CreateDelegate(typeof(ProgramDelegate));

        // Pass argument 14 to the Ldarg_1 of MSIL function
        Console.WriteLine("3 * test = {0}", invoke(14));
    }
}