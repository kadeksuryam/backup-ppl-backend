using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace App.Helpers
{
	public class DateTimeModelConverter : ValueConverter<DateTime, string>
	{
		public DateTimeModelConverter() : base(v => v.ToUniversalTime().ToString(
					"yyyy-MM-ddTHH:mm:ssZ"), v => DateTime.ParseExact(v,
						"yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime())
		{
		}
	}
}
