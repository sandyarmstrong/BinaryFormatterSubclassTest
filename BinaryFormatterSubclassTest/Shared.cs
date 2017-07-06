using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BinaryFormatterSubclassTest
{
	static class Util
	{

		public static BinaryFormatter CreateBinaryFormatter ()
		{
			var formatter = new BinaryFormatter {
				Binder = new CustomBinder (),
			};
			return formatter;
		}

		public static byte [] SerializeDefaultFinalClass (BinaryFormatter formatter)
		{
			var memoryStream = new MemoryStream ();
			formatter.Serialize (memoryStream, new FinalClass ());
			return memoryStream.ToArray ();
		}

		/// <summary>
		/// Verify that we can round-trip FinalClass within the same process
		/// (in practice this also works within the same runtime)
		/// </summary>
		public static void AssertLocalRoundTrip (BinaryFormatter formatter)
		{
			var bytes = SerializeDefaultFinalClass (formatter);
			var obj = formatter.Deserialize (new MemoryStream (bytes)) as FinalClass;
			if (obj?.MissingValue != BaseClass.DefaultValue)
				throw new Exception ("Round trip on same runtime failed");
		}

		public static string GetTestFilePath ()
			=> Path.Combine (Path.GetTempPath (), "BinaryFormatterSubclassTest");
	}

	[Serializable]
	class BaseClass
	{
		public const string DefaultValue = "hello";

		public string MissingValue { get; } = DefaultValue;
	}

	[Serializable]
	class FinalClass : BaseClass
	{
	}

	/// <summary>
	/// A simple binder that aids remoting identical types between different assemblies.
	/// </summary>
	class CustomBinder : SerializationBinder
	{
		public virtual StreamingContext Context { get; set; }

		// Map the type name to the matching type in this assembly
		public virtual Type BindToType (string typeName)
			=> Type.GetType (typeName);

		public virtual string BindToName (Type serializedType)
			=> null;

		public sealed override Type BindToType (string assemblyName, string typeName)
			=> BindToType (typeName);

		public sealed override void BindToName (Type serializedType,
			out string assemblyName, out string typeName)
		{
			assemblyName = null;
			typeName = BindToName (serializedType);
		}
	}
}
