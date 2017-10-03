using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncrediblePoems.Models
{
    public class Poem
    {

        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        [DisplayName("Date Added")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Category { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
	    public string Audio { get; set; }
    }
}