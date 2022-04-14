using System;
namespace App.Models
{
	public class PagingParameters
	{
        const int maxPageSize = 50;
        public int page { get; set; } = 1;

        private int _pageSize = 10;
        public int size
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}

