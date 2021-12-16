using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduAPI.Data
{
    public class FakeUser
    {
        public int id { get; set; }
        public Name name { get; set; }
        public string email { get; set; }
    }
    public class Name
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}
