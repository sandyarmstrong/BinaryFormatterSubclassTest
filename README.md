# Cross-Runtime `BinaryFormatter` Failure Test Case

Given the following serializable classes:

```csharp
[Serializable]
class BaseClass
{
    public const string DefaultValue = "hello";
    public string MissingValue { get; } = DefaultValue;
}

[Serializable]
class FinalClass : BaseClass { }
```

When serializing on Desktop .NET (4.6.1) and deserializing on .NET Core 2.0
Preview 2, `FinalClass.MissingValue` will always be unset.

## How to run test case

1. Clone the repo
2. Run `DesktopProject` (a .NET 4.6.1 console app), which will serialize a
   default instance of `FileClass` and save it to a temp file.
3. Run `BinaryFormatterSubclassTest` (a .NET Core 2.0 Preview 2 console app),
   which will read the serailized `FileClass`, and verify that `MissingValue` is
   unset.