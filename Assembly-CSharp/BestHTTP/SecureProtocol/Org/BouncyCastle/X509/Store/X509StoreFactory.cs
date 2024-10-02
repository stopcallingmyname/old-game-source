using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store
{
	// Token: 0x0200025C RID: 604
	public sealed class X509StoreFactory
	{
		// Token: 0x06001612 RID: 5650 RVA: 0x00022F1F File Offset: 0x0002111F
		private X509StoreFactory()
		{
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x000AFB1C File Offset: 0x000ADD1C
		public static IX509Store Create(string type, IX509StoreParameters parameters)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			string[] array = Platform.ToUpperInvariant(type).Split(new char[]
			{
				'/'
			});
			if (array.Length < 2)
			{
				throw new ArgumentException("type");
			}
			if (array[1] != "COLLECTION")
			{
				throw new NoSuchStoreException("X.509 store type '" + type + "' not available.");
			}
			ICollection collection = ((X509CollectionStoreParameters)parameters).GetCollection();
			string a = array[0];
			if (!(a == "ATTRIBUTECERTIFICATE"))
			{
				if (!(a == "CERTIFICATE"))
				{
					if (!(a == "CERTIFICATEPAIR"))
					{
						if (!(a == "CRL"))
						{
							throw new NoSuchStoreException("X.509 store type '" + type + "' not available.");
						}
						X509StoreFactory.checkCorrectType(collection, typeof(X509Crl));
					}
					else
					{
						X509StoreFactory.checkCorrectType(collection, typeof(X509CertificatePair));
					}
				}
				else
				{
					X509StoreFactory.checkCorrectType(collection, typeof(X509Certificate));
				}
			}
			else
			{
				X509StoreFactory.checkCorrectType(collection, typeof(IX509AttributeCertificate));
			}
			return new X509CollectionStore(collection);
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x000AFC2C File Offset: 0x000ADE2C
		private static void checkCorrectType(ICollection coll, Type t)
		{
			foreach (object o in coll)
			{
				if (!t.IsInstanceOfType(o))
				{
					throw new InvalidCastException("Can't cast object to type: " + t.FullName);
				}
			}
		}
	}
}
