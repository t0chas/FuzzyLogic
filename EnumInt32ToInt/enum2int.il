.assembly extern mscorlib
{
  .ver 0:0:0:0
}
.assembly 'enum2int'
{
  .hash algorithm 0x00008004
  .ver  0:0:0:0
}

.class public auto ansi beforefieldinit EnumInt32ToInt
    extends [mscorlib]System.Object
{
    .method public hidebysig static int32  Convert<valuetype 
        .ctor ([mscorlib]System.ValueType) TEnum>(!!TEnum 'value') cil managed
    {
      .maxstack  8
      IL_0000:  ldarg.0
      IL_000b:  ret
    }
} 