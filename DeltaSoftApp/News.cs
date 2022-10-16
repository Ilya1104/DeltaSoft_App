using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DeltaSoftApp
{
    [Table("News")]
    public class News
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int ID { get; set; }
        public string Theme { get; set; }      
        public string Description { get; set; }
        public string Preview { get; set; }
    }
}
