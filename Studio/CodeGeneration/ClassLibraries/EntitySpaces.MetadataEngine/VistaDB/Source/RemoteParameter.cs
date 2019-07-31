using System;
using System.Text;

namespace Provider.VistaDB
{
	internal class RemoteParameter
	{
		private enum ServerNativeType: ushort
		{
			Character = 1,
			Date      = 9,
			DateTime  = 11,
			Boolean   = 5,
			Blob      = 15,
			Currency  = 8,
			Int32     = 3,
			Int64     = 25,
			Double    = 6,
			Guid      = 35
		}

		private VistaDBType type;
		private string      name;
		private object      val;

		public RemoteParameter(string name, VistaDBType type, object val)
		{
			this.type = type;
			this.name = name;
			this.val  = val;
		}

		private int PutParamInfo(byte[] buffer, int offset, int dataSize, ushort serverDataType, Encoding encoding)
		{
			encoding.GetBytes(serverDataType.ToString("X8")).CopyTo(buffer, offset);
			offset += 8;
			encoding.GetBytes(serverDataType.ToString("X2")).CopyTo(buffer, offset);
			offset += 2;
			encoding.GetBytes("01").CopyTo(buffer, offset);
			offset += 2;
			encoding.GetBytes(this.name.PadRight(20)).CopyTo(buffer, offset);
			offset += 20;

			return offset;
		}

		private void CurrToBcd(byte[] buffer, int offset)
		{
			//Convert decimal to TBcd
			long i64   = (long)((decimal)this.val * 10000);
			int first  = 16;
			byte nibble;

			buffer[offset]      = 32;
			buffer[offset + 1]  = i64 < 0 ? (byte)0x80: (byte)0x00;
			buffer[offset + 1] |= 4;

			for(int i = first - 1; i >= 0; i--)
			{
				if(i64 == 0)
					buffer[i + offset + 2] = 0;
				else
				{
					nibble                 = (byte)(i64 % 100);
					buffer[i + offset + 2] = (byte)(((nibble / 10) << 4) | (nibble % 10));
					i64                   /= 100;
				}
			}
		}

		public int PutParameterToBuffer(byte[] buffer, int offset, bool getSize)
		{
			int           size     = 0;
			string        s;
			Encoding encoding = Encoding.Default;

			if(this.val == null)
			{
				ushort type = 0;

				switch(this.type)
				{
					case VistaDBType.Character:
					case VistaDBType.Varchar:
					case VistaDBType.Memo:
						type = (ushort)ServerNativeType.Character;
						break;
					case VistaDBType.Date:
						type = (ushort)ServerNativeType.Date;
						break;
					case VistaDBType.DateTime:
						type = (ushort)ServerNativeType.DateTime;
						break;
					case VistaDBType.Boolean:
						type = (ushort)ServerNativeType.Boolean;
						break;
					case VistaDBType.Picture:
					case VistaDBType.Blob:
						type = (ushort)ServerNativeType.Blob;
						break;
					case VistaDBType.Currency:
						type = (ushort)ServerNativeType.Currency;
						break;
					case VistaDBType.Int32:
						type= (ushort)ServerNativeType.Int32;
						break;
					case VistaDBType.Int64:
						type = (ushort)ServerNativeType.Int64;
						break;
					case VistaDBType.Double:
						type = (ushort)ServerNativeType.Double;
						break;
					case VistaDBType.Guid:
						type = (ushort)ServerNativeType.Guid;
						break;
				}

				if(!getSize)
				{
					offset = PutParamInfo(buffer, offset, 0, type, encoding);
				}
			}
			else
			{
				switch(this.type)
				{
					case VistaDBType.Character:
					case VistaDBType.Varchar:
					case VistaDBType.Memo:
						s = (string)this.val;
						size = s.Length + 1;
						if(!getSize)
						{
							offset = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Character, encoding);
							encoding.GetBytes(s).CopyTo(buffer, offset);
							buffer[offset + s.Length] = 0;
						}
						break;
					case VistaDBType.Date:
						size = 4;
						if(!getSize)
						{
							long timeStamp = ((DateTime)this.val).Ticks / 10000 + 86400000;
							int  date      = (int)(timeStamp / VistaDBAPI.MSecsPerDay);
							offset         = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Date, encoding);
							BitConverter.GetBytes(date).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.DateTime:
						size = 8;
						if(!getSize)
						{
							double dateTime  = ((DateTime)this.val).Ticks / 10000;
							offset          = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.DateTime, encoding);
							BitConverter.GetBytes(dateTime).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.Boolean:
						size = 2;
						if(!getSize)
						{
							ushort b = (bool)this.val ? (ushort)1: (ushort)0;
							offset   = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Boolean, encoding);
							BitConverter.GetBytes(b).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.Picture:
					case VistaDBType.Blob:
						size = ((byte[])this.val).Length;
						if(!getSize)
						{
							offset = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Blob, encoding);
							((byte[])val).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.Currency:
						size = 34;
						if(!getSize)
						{
							offset    = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Currency, encoding);
							CurrToBcd(buffer, offset);
						}
						break;
					case VistaDBType.Int32:
						size = 4;
						if(!getSize)
						{
							offset = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Int32, encoding);
							BitConverter.GetBytes((int)this.val).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.Int64:
						size = 8;
						if(!getSize)
						{
							offset = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Int64, encoding);
							BitConverter.GetBytes((long)this.val).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.Double:
						size = 8;
						if(!getSize)
						{
							offset = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Double, encoding);
							BitConverter.GetBytes((double)this.val).CopyTo(buffer, offset);
						}
						break;
					case VistaDBType.Guid:
						s = "{" + this.val.ToString() + "}";
						size     = s.Length + 1;
						if(!getSize)
						{
							offset = PutParamInfo(buffer, offset, size, (ushort)ServerNativeType.Guid, encoding);
							encoding.GetBytes(s).CopyTo(buffer, offset);
							buffer[offset + s.Length] = 0;
						}
						break;
				}
			}

			size += 8 + 2 + 2 + 20;
			return size;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public VistaDBType DataType
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		public object Value
		{
			get
			{
				return val;
			}
			set
			{
				this.val = value;
			}
		}
	}

	internal class RemoteParameterCollection
	{
		private RemoteParameter[] parameters;

		public RemoteParameterCollection()
		{
			this.parameters = null;
		}

		private void Add(string name, VistaDBType dataType, object val)
		{
			int len = this.parameters != null ? this.parameters.Length : 0;
			RemoteParameter[] newParameters = new RemoteParameter[len + 1];

			if(this.parameters != null)
				this.parameters.CopyTo(newParameters, 0);

			newParameters[len] = new RemoteParameter(name, dataType, val);
			this.parameters = newParameters;
		}

		public void SetParameter(string name, VistaDBType dataType, object val)
		{
			if(this.parameters != null)
			{
				foreach(RemoteParameter p in this.parameters)
					if(p.Name == name)
					{
						p.DataType = dataType;
						p.Value = val;
					}
			}

			Add(name, dataType, val);
		}

		public int GetParameters(byte[] buffer, int offset, bool getSize)
		{
			int len = 0;

			//Put parameters to buffer
			if(this.parameters != null)
			{
				foreach(RemoteParameter p in this.parameters)
					len += p.PutParameterToBuffer(buffer, offset + len, getSize);
			}

			return len;
		}
	}
}
