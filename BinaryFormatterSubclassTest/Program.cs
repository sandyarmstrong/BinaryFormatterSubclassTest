using System;
using System.IO;

namespace BinaryFormatterSubclassTest
{
	class Program
	{
		static void Main (string [] args)
		{
			var formatter = Util.CreateBinaryFormatter ();

			// FinalClass round-trips just fine within this process.
			Util.AssertLocalRoundTrip (formatter);

			using (var fileStream = File.OpenRead (Util.GetTestFilePath ())) {
				var obj2 = formatter.Deserialize (fileStream) as FinalClass;
				if (obj2 == null)
					throw new Exception ("something went wrong. check temp path");

				// This check will fail. The property defined in BaseClass
				// will be unset.
				if (obj2.MissingValue != BaseClass.DefaultValue)
					throw new Exception ("Round trip between runtimes failed");
			}
		}
	}
}
