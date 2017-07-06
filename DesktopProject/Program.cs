using System.IO;

namespace BinaryFormatterSubclassTest
{
	class Program
	{
		static void Main (string [] args)
		{
			var formatter = Util.CreateBinaryFormatter ();

			Util.AssertLocalRoundTrip (formatter);

			var bytes = Util.SerializeDefaultFinalClass (formatter);
			File.WriteAllBytes (Util.GetTestFilePath (), bytes);
		}
	}
}
