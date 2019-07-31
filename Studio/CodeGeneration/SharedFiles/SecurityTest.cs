using System;
using System.IO;
using System.Reflection;

namespace EntitySpaces
{
	/// <summary>
	/// Summary description for StrongNameTest.
	/// </summary>
	internal class SecurityTest
	{
		// Used to read assembly
		FileStream m_FileStream;

		// Cache of external properties - CLR initialises to false
		private bool m_IsStrongNameValid_Cache;
		private bool m_IsPublicTokenOkay_Cache;
		private bool m_IsStrongSignatureValid_Cache;

		// Internal state of properties - CLR initialises to false
		private bool m_IsStrongNameValid_Init;
		private bool m_IsPublicTokenOkay_Init;
		private bool m_IsStrongSignatureValid_Init;

        // 12 7b cb 8c ee db e2 20
        private byte[] m_TokenExpected = { 0x12, 0x7B, 0xCB, 0x8C, 0xEE, 0xDB, 0xE2, 0x20 };

		// Empty constructor 
        internal SecurityTest()
		{
		}

		// Constructor taking the expected public key token
        internal SecurityTest(byte[] tokenExpected)
		{
			m_TokenExpected = tokenExpected;
		}

		// This section contains public implementation of properties

		// Make sure that no managed or un-managed debugger is attached to this assembly.
        internal bool IsDebuggerOkay
		{
			get 
			{
				return !
				( System.Diagnostics.Debugger.IsAttached || NativeMethods.UnmanagedDebuggerPresent() ) ;
			}
		}

		// Make sure that nobody has turned off CAS.
        internal bool IsSecurityEnabled
		{
			get {return System.Security.SecurityManager.SecurityEnabled;}
		}

		// Check that this assembly has a strong name
		// that was verified and no tampering has occurred.
        internal bool IsStrongNameValid
		{
			get 
			{
				if (m_IsStrongNameValid_Init == true)
					return m_IsStrongNameValid_Cache;
				else
				{
					m_IsStrongNameValid_Cache = false;
					m_IsStrongNameValid_Cache = IsStrongNameValid_Check();
					m_IsStrongNameValid_Init = true;
					return m_IsStrongNameValid_Cache;
				}
			}
		}

		// Check that public key token matches what it should be.
        internal bool IsPublicTokenOkay
		{
			get 
			{
				if (m_IsPublicTokenOkay_Init == true)
					return m_IsPublicTokenOkay_Cache;
				else
				{
					m_IsPublicTokenOkay_Cache = false;
					m_IsPublicTokenOkay_Cache = IsPublicTokenOkay_Check(m_TokenExpected);
					m_IsPublicTokenOkay_Init = true;
					return m_IsPublicTokenOkay_Cache;
				}
			}
		}

		// Make sure that strong name signature is non-zero.
		// This checks for a CLR 1.0/1.1 bug where a hacker can skip
		// strong name verification by zeroising a single byte at
		// the start of the strong name signature.
        internal bool IsStrongSignatureValid
		{
			get 
			{
				if (m_IsStrongSignatureValid_Init == true)
					return m_IsStrongSignatureValid_Cache;
				else
				{
					m_IsStrongSignatureValid_Cache = false;
					m_IsStrongSignatureValid_Cache = IsStrongSignatureValid_Check();
					m_IsStrongSignatureValid_Init = true;
					return m_IsStrongSignatureValid_Cache;
				}
			}
		}

		// Check all of the security tests together.
        internal bool IsAllSecurityOkay
		{
			get
			{
				return (this.IsDebuggerOkay && this.IsStrongSignatureValid &&
						this.IsStrongNameValid && this.IsPublicTokenOkay &&
						this.IsSecurityEnabled);
			}
		}

		// This section contains private implementation of properties

		// Check that this assembly has a strong name.
		private bool IsStrongNameValid_Check()
		{
			byte wasVerified = Convert.ToByte(false); byte forceVerification = Convert.ToByte(true);
			string assemblyName = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName; 
			return NativeMethods.CheckSignature(assemblyName, forceVerification, ref wasVerified);
		}

		// Check that public key token matches what's expected.
		private static bool IsPublicTokenOkay_Check(byte [] tokenExpected)
		{
			// Retrieve token from current assembly
			byte [] tokenCurrent = Assembly.GetExecutingAssembly().GetName().GetPublicKeyToken();

			// Check that lengths match
			if (tokenExpected.Length == tokenCurrent.Length)
			{
				// Check that token contents match
				for (int i = 0; i < tokenCurrent.Length; i++)
					if (tokenExpected[i] != tokenCurrent[i]) 
						return false;
			}
			else
			{
				return false;
			}

			return true;
		}

		// Make sure that strong name signature is non-zero.
		// This checks for a CLR bug where a hacker can skip
		// strong name verification by zeroising a single byte.
		private bool IsStrongSignatureValid_Check()
		{
		
			// Locations and sizes of various things in the PE file
			const int pePos = 0x003c;
			const int numSectOffset = 0x02;
			const int peIdentSize = 0x04;
			const int coffHeaderSize = 0x14;
			const int dataDirectoryOffset = 0x60;
			const int dataDirectoryLength = 0x08;
			const int clrHeaderIndex = 0x0e;
			const int strongNameSigOffset = 0x20;
			const int sectionHeaderSize = 0x28;
			const int sectionRVAOffset = 0x0c;
			const int sectionRawOffset = 0x14;
			string assemblyName = AppDomain.CurrentDomain.FriendlyName; 

			using (m_FileStream = File.Open(assemblyName, FileMode.Open, FileAccess.Read))
			{
				// Find the pointer to the PE identifier before the COFF header 
				int coffHeader = GetInt(pePos);
				// Get the number of sections from the COFF header
				short numSections = GetWord(coffHeader + peIdentSize + numSectOffset);

				// Calculate the location of the PE header
				int peHeader = coffHeader + coffHeaderSize + peIdentSize;
				// Determine the location of the data directories
				int dataDirectories = peHeader + dataDirectoryOffset;
				// Determine the location of the CLR directory
				int clrHeaderDD = dataDirectories + (dataDirectoryLength * clrHeaderIndex);

				// Read the RVA of the CLR header
				int clrHeader = GetInt(clrHeaderDD);

				// Note that CLR header is stored in the .text section, so read this section
				// so that we can convert between RVA and actual file offsets
				int textRVA = 0;
				int textRaw = 0;
				// Determine the location of the section headers
				int sections = clrHeaderDD + (2 * dataDirectoryLength);
				// Iterate through the section headers until we find the header for the 
				// .text section
				byte[] bText = {0x2e, 0x74, 0x65, 0x78, 0x74, 0x00, 0x00, 0x00};
				for (int idx = 0; idx < numSections; ++idx)
				{
					// Read the first eight bytes which will have the name of the section
					// Note that this is NOT a NUL terminated string, the section name
					// can be 8 characters.
					int sectionStart = sections + (idx * sectionHeaderSize);
					byte[] eightBuf = GetEightBytes(sectionStart);
					if (!Compare(eightBuf, bText)) continue;

					// We have the right section so get the values
					textRVA = GetInt(sectionStart + sectionRVAOffset);
					textRaw = GetInt(sectionStart + sectionRawOffset);
				}

				// Convert RVA to file offset
				clrHeader = clrHeader - textRVA + textRaw;
				// Calculate the file offset of the strong name data directory 
				int strongNameDataDirectory = clrHeader + strongNameSigOffset;
				// Get the strong name signature itself
				int strongNameSigFirstByte = GetInt(strongNameDataDirectory);	

				// If first byte of strong name sig is zero, we have a problem
				return (strongNameSigFirstByte != 0);
			}
		}

		// From here are the helper procedures

		// Do a character by character comparison
		private static bool Compare(byte[] lhs, byte[] rhs)
		{
			int size = (lhs.Length > rhs.Length) ? rhs.Length : lhs.Length;
			for (int idx = 0; idx < size; ++idx)
			{
				if (lhs[idx] != rhs[idx]) return false;
			}
			return true;
		}

		// Read a 16-bit value from the file
		private short GetWord(int pos)
		{
			byte[] wordBuf = new byte[2];
			m_FileStream.Seek(pos, SeekOrigin.Begin);
			m_FileStream.Read(wordBuf, 0, wordBuf.Length);
			return BitConverter.ToInt16(wordBuf, 0);
		}
		
		// Read a 32-bit value from the file
		private int GetInt(int pos)
		{
			byte[] intBuf = new byte[4];
			m_FileStream.Seek(pos, SeekOrigin.Begin);
			m_FileStream.Read(intBuf, 0, intBuf.Length);
			return BitConverter.ToInt32(intBuf, 0);
		}
		
		// Read an eight-byte array from the file
		private byte[] GetEightBytes(int pos)
		{
			byte[] eightBuf = new byte[8];
			m_FileStream.Seek(pos, SeekOrigin.Begin);
			m_FileStream.Read(eightBuf, 0, eightBuf.Length);
			return eightBuf;
		}
	}
}
