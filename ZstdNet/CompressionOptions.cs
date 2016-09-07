﻿using System;
#if BUILD64
using size_t = System.UInt64;
#else
using size_t = System.UInt32;
#endif

namespace ZstdNet
{
	public class CompressionOptions
	{
		public CompressionOptions(byte[] dict, int compressionLevel = DefaultCompressionLevel)
			: this(compressionLevel)
		{
			Dictionary = dict;

			if (dict != null)
				Cdict = ExternMethods.ZSTD_createCDict(dict, (size_t)dict.Length, compressionLevel).EnsureZstdSuccess();
		}

		public CompressionOptions(int compressionLevel)
		{
			CompressionLevel = compressionLevel;
		}

		~CompressionOptions()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposed)
				return;
			disposed = true;

			if (Cdict != IntPtr.Zero)
				ExternMethods.ZSTD_freeCDict(Cdict);

			if (disposing)
				GC.SuppressFinalize(this);
		}

		private bool disposed = false;

		public static int MaxCompressionLevel
		{
			get { return ExternMethods.ZSTD_maxCLevel(); }
		}

		public const int DefaultCompressionLevel = 3; // Used by zstd utility by default

		public readonly int CompressionLevel;
		public readonly byte[] Dictionary;

		internal readonly IntPtr Cdict;
	}
}
