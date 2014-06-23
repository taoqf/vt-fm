﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.Adapter
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// 缓冲对象
	/// </summary>
	/// <remarks>缓冲对象</remarks>
	public class VEntry<T>
	{
		/// <summary>
		/// 对象实体
		/// </summary>
		public virtual T EntryObject
		{
			get;
			set;
		}

		/// <summary>
		/// 创建时间
		/// </summary>
		public virtual DateTime CreateDate
		{
			get;
			set;
		}

		/// <summary>
		/// 失效时间
		/// </summary>
		public virtual DateTime LastDate
		{
			get;
			set;
		}

		/// <summary>
		/// 是否刷新
		/// </summary>
		public virtual int Refresh
		{
			get;
			set;
		}

		public VEntry()
		{

		}

		public VEntry(T obj)
		{
            this.EntryObject = obj;
		}

		public VEntry(T obj, DateTime createDate)
		{
            this.EntryObject = obj;
            this.CreateDate = createDate;
            this.LastDate = createDate;
		}

	}
}

